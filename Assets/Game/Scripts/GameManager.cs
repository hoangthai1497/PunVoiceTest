using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{
    public GameObject PlayerPrefab;
    private void Start()
    {
        PhotonNetwork.Instantiate(this.PlayerPrefab.name, new Vector3(0, 5, 0), Quaternion.identity, 0);
    }
    public override void OnLeftRoom()
    {
        base.OnLeftRoom();
        SceneManager.LoadScene(0);
    }

   public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }

    void LoadArea()
    {
        if(!PhotonNetwork.IsMasterClient)
        {
            return;
        }
        PhotonNetwork.LoadLevel("Room for " + PhotonNetwork.CurrentRoom.PlayerCount);
    }
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        base.OnPlayerEnteredRoom(newPlayer);
        if(PhotonNetwork.IsMasterClient)
        {
            LoadArea();
        }
    }
    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        base.OnPlayerLeftRoom(otherPlayer);
        if(PhotonNetwork.IsMasterClient )
        {
            LoadArea();
        }
    }
}
