using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.Collections.Generic;
using Photon.Voice.PUN;

public class RoomTrigger : MonoBehaviour
{
    public List<byte> _listInterestGroupAdd = new List<byte>();
    public List<byte> _listInterestGroupRemove = new List<byte>();


    [PunRPC]
    private void ChangeGroupSub()
    {
        byte[] groupAdd = _listInterestGroupAdd.ToArray();
        byte[] groupRemove = _listInterestGroupRemove.ToArray();
        PunVoiceClient.Instance.Client.OpChangeGroups(groupRemove, groupAdd);
        PunVoiceClient.Instance.Client.ChangeAudioGroups(groupRemove, groupAdd);
        Debug.Log("Call RPC");
    }
}