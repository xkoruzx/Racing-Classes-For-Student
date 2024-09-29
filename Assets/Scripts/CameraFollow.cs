using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    CameraCalculations cameraCalc;
    public static Transform target;
    public int playerLayerNumber = 9;
    public float maxDistance = 4.0f; //maximum distance the camera can be from the target
    public float minDistance = 2.0f; //minimum distance the camera cana be from the target

    public float minHeight = 1.0f; // mimimum height the camera can be from the ground
    public float distanceOffset; // Offset for the camera's distance from the target
    public float finalDistance; // the final calculated distance from the target
    
    // A Vector2 to control the speed at which the camera moves, with seperate values for X and y axes
    public Vector2 cameraSpeed = new Vector2(3.0f, 1.0f); 
    //A Vector2 to define the limits on the Y-Axis for the camera's movement
    public Vector2 cameraYLimits = new Vector2(5.0f, 60.0f);
    //A Boolean to determine if the camera should be reset to the default position
    public bool resetCamera = true;

    // Variables to store the camera's X and y rotation values
    public float x = 0.0f;
    public float y = 0.0f;

    //Offset for the Y position of the camera
    public float yOffset;
    // the new position for the camera after calculations
    public float newPos;
    // Variables to store the camera's rotation and position
    private Quaternion rotation;
    private Vector3 position;

    // Start is called before the first frame update
    void Start()
    {
        //finds the cameracalculations component in the scene and assigns it to cameracalc
        cameraCalc = FindObjectOfType<CameraCalculations>();

        // Gets the current rotation of the camera and assigns the X and Y angles to the respective variabels
        Vector3 angles = transform.eulerAngles;
        x = angles.x;
        y = angles.y;

        //Hides the mouse cursor
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        distanceOffset = cameraCalc.CalculateOffset(target, playerLayerNumber, maxDistance, distanceOffset);
    }

    void LateUpdate()
    {
        // Updates the X and Y rotation values based on mouse input and camera speed
        x += Input.GetAxis("Mouse X") * cameraSpeed.x;
        y -= Input.GetAxis("Mouse Y") * cameraSpeed.y;
        
        // clamps the Y roation value within the specified limits
        y = cameraCalc.ClampAngle(y, cameraYLimits.x, cameraYLimits.y);

        yOffset = cameraCalc.FindOffsetY(minHeight);
        newPos = cameraCalc.FindPosition(minHeight);

        if (resetCamera == true)
        {
            x = target.transform.eulerAngles.y;
            y = 0;
            
            // Creates a new roation Quaternion based on the updated X and Y values
            rotation = Quaternion.Euler(y, x, 0.0f);
        }
        else
        {
            // Calculates the final distance from the targetm considering the distance offset
            finalDistance = Mathf.Min(-minDistance, -maxDistance + distanceOffset);
            
            //Creates a new rotation Quaternion based on the updated X and Y values
            rotation = Quaternion.Euler(y,x,0.0f);
            

            //Calculates the camera's position based on the targets' position and the final distance
            Vector3 changedPos = target.position - new Vector3(0.0f, newPos, 0.0f);

            position = rotation * new Vector3(0.0f, 0.0f, finalDistance)+ changedPos;
            // Adds the Y offset
            position.y = position.y + yOffset;
        }

        // Updates the camera's rotation and position basaed on the calculated values
        transform.rotation = rotation;
        transform.position = position;

        //disables the resetCamera flag to prevent resetting the camera on the next frame
        resetCamera = false;
    }
}
