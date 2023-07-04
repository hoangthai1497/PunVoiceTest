using System.Collections.Generic;
using Photon.Pun;
using Photon.Realtime;
using Photon.Voice.PUN;
using UnityEngine;

public class RoomTriggerRemake : MonoBehaviour
{
    public List<byte> interestGroups = new List<byte>();
    public List<Player> playersInGroup = new List<Player>();

    public PhotonView photonView;

    private void Awake()
    {
        photonView = GetComponent<PhotonView>();
    }

    [PunRPC]
    public void UpdatePlayerGroupList()
    {
        foreach (Player player in playersInGroup)
        {
            if (!player.IsLocal)
            {
                PunVoiceClient.Instance.Client.OpChangeGroups(new byte[] { photonView.Group }, new byte[0]);
            }
        }
    }

    [PunRPC]
    public void AddToGroup(Player player)
    {
        if (!playersInGroup.Contains(player))
        {
            playersInGroup.Add(player);
            if (!interestGroups.Contains(photonView.Group))
            {
                interestGroups.Add(photonView.Group);
            }
        }
    }

    [PunRPC]
    public void RemoveFromGroup(Player player)
    {
        if (playersInGroup.Contains(player))
        {
            playersInGroup.Remove(player);
        }
        if (playersInGroup.Count == 0 && interestGroups.Contains(photonView.Group))
        {
            interestGroups.Remove(photonView.Group);
        }
    }
}
