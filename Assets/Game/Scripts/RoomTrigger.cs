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
    public void UpdateList()
    {
        _listInterestGroupAdd = this._listInterestGroupAdd;
        _listInterestGroupRemove = this._listInterestGroupRemove;
    }
}