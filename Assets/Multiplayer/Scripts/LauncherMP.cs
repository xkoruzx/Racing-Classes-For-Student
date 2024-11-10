using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun; // Photon networking service
using Photon.Realtime;

// Custom namespace to avoid conflicts with asset scripts if they occur
namespace Com.CodaKid.KartRacing
{
    public class LauncherMP : MonoBehaviourPunCallbacks
    {
        public string gameSceneName = "RacingGame";

        #region Private Serializable Fields
        
        [Tooltip("The maximum number of players per room. When a room is full, it can't be joined by new players, and so a new room will be created.")]
        [SerializeField]
        private readonly byte maxPlayersPerRoom = 4;
        #endregion

        #region Private Fields

        // Client version number, used to make sure that clients have the same version before joining games.
        bool isConnecting;
        string gameVersion = "1";

        #endregion

        #region Public Methods

        public void Connect()
        {
            isConnecting = true;

            if (PhotonNetwork.IsConnected)
            {
                PhotonNetwork.JoinRandomRoom();
            }
            else
            {
                PhotonNetwork.GameVersion = gameVersion;
                PhotonNetwork.ConnectUsingSettings();
            }
        }

        #endregion

        #region MonoBehaviourPunCallbacks Callbacks

        public override void OnConnectedToMaster()
        {
            if (isConnecting)
            {
                PhotonNetwork.JoinRandomRoom();
            }
            Debug.Log("Kart Racing Launcher: OnConnectedToMaster() was called by PUN");
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.LogWarningFormat("Kart Racing Launcher: OnDisconnected() was called by PUN with reason {0}", cause);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("Kart Racing Launcher: OnJoinRandomFailed() was called by PUN. No random room available, so we create one.\nCalling: PhotonNetwork.CreateRoom");

            PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = maxPlayersPerRoom });
        }

        public override void OnJoinedRoom()
        {
            Debug.Log("Kart Racing Launcher: OnJoinedRoom() called by PUN. Client is in multiplayer room.");
            ButtonHandler buttonHandler = GameObject.Find("CharacterLogic").GetComponent<ButtonHandler>();
            if (PhotonNetwork.IsMasterClient)
                buttonHandler.connectText.text = "Connected as Server";
            else
                buttonHandler.connectText.text = "Connected as Client";
        }

        void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
        }

        // Start is called before the first frame update
        void Start()
        {
        }

        #endregion 
    }
}
