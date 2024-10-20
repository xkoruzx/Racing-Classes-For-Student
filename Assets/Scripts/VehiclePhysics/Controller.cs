using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class Axle
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor;
    public bool steering;

}
public class Controller : MonoBehaviour
{
    // Start is called before the first frame update

    public List<Axle> axles;
    public float maxMotorTorque;
    public float maxSteeringAngle;
    public bool frozen = false;

    
    public void FixedUpdate()
    {

        float motor = 0;
        float steering = 0;

        if (!frozen)
        {
            // Get the input for motor torque from the vertical W = Forward S = backward
            motor = maxMotorTorque * Input.GetAxis("Vertical");
            // Get the input forr steering angle from the horizontal A = left D = right
            steering = maxSteeringAngle * Input.GetAxis("Horizontal");
        }
      
        
        // Loop Through each axle in the axles list
        foreach (Axle axle in axles)
        {
            // If the axle is connected to the motor, apply the motor torque to both wheels
            if (axle.motor)
            {
                axle.leftWheel.motorTorque = motor;
                axle.rightWheel.motorTorque = motor;
            }
            // if the axle is used for steering, apply the steering angle to both wheels
            if (axle.steering)
            {
                axle.leftWheel.steerAngle = steering;
                axle.rightWheel.steerAngle = steering;
            }
        }
    }

    public void Unfreeze() 
    {

        frozen = false;
        
    }

    public void Freeze() 
    {
        frozen = true;
    }
}
