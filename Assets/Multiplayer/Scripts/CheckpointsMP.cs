using UnityEngine;
using System.Collections;
using Photon.Realtime;
using Photon.Pun;

public class CheckpointsMP : MonoBehaviour
{
    PhotonView photonView;
    public static bool readyToRequest = true;

    private void Awake()
    {
        readyToRequest = true;
    }

    void OnTriggerEnter(Collider other)
    {
        byte playerNum = 0;
        for (int i = 1; i <= GameLogicMP.MAX_PLAYERS; i++)
        {
            if (other.CompareTag("player" + i) == true)
            {
                playerNum = (byte)i;
                break;
            }
        }

        if (playerNum == 0) // Player was not found in collision.
            return;

        short currentCheckpoint = GameLogicMP.ccArray[playerNum - 1];
        short currentLap = GameLogicMP.clArray[playerNum - 1];

        if (readyToRequest && transform == GameLogicMP.checkpointA[currentCheckpoint].transform)
        {
            photonView = GameObject.FindGameObjectWithTag("player" + playerNum).GetComponent<PhotonView>();
            readyToRequest = false;
            photonView.RPC("PositionHandler", RpcTarget.MasterClient, playerNum);
        }
    }
}