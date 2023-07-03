using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.Collections.Generic;
using Photon.Voice.PUN;

public class RoomTrigger : MonoBehaviour
{
    public List<byte> _listInterestGroupAdd = new List<byte>();
    public List<byte> _listInterestGroupRemove = new List<byte>();
    public PhotonView photonView;
    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }
    [PunRPC]
    public void AddToList(byte value)
    {
        _listInterestGroupAdd.Add(value);
    }
    [PunRPC]
    public void RemoveToList(byte value)
    {
        if (_listInterestGroupAdd.Contains(value))
        {
            _listInterestGroupAdd.Remove(value);
            Debug.Log("remove");
        }
        if (!_listInterestGroupRemove.Contains(value))
        {
            _listInterestGroupRemove.Add(value);
        }
    }
}