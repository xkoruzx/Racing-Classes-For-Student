using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameLogic : MonoBehaviour
{

    //arrays for the checkpoints
    public Transform[] checkpointArray;
    public static Transform[] checkpointA;

    public GameObject finishPanel;

    //current lap and checkpoint variables
    public static int currentCheckpoint, currentLap;

    //Lap used for text display and max lap #
    public int Lap;
    public int MaxLaps = 3;


    //starting position and the player
    public Vector3 startPos;
    public Quaternion startRotation;
    public static Transform player;

    //total players and the current player position as an int
    public int playerPosition = 1;
    public int playerCount = 1;

    //All of the UI text variables
    public Text lapText, lapTimerText, totalTimerText, startTimerText, positionTextUpper, positionTextLower;

    public Text winnerNameText, returnTimerText;


    //Lap timer arras for Minutes : Seconds . Milliseconds
    public static float lapTimeCount, totalTimeCount;

    //Counting Numbers for the lap and total timers
    public float[] lapTime = { 0, 0, 0 };
    public float[] totalTime = { 0, 0, 0 };


    //boolean that determines when to start counting
    private bool startTiming;

    private bool finish = false;
    void Start()
    {
        //set the current lap and checkpoint to 0 at start
        currentCheckpoint = 0;
        currentLap = 0;


        //teleport player
        player.position = startPos;

        //set the player position and total players to the current player count
        positionTextUpper.text = playerPosition.ToString();
        positionTextLower.text = playerCount.ToString();

        //start the setOff countdown coroutine 
        StartCoroutine(SetOff());
    }

    //steps for counting down from 3 and the unfreezing the player
    IEnumerator SetOff()
    {
        Controller kartScript = player.GetComponent<Controller>();
        kartScript.Freeze();
        yield return new WaitForSeconds(1.0f);
        startTimerText.text = "2";
        yield return new WaitForSeconds(1.0f);
        startTimerText.text = "1";
        yield return new WaitForSeconds(1.0f);
        startTiming = true;
        kartScript.Unfreeze();
        startTimerText.text = "Go!!";
        startTimerText.color = Color.yellow;
        yield return new WaitForSeconds(1.0f);
        startTimerText.text = "";
    }

    IEnumerator FinishGame()
    {
        finish = true;
        totalTimeCount = 0.0f;

        startTimerText.text = "Finished";
        yield return new WaitForSeconds(2.0f);

        startTimerText.text = "";
        finishPanel.SetActive(true);

        returnTimerText.text = "3";
        yield return new WaitForSeconds(1.0f);

        returnTimerText.text = "2";
        yield return new WaitForSeconds(1.0f);

        returnTimerText.text = "1";
        yield return new WaitForSeconds(1.0f);
        returnTimerText.text = "0";

        Destroy(player.gameObject);
        SceneManager.LoadScene("MainMenu");
        
    }    
    // Update is called once per frame
    void Update()
    {
        if (!finish)
        {
            TimerCount();
            LapLogic();
            positionTextUpper.text = playerPosition.ToString();
            checkpointA = checkpointArray;
        }
        
    }

    private void TimerCount()
    {
        //if start timing is true do all the time 
        if (startTiming)
        {
            //increment both of the timers
            lapTimeCount += Time.deltaTime;
            totalTimeCount += Time.deltaTime;

            //calculate the Minutes : Seconds . Milliseconds
            lapTime[0] = Mathf.Floor(lapTimeCount / 60.0f);
            lapTime[1] = Mathf.Floor(lapTimeCount) % 60;
            lapTime[2] = Mathf.Floor(lapTimeCount * 1000.0f) % 1000;

            //calculate the Minutes : Seconds . Milliseconds
            totalTime[0] = Mathf.Floor(totalTimeCount / 60.0f);
            totalTime[1] = Mathf.Floor(totalTimeCount) % 60;
            totalTime[2] = Mathf.Floor(totalTimeCount * 1000.0f) % 1000;

        }

        //Display the Minutes : Seconds . Milliseconds for both timers
        lapTimerText.text = string.Format("{0:00}:{1:00}.{2:000}", lapTime[0], lapTime[1], lapTime[2]);
        totalTimerText.text = string.Format("{0:00}:{1:00}.{2:000}", totalTime[0], totalTime[1], totalTime[2]);
    }

    private void LapLogic()
    {
        //if the current lap is not equal to the lap, we want to reset lapTimer
        if (currentLap != Lap)
        {
            //if the lap is not the very first 0 lap, then reset the lapcount
            if (Lap != 0)
            {
                lapTimeCount = 0.0f;
            }
        }
        //set lap = currentlap so we can run checkpoints again
        Lap = currentLap;


        //if the lap is greater than maxLaps we want to stop the code
        if (Lap > MaxLaps)
        {
            StartCoroutine(FinishGame());

        }

        //if the lap is the very first one, display it as 1 rather than 0. Else display as lap
        if (Lap == 0)
        {
            lapText.text = "Lap " + (Lap + 1) + " of " + MaxLaps;
        }
        else if (Lap <= MaxLaps)
        {
            lapText.text = "Lap " + Lap + " of " + MaxLaps;
        }
    }
}
