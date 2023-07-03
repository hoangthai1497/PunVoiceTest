using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.Collections.Generic;
public class RoomTrigger : MonoBehaviour
{
    private List<byte> _listInterestGroupAdd = new List<byte>();
    private List<byte> _listInterestGroupRemove = new List<byte>();
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PrivateVoicePun trigger = other.GetComponent<PrivateVoicePun>();
            if (trigger != null)
            {
                _listInterestGroupAdd.Add(trigger.TargetInterestGroup);
                Debug.Log("player targetGroup " + trigger.TargetInterestGroup);
            }
            trigger.photonView.RPC("ChangGroupSub", RpcTarget.OthersBuffered, _listInterestGroupRemove.ToArray(), _listInterestGroupAdd.ToArray());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PrivateVoicePun trigger = other.GetComponent<PrivateVoicePun>();
            if (_listInterestGroupAdd.Contains(trigger.TargetInterestGroup))
            {
                _listInterestGroupAdd.Remove(trigger.TargetInterestGroup);
            }
            if (!_listInterestGroupRemove.Contains(trigger.TargetInterestGroup))
            {
                Debug.Log("Add to remove " + trigger.TargetInterestGroup);
                _listInterestGroupRemove.Add(trigger.TargetInterestGroup);
            }
            trigger.photonView.RPC("ChangGroupSub", RpcTarget.OthersBuffered, _listInterestGroupRemove.ToArray(), _listInterestGroupAdd.ToArray());
        }
    }

}