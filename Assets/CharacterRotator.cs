using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterRotator : MonoBehaviour
{
    public float secondsPerRotation = 6.0f; //how many seconds it takes to rotate 360 degrees
    public static Transform objectToRotate; //the object to rotate

    void Update() //called once per frame
    {
        objectToRotate.Rotate(0, 6.0f * (2.5f * secondsPerRotation) * Time.deltaTime, 0); //rotate the object by 6 degrees per second
    }
}
