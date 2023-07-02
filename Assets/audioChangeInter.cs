using Photon.Pun;
using Photon.Voice.PUN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioChangeInter : MonoBehaviour
{
    [PunRPC]
    private void ChangeGroup(byte[] groupsToAdd, byte[] groupsToRemove)
    {
        PunVoiceClient.Instance.Client.OpChangeGroups(groupsToRemove, groupsToAdd);
        Debug.Log("Call");
    }
    private void Update()
    {
        
    }
}
