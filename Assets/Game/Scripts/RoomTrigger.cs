using Photon.Pun;
using Photon.Realtime;
using UnityEngine;
using System.Collections.Generic;
using Photon.Voice.PUN;

public enum RoomName
{ 
  
}
public class RoomTrigger : MonoBehaviour
{
    
    public List<byte> _listInterestGroupAdd = new List<byte>();
    public List<byte> _listInterestGroupRemove = new List<byte>();
    private byte[] temp = new byte[4];
    public PhotonView photonView;
    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }
    [PunRPC]
    public void AddToList(byte value)
    {
        _listInterestGroupAdd.Add(value);
        temp = _listInterestGroupAdd.ToArray();
    }
    [PunRPC]
    public void RemoveListFromAdd(byte value)
    {
        if (_listInterestGroupAdd.Contains(value))
        {
            _listInterestGroupAdd.Remove(value);            
        }        
    }
    [PunRPC]
    public void AddToRemoveList(byte value)
    {
        if (!_listInterestGroupRemove.Contains(value))
        {
            _listInterestGroupRemove.Add(value);
        }
    }
}