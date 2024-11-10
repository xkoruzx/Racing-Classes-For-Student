using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateCallsMP : MonoBehaviour
{
    #region Character Swap Resources

    public Texture boySkin1, boySkin2, boySkin3, girlSkin1, girlSkin2, girlSkin3, boyHair1, boyHair2, boyHair3, girlHair1, girlHair2, girlHair3;
    public static byte skin = 0, hair = 0, charInUse = 0;

    #endregion

    [PunRPC]
    void ColliderSwitch(bool enable)
    {
        Collider[] colliders = transform.gameObject.GetComponentsInChildren<Collider>();

        foreach (Collider collider in colliders)
        {
            collider.enabled = enable; 
        }
    }

    [PunRPC]
    void PositionHandler(byte playerNum)
    {
        short currentCheckpoint = GameLogicMP.ccArray[playerNum - 1];
        short currentLap = GameLogicMP.clArray[playerNum - 1];

        Debug.Log("UpdateArrays called");
        string playerTag = "player" + playerNum;
        PhotonView photonView = transform.GetComponent<PhotonView>();

        Debug.Log("Checkpoint triggered by " + playerTag);

        Debug.Log("Current Checkpoint value for player " + playerNum + " is " + currentCheckpoint);
        Debug.Log("Checkpoint Array Length: " + GameLogicMP.checkpointA.Length);

        //Check so we dont exceed our checkpoint quantity
        if (currentCheckpoint + 1 < GameLogicMP.checkpointA.Length)
        {
            //Add to currentLap if currentCheckpoint is 0
            if (currentCheckpoint == 0)
            {
                currentLap++;
            }

            currentCheckpoint++;
        }
        else
        {
            //If we dont have any Checkpoints left, go back to 0
            currentCheckpoint = 0;
        }

        GameLogicMP.ccArray[playerNum - 1] = currentCheckpoint;
        GameLogicMP.clArray[playerNum - 1] = currentLap;

        photonView.RPC("DistributeArrays", RpcTarget.All, playerNum, currentCheckpoint, currentLap);

        Debug.Log("Player " + playerNum + " Checkpoint: " + GameLogicMP.ccArray[playerNum - 1] + " Lap: " + GameLogicMP.clArray[playerNum - 1]);
    }

    [PunRPC]
    void DistributeArrays(byte playerNum, short currentCheckpoint, short currentLap)
    {
        GameLogicMP.ccArray[playerNum - 1] = currentCheckpoint;
        GameLogicMP.clArray[playerNum - 1] = currentLap;
        if (playerNum == GameLogicMP.playerNum)
            CheckpointsMP.readyToRequest = true;
    }

    [PunRPC]
    void UpdateTags(string playerTag, byte playerNum)
    {
        Debug.Log("UpdateTags: " + playerTag + " being written to tagsArray[" + (playerNum - 1) + "] from Photon viewID " + transform.gameObject.GetComponent<PhotonView>().ViewID);
        GameLogicMP.tagsArray[playerNum - 1] = playerTag;
        PhotonView localPhotonView = transform.gameObject.GetComponent<PhotonView>();
        if (localPhotonView.IsMine)
            GameLogicMP.playerNum = playerNum;
        transform.gameObject.tag = playerTag;
    }

    [PunRPC]
    void AssignTag()
    {
        // Tag player depending on available tags. There should be at least ONE tag available, otherwise there was an error with the max amount of players.
        if (GameLogicMP.tagNum <= GameLogicMP.MAX_PLAYERS)
        {
            transform.tag = "player" + GameLogicMP.tagNum;
            PhotonView photonView = transform.GetComponent<PhotonView>();
            photonView.RPC("UpdateTags", RpcTarget.All, ("player" + GameLogicMP.tagNum), GameLogicMP.tagNum);
            GameLogicMP.tagNum++;
        }
    }

    #region Character Swap Code

    [PunRPC]
    void PlayerSwap(byte charInUse, byte skin, byte hair)
    {
        GameObject currentAvatar = (transform.Find("Char").transform.Find("Avatar")).gameObject;

        if (charInUse > 0) // Only attempt to change hair if character is human (not cat)
        {
            Material[] currentMaterialArray = currentAvatar.GetComponent<Renderer>().materials;
            foreach (Material matItem in currentMaterialArray)
            {
                Debug.Log(matItem.name);
            }
            Material currentMaterial;
            Material currentMaterial2;
            bool isGirl = false;
            if (charInUse == 2)
                isGirl = true;
            if (isGirl)
            {
                currentMaterial = currentAvatar.GetComponent<Renderer>().materials[2];
                currentMaterial2 = currentAvatar.GetComponent<Renderer>().materials[2];
            }
            else
            {
                currentMaterial = currentAvatar.GetComponent<Renderer>().materials[0];
                currentMaterial2 = currentAvatar.GetComponent<Renderer>().materials[1];
            }

            switch (hair)
            {
                // Skin 1
                case (0):
                    if (isGirl)
                        currentMaterial.SetTexture("_MainTex", girlHair1);
                    else
                    {
                        currentMaterial.SetTexture("_MainTex", boyHair1);
                        currentMaterial2.SetTexture("_MainTex", boyHair1);
                    }
                    break;
                // Skin 2
                case (1):
                    if (isGirl)
                        currentMaterial.SetTexture("_MainTex", girlHair2);
                    else
                    {
                        currentMaterial.SetTexture("_MainTex", boyHair2);
                        currentMaterial2.SetTexture("_MainTex", boyHair2);
                    }
                    break;
                // Skin 3
                case (2):
                    if (isGirl)
                        currentMaterial.SetTexture("_MainTex", girlHair3);
                    else
                    {
                        currentMaterial.SetTexture("_MainTex", boyHair3);
                        currentMaterial2.SetTexture("_MainTex", boyHair3);
                    }
                    break;
            }
        }

        if (charInUse > 0) // Only address skin if the character is a human (not cat)
        {
            bool isGirl = false;
            if (charInUse == 2)
                isGirl = true;
            Material currentMaterial;
            if (isGirl)
            {
                currentMaterial = currentAvatar.GetComponent<Renderer>().materials[0];
            }
            else
            {
                currentMaterial = currentAvatar.GetComponent<Renderer>().materials[2];
            }

            switch (skin)
            {
                // Skin 1
                case (0):
                    if (isGirl)
                        currentMaterial.SetTexture("_MainTex", girlSkin1);
                    else
                        currentMaterial.SetTexture("_MainTex", boySkin1);
                    break;
                // Skin 2
                case (1):
                    if (isGirl)
                        currentMaterial.SetTexture("_MainTex", girlSkin2);
                    else
                        currentMaterial.SetTexture("_MainTex", boySkin2);
                    break;
                // Skin 3
                case (2):
                    if (isGirl)
                        currentMaterial.SetTexture("_MainTex", girlSkin3);
                    else
                        currentMaterial.SetTexture("_MainTex", boySkin3);
                    break;
            }
        }
    }

    #endregion
}
