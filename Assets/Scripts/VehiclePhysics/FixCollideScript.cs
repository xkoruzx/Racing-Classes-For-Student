using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FixCollideScript : MonoBehaviour
{
    public bool isGrounded;

    public float jumpForce = 1500.0f;

    public Vector3 jumpValue = new Vector3(0.0f, 2.0f, 0.0f);

    Rigidbody rigidBody;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody>();
    }

    void OnCollisionStay()
    {
        isGrounded = true;
    }

    void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            rigidBody.AddForce(jumpValue * jumpForce, ForceMode.Impulse);

            isGrounded = false;
        }
    }
}
