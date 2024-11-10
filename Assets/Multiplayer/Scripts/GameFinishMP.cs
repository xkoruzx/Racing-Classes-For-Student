using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameFinishMP : MonoBehaviour
{
    PhotonView photonView;
    public static GameObject finishScreen;
    public static Text startTimer;
    public static Text returnTimer;
    public static Text winnerText;

    [PunRPC]
    void FinishGame(string playerName)
    {
        StartCoroutine(FinishCountdown(playerName));
    }

    IEnumerator FinishCountdown(string playerName)
    {
        //Debug.Log("Finished Game Called");
        winnerText.text = playerName;
        startTimer.text = "FINISHED!";
        yield return new WaitForSeconds(2);

        startTimer.text = "";
        finishScreen.SetActive(true);

        returnTimer.text = "3";
        yield return new WaitForSeconds(1);
        returnTimer.text = "2";
        yield return new WaitForSeconds(1);
        returnTimer.text = "1";
        yield return new WaitForSeconds(1);
        returnTimer.text = "0";

        GameObject.Find("FollowCamera").GetComponent<CameraFollowMP>().enabled = false;
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("MainMenu");
    }
}
