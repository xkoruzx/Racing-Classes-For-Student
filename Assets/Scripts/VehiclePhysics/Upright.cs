using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Upright : MonoBehaviour
{
    private Rigidbody carRigidbody;
    [SerializeField] float stopRotation;
    [SerializeField] float slerpRotation;

    float verticalAttractorTime = 1f;

    // Start is called before the first frame update
    void Start()
    {
        carRigidbody = GetComponent<Rigidbody>();
        stopRotation = 40f;
        slerpRotation = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        // Get our local Y+ vector (relative to world)
        Vector3 localUp = transform.up;

        // Now build a rotation taking us from our current Y+ vector towards the actual world Y+
        Quaternion vertical = Quaternion.FromToRotation(localUp, Vector3.up) * carRigidbody.rotation;

        // How far are we tilted off the ideal Y+ world rotation?
        float angleVerticalDiff = Quaternion.Angle(carRigidbody.rotation, vertical);
        if (angleVerticalDiff > stopRotation) // Greater than stopRotation degrees, stop angular velocity
        {
            // Slerp blend based on our current rotation
            carRigidbody.angularVelocity = Vector3.zero;
        }
        if (angleVerticalDiff > slerpRotation) // Greater than slerpRotation degrees, start the vertical attractor
        {
            // Slerp blend based on our current rotation
            carRigidbody.MoveRotation(Quaternion.Slerp(carRigidbody.rotation, vertical, Time.deltaTime * verticalAttractorTime));
        }

    }
}
