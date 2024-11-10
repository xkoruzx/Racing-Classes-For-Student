using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable; // The only hashtables we use here are Photon's.
using UnityEngine.SceneManagement;
using System;

public class GameLogicMP : MonoBehaviourPunCallbacks //, IPunObservable
{
    // Public variables
    //Max players in the server
    public static int MAX_PLAYERS = 4;

    //arrays for the checkpoints
    public Transform[] checkpointArray;
    public static Transform[] checkpointA;

    //current lap and checkpoint variables
    public static short currentCheckpoint = 0, currentLap = 0;

    //current checkpoint array and current lap array
    public static short[] ccArray, clArray;

    //Finish Screen GameObject and textGroup object
    public GameObject finishScreen, textGroup;

    //Lap used for text display and max lap #
    public int Lap;
    public int maxLaps = 3;

    //starting position and rotation
    public Vector3 startPos;
    public Quaternion startRotation;

    //total platers and the current player position as an int
    public static int playerPosition = 1;
    public int playerCount = 1;

    //Player number and tag number
    public static byte playerNum;
    public static byte tagNum;

    //UI text variables for Timers and position
    public Text lapText, lapTimerText, totalTimerText, startTimerText, positionTextUpper, positionTextLower;
    //UI text for end game
    public Text winnerNameText, returnTimerText;

    //Lap timer arrays for Minutes : Seconds . Milliseconds
    public float[] lapTime = { 0, 0, 0 };
    public float[] totalTime = { 0, 0, 0 };

    //Counting numbers for the lap and total timers
    public static float lapTimeCount, totalTimeCount;

    //Model label numbers as well as character in use number
    public static byte charModel = 0, kartModel = 0, charInUse = 0, hair = 0, skin = 0;

    //winnerName string
    public string winnerName;

    //finish boolean set to false
    public static bool finish = false;

    //array that holds the player tags
    public static string[] tagsArray;

    // Private variables
    private Transform player;
    private bool startTiming;
    private new PhotonView photonView;
    private readonly List<float> lapTimeList = new List<float>();

    private void Awake()
    {
        finish = false;
        tagNum = 1;
        playerPosition = 1;
        ccArray = new short[MAX_PLAYERS];
        clArray = new short[MAX_PLAYERS];
        tagsArray = new string[MAX_PLAYERS];
        for (int i = 0; i < MAX_PLAYERS; i++)
        {
            ccArray[i] = 0;
            clArray[i] = 0;
        }
    }

    void Start()
    {
        checkpointA = checkpointArray;

        playerCount = PhotonNetwork.PlayerList.Length;
        string resourceString = "Prefabs/Player Prefabs/" + charModel + "/" + charModel + "_" + kartModel;
        player = (PhotonNetwork.Instantiate(resourceString, new Vector3(0, 0, 0), startRotation)).transform;
        player = (PhotonNetwork.Instantiate(resourceString, GetPlayerStart(PhotonNetwork.LocalPlayer.ActorNumber), startRotation)).transform;
        player.name = "Player(Clone)";

        AnimController.kartAnim = player.transform.Find("Kart").gameObject.GetComponent<Animator>();
        AnimController.characterAnim = player.transform.Find("Char").gameObject.GetComponent<Animator>();
        photonView = player.GetComponent<PhotonView>();

        photonView.RPC("AssignTag", RpcTarget.MasterClient);
        photonView.RPC("PlayerSwap", RpcTarget.All, charModel, skin, hair);
        
        Debug.Log("PlayerNum during start: " + playerNum);
        player.rotation = startRotation;

        photonView.RPC("ColliderSwitch", RpcTarget.All, true);
        player.GetComponent<Rigidbody>().useGravity = true;

        positionTextUpper.text = "" + playerPosition;
        positionTextLower.text = "" + playerCount;

        GameFinishMP.finishScreen = finishScreen;
        GameFinishMP.startTimer = startTimerText;
        GameFinishMP.returnTimer = returnTimerText;
        GameFinishMP.winnerText = winnerNameText;

        StartCoroutine(SetOff());
    }

    private Vector3 GetPlayerStart(int playerID)
    {
        Vector3 startPosLocal = startPos;
        if ((playerID % 2) == 1)
        {
            startPosLocal -= player.transform.forward * 2 * (playerID - 1);
        }
        else
        {
            startPosLocal += player.transform.right * 2;
            startPosLocal -= player.transform.forward * 2 * (playerID - 1);
        }

        return startPosLocal;
    }

    IEnumerator SetOff()
    {
        ControllerMP kartScript = player.GetComponent<ControllerMP>();
        CameraFollowMP.target = player.transform;
        kartScript.freeze();
        yield return new WaitForSeconds(1.0f);
        startTimerText.text = "2";
        yield return new WaitForSeconds(1.0f);
        startTimerText.text = "1";
        yield return new WaitForSeconds(1.0f);
        startTiming = true;
        kartScript.unfreeze();
        startTimerText.text = "GO!";
        startTimerText.color = Color.green;
        yield return new WaitForSeconds(1.0f);
        startTimerText.text = "";
        
    }

