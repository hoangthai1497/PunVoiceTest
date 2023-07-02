using Photon.Pun;
using Photon.Voice.PUN;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoomPrivateVoice : MonoBehaviourPunCallbacks
{
    byte roomID = 5;
    byte defaultRoom = 0;
    private void OnTriggerEnter(Collider other)
    {
        PrivateVoicePun chatVoice = other.GetComponent<PrivateVoicePun>();        
        chatVoice.groupsToAdd.Add(roomID);        
       Debug.Log("Change Group "+ roomID);
    }
    private void OnTriggerExit(Collider other)
    {
        PrivateVoicePun chatVoice = other.GetComponent<PrivateVoicePun>();
        
        if (chatVoice.groupsToAdd.Contains(roomID))
        {
            chatVoice.groupsToAdd.Remove(roomID);
        }
        if(!chatVoice.groupsToRemove.Contains(roomID))
        {
            chatVoice.groupsToRemove.Add(roomID);
        }
    }
}
