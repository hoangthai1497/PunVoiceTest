using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using Photon.Voice.PUN;
using UnityEngine;

public class PrivateVoicePunRemake : MonoBehaviourPunCallbacks
{
    private PhotonVoiceView photonVoiceView;
    private PhotonView photonView;
    private RoomTriggerRemake trigger;

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
        photonVoiceView = GetComponentInParent<PhotonVoiceView>();
        photonView = GetComponentInParent<PhotonView>();
        Collider tmpCollider = GetComponent<Collider>();
        tmpCollider.isTrigger = true;
        IsLocalCheck();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Room"))
        {
            Player player = photonView.Owner;
            trigger = other.GetComponent<RoomTriggerRemake>();
            

            if (trigger != null && photonView.IsMine)
            {
                trigger.photonView.RPC("AddToGroup", RpcTarget.All, player);
               // trigger.photonView.RPC("UpdatePlayerGroupList", RpcTarget.All);
                photonView.RPC("UpdateGroupRegistration", RpcTarget.All);

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Room"))
        {
            Player player = photonView.Owner;
            if (trigger != null && photonView.IsMine)
            {
                trigger.photonView.RPC("RemoveFromGroup", RpcTarget.All, player);
               //trigger.photonView.RPC("UpdatePlayerGroupList", RpcTarget.All);
                photonView.RPC("UpdateGroupRegistration", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    private void UpdateGroupRegistration()
    {
        if (trigger != null)
        {
            byte[] group = trigger.interestGroups.ToArray();
            PunVoiceClient.Instance.Client.OpChangeGroups(group, new byte[0]);
        }
    }

    private void ToggleTransmission()
    {
        if (photonVoiceView.RecorderInUse != null)
        {
            byte group = this.TargetInterestGroup;
            if (photonVoiceView.RecorderInUse.InterestGroup != group)
            {
                photonVoiceView.RecorderInUse.InterestGroup = group;
            }
            photonVoiceView.RecorderInUse.RecordingEnabled = true;
        }
    }

    private bool IsLocalCheck()
    {
        if (photonView.IsMine)
        {
            return true;
        }
        if (enabled)
        {
            enabled = false;
        }
        return false;
    }
}