    void Update()
    {
        if (photonView.IsMine)
        {
            if (!finish && playerNum != 0)
            {
                Debug.Log("PlayerNum during update: " + playerNum);
                int currentCheckpoint = ccArray[playerNum - 1];
                int currentLap = clArray[playerNum - 1];

                TimerCount();
                LapLogic(currentLap);
                RankingLogic(currentCheckpoint, currentLap);

                positionTextUpper.text = playerPosition.ToString();
                checkpointA = checkpointArray;
            }
        }
    }

    private static void RankingLogic(int currentCheckpoint, int currentLap)
    {
        int realListPosition = PhotonNetwork.PlayerList.Length;

        Debug.Log("Player checkpoint: " + currentCheckpoint);
        string debugString = "Tags Array: ";
        for (int i = 0; i < tagsArray.Length; i++)
        {
            if (tagsArray[i] == null)
                debugString = debugString + i + ": NULL ";
            else
                debugString = debugString + i + ": " + tagsArray[i] + " ";
        }
        Debug.Log(debugString);

        // In this section, we compare the player's position in the race to the positions of other racers.
        for (int i = 0; i < PhotonNetwork.PlayerList.Length; i++)
        {
            if (PhotonNetwork.PlayerList[i] != PhotonNetwork.LocalPlayer)
            {
                int npCurrentLap = clArray[i];
                int npCurrentCheckpoint = ccArray[i];

                Debug.Log("Player's current checkpoint is " + currentCheckpoint + ". Opponent's current checkpoint is " + npCurrentCheckpoint);
                Debug.Log("Player's current lap is " + currentLap + ". Opponent's current lap is " + npCurrentLap);

                if (currentLap > npCurrentLap)
                    realListPosition--;
                else if (currentLap == npCurrentLap)
                {
                    if (currentCheckpoint > npCurrentCheckpoint)
                        realListPosition--;
                    else if (currentCheckpoint == 0 && currentCheckpoint != npCurrentCheckpoint && currentLap > 0)
                        realListPosition--;
                    else if (currentCheckpoint == npCurrentCheckpoint)
                        realListPosition--;
                }
            }
        }

        Debug.Log("Player 1 Checkpoint: " + ccArray[0] + " Player 2 Checkpoint: " + ccArray[1]);

        playerPosition = realListPosition;
    }

    private void LapLogic(int currentLap)
    {

        //Reset lap time
        if (currentLap != Lap)
        {
            // Offset laps by one so that when the kart initially touches the first checkpoint, the lap doesn't increment the first time.
            if (Lap != 0)
            {
                lapTimeList.Add(lapTimeCount); //Add the lap time to the list for displaying at the end.
                lapTimeCount = 0.0f;
            }
        }
        Lap = currentLap;

        if (Lap > maxLaps)
        {
            float bestLapTime = lapTimeList[0];

            for (int i = 1; i < lapTimeList.Count; i++)
            {
                if (lapTimeList[i] < bestLapTime)
                {
                    bestLapTime = lapTimeList[i];
                }
            }
            float totalLapTime = totalTimeCount;
            string playerName = PhotonNetwork.LocalPlayer.NickName;
            finish = true;
            totalTimeCount = 0.0f;
            photonView.RPC("FinishGame", RpcTarget.AllViaServer, playerName); // Tell all clients that the game is finished.
        }

        if (Lap == 0)
        {
            lapText.text = "Lap: " + (Lap + 1) + " of " + maxLaps;
        }
        else if (Lap <= maxLaps)
        {
            lapText.text = "Lap: " + (Lap) + " of " + maxLaps;
        }
    }

    private void TimerCount()
    {
        if (startTiming)
        {
            totalTimeCount += Time.deltaTime;
            lapTimeCount += Time.deltaTime;

            lapTime[0] = Mathf.Floor(lapTimeCount / 60f);
            lapTime[1] = Mathf.Floor(lapTimeCount) % 60;
            lapTime[2] = Mathf.Floor(lapTimeCount * 1000f) % 1000;

            totalTime[0] = Mathf.Floor(totalTimeCount / 60f);
            totalTime[1] = Mathf.Floor(totalTimeCount) % 60;
            totalTime[2] = Mathf.Floor(totalTimeCount * 1000f) % 1000;

        }

        totalTimerText.text = string.Format("{0:00}:{1:00}:{2:00}", totalTime[0], totalTime[1], totalTime[2]);
        lapTimerText.text = string.Format("{0:00}:{1:00}:{2:00}", lapTime[0], lapTime[1], lapTime[2]);
    }
}