#if PUN_2_OR_NEWER

using System.Collections.Generic;
using Photon.Pun;
using Photon.Voice.PUN;
using Photon.Voice.Unity;
using UnityEngine;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class PrivateVoicePun : MonoBehaviourPunCallbacks
{
    private List<byte> groupsToAdd = new List<byte>();
    private List<byte> groupsToRemove = new List<byte>();

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
        Collider tmpCollider = this.GetComponent<Collider>();
        tmpCollider.isTrigger = true;
        this.IsLocalCheck();
    }

    private void ToggleTransmission()
    {
        if (this.photonVoiceView.RecorderInUse != null)
        {
           // byte group = this.TargetInterestGroup;
            if (this.photonVoiceView.RecorderInUse.InterestGroup != _roomGroup)
            {
               
                this.photonVoiceView.RecorderInUse.InterestGroup = _roomGroup;
            }
            this.photonVoiceView.RecorderInUse.RecordingEnabled = true;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (this.IsLocalCheck())
        {
            ProximityVoiceTrigger trigger = other.GetComponent<ProximityVoiceTrigger>();
            
            if (trigger != null)
            {
                byte group = trigger.TargetInterestGroup;
               
                if (group == this.TargetInterestGroup)
                {
                    return;
                }
                if (group == 0)
                {
                    return;
                }
                if (!this.groupsToAdd.Contains(group))
                {
                    this.groupsToAdd.Add(_roomGroup);
                }
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (this.IsLocalCheck())
        {
            ProximityVoiceTrigger trigger = other.GetComponent<ProximityVoiceTrigger>();
            if (trigger != null)
            {
                byte group = trigger.TargetInterestGroup;
               
                if (group == this.TargetInterestGroup)
                {
                    return;
                }
                if (group == 0)
                {
                    return;
                }
                if (this.groupsToAdd.Contains(_roomGroup))
                {
                    this.groupsToAdd.Remove(_roomGroup);
                }
                if (!this.groupsToRemove.Contains(_roomGroup))
                {
                    this.groupsToRemove.Add(_roomGroup);
                }
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