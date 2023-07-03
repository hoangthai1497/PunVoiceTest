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

    private byte _roomGroup = 5;
    private PhotonVoiceView photonVoiceView;
    private PhotonView photonView;

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
        Debug.Log("get Photon" + photonView.ViewID);
        Collider tmpCollider = this.GetComponent<Collider>();
        tmpCollider.isTrigger = true;
        this.IsLocalCheck();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Room"))
        {
            RoomTrigger trigger = other.GetComponent<RoomTrigger>();
            if (trigger != null)
            {
                trigger._listInterestGroupAdd.Add(TargetInterestGroup);
                groupsToAdd = trigger._listInterestGroupAdd;
                Debug.Log("Add Group");
                foreach (var item in groupsToAdd)
                {
                    Debug.Log("trigger add " + item);
                }
            }

        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Room"))
        {
            RoomTrigger trigger = other.GetComponent<RoomTrigger>();
            if (trigger != null)
            {
                if (trigger._listInterestGroupAdd.Contains(TargetInterestGroup))
                {
                    Debug.Log("(trigger._listInterestGroupAdd.Contains(TargetInterestGroup)" + (trigger._listInterestGroupAdd.Contains(TargetInterestGroup)));
                    trigger._listInterestGroupAdd.Remove(TargetInterestGroup);
                    groupsToAdd = trigger._listInterestGroupAdd;
                    foreach (var item in groupsToAdd)
                    {
                        Debug.Log("TriggerExit add " + item);
                    }
                }

            }
            if (!trigger._listInterestGroupRemove.Contains(TargetInterestGroup))
            {
                groupsToAdd.Remove(TargetInterestGroup);
                trigger._listInterestGroupRemove.Add(TargetInterestGroup);
                groupsToRemove = trigger._listInterestGroupRemove;
                foreach (var item in groupsToRemove)
                {
                    Debug.Log("TriggerExit remove " + item);
                }
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



    protected void Update()
    {

        if (!PunVoiceClient.Instance.Client.InRoom)
        {
            this.subscribedGroups = null;
        }
        else if (this.IsLocalCheck())
        {
            if (this.groupsToAdd.Count > 0 || this.groupsToRemove.Count > 0)
            {
                byte[] toAdd = null;
                byte[] toRemove = null;
                if (this.groupsToAdd.Count > 0)
                {
                    toAdd = this.groupsToAdd.ToArray();
                }
                if (this.groupsToRemove.Count > 0)
                {
                    toRemove = this.groupsToRemove.ToArray();
                }
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
                    this.groupsToAdd.Clear();
                    this.groupsToRemove.Clear();
                }

            }
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