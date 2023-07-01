using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class Launcher : MonoBehaviourPunCallbacks
{
    [SerializeField]
    private GameObject _controlPanel;
    [SerializeField]
    private GameObject _progessLabel;

    private string _gameVer = "1";
    [SerializeField]
    private byte _maxPlayerPerRoom = 4;
    bool _isConnecting;

   
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    private void Start()
    {
        _progessLabel.SetActive(false);
        _controlPanel.SetActive(true);
    }
    public void Connect()
    {
        _progessLabel.SetActive(true);
        _controlPanel.SetActive(false);
        
        if (PhotonNetwork.IsConnected)
        {
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            _isConnecting = PhotonNetwork.ConnectUsingSettings();
            PhotonNetwork.GameVersion = _gameVer;

        }
    }
    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        if (_isConnecting)
        {
            PhotonNetwork.JoinRandomRoom();
            _isConnecting = false;
        }

        
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
        _progessLabel.SetActive(false);
        _controlPanel.SetActive(true);
        _isConnecting = false;
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        base.OnJoinRandomFailed(returnCode, message);
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = _maxPlayerPerRoom});
    }
    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            PhotonNetwork.LoadLevel("Room for 1");
        }
    }
}
