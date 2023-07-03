#if PUN_2_OR_NEWER

using System.Collections.Generic;
using Photon.Pun;
using Photon.Voice.PUN;
using Photon.Voice.Unity;
using UnityEngine;

//[RequireComponent(typeof(Collider))]
//[RequireComponent(typeof(Rigidbody))]
public class PrivateVoicePun : MonoBehaviourPunCallbacks
{
    public List<byte> groupsToAdd = new List<byte>();
    public List<byte> groupsToRemove = new List<byte>();

    [SerializeField] // TODO: make it readonly
    private byte[] subscribedGroups;
    private bool isOut = false;
    private byte _roomGroup = 5;
    private PhotonVoiceView photonVoiceView;
    private PhotonView photonView;
    private RoomTrigger trigger;
    Collider tmpCollider;
    public byte TargetInterestGroup
    {
        get
        {
            if (this.photonView != null)
            {
                return (byte)this.photonView.OwnerActorNr;
            }
            return 0;
        }
    }

    private void Awake()
    {
        this.photonVoiceView = this.GetComponentInParent<PhotonVoiceView>();
        this.photonView = this.GetComponentInParent<PhotonView>();
        tmpCollider = this.GetComponent<Collider>();
        tmpCollider.isTrigger = true;
        this.IsLocalCheck();
    }

    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Room"))
        {
            isOut = false;
            trigger = other.GetComponent<RoomTrigger>();
            if (trigger != null && photonView.IsMine)
            {
                trigger.photonView.RPC("AddToList", RpcTarget.All, TargetInterestGroup);
                groupsToAdd = trigger._listInterestGroupAdd;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {

        if (other.CompareTag("Room"))
        {
            isOut = true;
            if (trigger != null && photonView.IsMine)
            {
                groupsToRemove = trigger._listInterestGroupRemove;
                trigger.photonView.RPC("RemoveToList", RpcTarget.All, TargetInterestGroup);
                groupsToAdd = trigger._listInterestGroupAdd;

            }
        }
    }

    private void ToggleTransmission()
    {
        if (this.photonVoiceView.RecorderInUse != null)
        {
            byte group = this.TargetInterestGroup;
            if (this.photonVoiceView.RecorderInUse.InterestGroup != group)
            {

                this.photonVoiceView.RecorderInUse.InterestGroup = group;
            }
            this.photonVoiceView.RecorderInUse.RecordingEnabled = true;
        }
    }

    private void ChangeGroupSubcrise()
    {
        if (this.groupsToAdd.Count > 0 || this.groupsToRemove.Count > 0)
        {
            byte[] toAdd = null;
            byte[] toRemove = null;
            if (this.groupsToAdd.Count > 0)
            {
                if (isOut == true)
                {
                    groupsToAdd.Clear();
                    groupsToAdd.Add(TargetInterestGroup);
                }
                toAdd = this.groupsToAdd.ToArray();

            }
            if (this.groupsToRemove.Count > 0)
            {
                toRemove = this.groupsToRemove.ToArray();
            }

            Debug.Log("groupRemove " + groupsToRemove.Count);
            Debug.Log("groupAdd " + groupsToAdd.Count);

            if (PunVoiceClient.Instance.Client.OpChangeGroups(toRemove, toAdd))
            {
                if (this.subscribedGroups != null)
                {
                    List<byte> list = new List<byte>();
                    for (int i = 0; i < this.subscribedGroups.Length; i++)
                    {
                        list.Add(this.subscribedGroups[i]);
                    }
                    for (int i = 0; i < this.groupsToRemove.Count; i++)
                    {
                        if (list.Contains(this.groupsToRemove[i]))
                        {
                            list.Remove(this.groupsToRemove[i]);
                        }
                    }
                    for (int i = 0; i < this.groupsToAdd.Count; i++)
                    {
                        if (!list.Contains(this.groupsToAdd[i]))
                        {
                            list.Add(this.groupsToAdd[i]);
                        }
                    }
                    this.subscribedGroups = list.ToArray();
                }
                else
                {
                    this.subscribedGroups = toAdd;
                }
                //this.groupsToAdd.Clear();
                //this.groupsToRemove.Clear();
            }

        }
    }

    protected void Update()
    {

        if (!PunVoiceClient.Instance.Client.InRoom)
        {
            this.subscribedGroups = null;
        }
        else if (this.IsLocalCheck())
        {
            ChangeGroupSubcrise();
            this.ToggleTransmission();
        }
    }


    private bool IsLocalCheck()
    {
        if (this.photonView.IsMine)
        {
            return true;
        }
        if (this.enabled)
        {
            this.enabled = false;
        }
        return false;
    }

}
#endif