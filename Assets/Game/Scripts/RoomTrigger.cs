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

    public List<byte> _listPlayer = new List<byte>();
    public List<Player> PlayerIngroup = new List<Player>();


    public PhotonView photonView;
    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }
    [PunRPC]
    public void AddToList(byte value)
    {
        _listInterestGroupAdd.Add(value);
        if (!_listPlayer.Contains(value))
        {
            _listPlayer.Add(value);
        }

    }
    [PunRPC]
    public void RemoveListFromAdd(byte value)
    {
        if (_listInterestGroupAdd.Contains(value))
        {
            _listInterestGroupAdd.Remove(value);
        }
        if (!_listInterestGroupRemove.Contains(value))
        {
            _listInterestGroupRemove.Add(value);
        }
    }
    [PunRPC]
    public void AddGroupPlayer(Player player)
    {
        if (!PlayerIngroup.Contains(player))
        {
            PlayerIngroup.Add(player);
        }
    }
    [PunRPC]
    public void RemoveGroupPlayer(Player player)
    {
        if (PlayerIngroup.Contains(player))
        {
            PlayerIngroup.Remove(player);
        }
    }
}