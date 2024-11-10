using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AxleMP
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor;
    public bool steering;
}

public class ControllerMP : MonoBehaviourPun
{
    public List<AxleMP> axles;
    public float maxMotorTorque;
    public float maxSteeringAngle;
    private bool frozen = false;

    public void FixedUpdate()
    {
        if (photonView.IsMine) // Make sure the local player is controlling this car only.
        {
            float motor = 0;
            float steering = 0;
            if (!frozen)
            {
                motor = maxMotorTorque * Input.GetAxis("Vertical");
                steering = maxSteeringAngle * Input.GetAxis("Horizontal");
            }

            foreach (AxleMP axle in axles)
            {
                if (axle.steering)
                {
                    axle.leftWheel.steerAngle = steering;
                    axle.rightWheel.steerAngle = steering;
                }
                if (axle.motor)
                {
                    axle.leftWheel.motorTorque = motor;
                    axle.rightWheel.motorTorque = motor;
                }
            }
        }
    }

    public void unfreeze()
    {
        frozen = false;
    }

    public void freeze()
    {
        frozen = true;
    }
}
