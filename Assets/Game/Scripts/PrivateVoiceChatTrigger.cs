using Photon.Pun;
using Photon.Realtime;
using UnityEngine;

public class PrivateVoiceChatTrigger : MonoBehaviour
{
    public byte privateInterestGroup = 1;

    private void OnTriggerEnter(Collider other)
    {
        PhotonView otherPhotonView = other.GetComponent<PhotonView>();
        if (otherPhotonView != null && otherPhotonView.IsMine)
        {
            byte[] groupsToAdd = new byte[] { privateInterestGroup };
            byte[] groupsToRemove = null;
            //PhotonNetwork.NetworkingClient.OpChangeGroups(groupsToRemove, groupsToAdd);
            otherPhotonView.RPC("ChangeGroup",RpcTarget.OthersBuffered, groupsToRemove, groupsToAdd);
           //otherPhotonView.Owner.OpChangeGroups(groupsToAdd, groupsToRemove);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        PhotonView otherPhotonView = other.GetComponent<PhotonView>();
        if (otherPhotonView != null && otherPhotonView.IsMine)
        {
            byte[] groupsToAdd = null;
            byte[] groupsToRemove = new byte[] { privateInterestGroup };
            //PhotonNetwork.NetworkingClient.OpChangeGroups(groupsToRemove, groupsToAdd);
            otherPhotonView.RPC("ChangeGroup", RpcTarget.OthersBuffered, groupsToRemove, groupsToAdd);

        }
    }
}