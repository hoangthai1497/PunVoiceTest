using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;

//[RequireComponent(typeof(InputField))]
public class PlayNameInputField : MonoBehaviour
{
    const string _playNamePrekey = "PlayerName";
    void Start()
    {
        string defaultName = string.Empty;
        InputField _inputField = this.GetComponent<InputField>();
        if (_inputField != null)
        {
            if(PlayerPrefs.HasKey(_playNamePrekey))
            {
                defaultName = PlayerPrefs.GetString(_playNamePrekey);
                _inputField.text = defaultName;
            }
        }
        PhotonNetwork.NickName = defaultName;

    }
    public void SetPlayerName(string name)
    {
        if(string.IsNullOrEmpty(name))
        {
            return;
        }
        PhotonNetwork.NickName=name;
        PlayerPrefs.SetString(_playNamePrekey, name);
    }

}
