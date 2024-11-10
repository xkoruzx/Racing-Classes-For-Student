using System;
using System.Collections;


using UnityEngine;
using UnityEngine.SceneManagement;


using Photon.Pun;
using Photon.Realtime;


namespace Com.CodaKid.CodaKart
{
    public class GameManagerMP : MonoBehaviourPunCallbacks
    {
        public string gameSceneName = "RacingGame";

        #region Photon Callbacks


        public override void OnLeftRoom()
        {
            SceneManager.LoadScene(0);
        }

        public override void OnPlayerLeftRoom(Player newPlayer)
        {
            Debug.LogFormat("OnPlayerLeftRoom() {0}", newPlayer.NickName);

            if (PhotonNetwork.IsMasterClient)
            {
                Debug.LogFormat("OnPlayerLeftRoom IsMasterClient {0}", PhotonNetwork.IsMasterClient);

                LoadGame();
            }
        }

        #endregion


        #region Public Methods


        public void LeaveRoom()
        {
            PhotonNetwork.LeaveRoom();
        }


        #endregion

        #region Private Methods

        public void LoadGame()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                Debug.LogError("PhotonNetwork : Trying to load a level but we are not the master client.");
            }
            Debug.LogFormat("PhotonNetwork : Loading Level");
            PhotonNetwork.CurrentRoom.IsVisible = false;
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.LoadLevel(gameSceneName);
        }

        #endregion
    }
}