﻿using System.Collections;
using UnityEngine;

public class AnimController : MonoBehaviour
{
    private bool noKeyPress;
    public GameObject kart;
    public GameObject character;
    [SerializeField] Animator kartAnim;
    [SerializeField] Animator characterAnim;

    void Start()
    {
        kartAnim = kart.GetComponent<Animator>();
        characterAnim = character.GetComponent<Animator>();
    }

    void Update()
    {
        RunAnimations();
    }

    private void RunAnimations()
    {
        UpdateNKP();
        if (noKeyPress)
        {
            kartAnim.SetTrigger("Idle");
            characterIdle();
        }
        else
        {
            kartAnim.ResetTrigger("Idle");
        }

        if (Input.GetKey(KeyCode.W))
        {
            kartAnim.SetTrigger("Kart_WheelSpin_Forward");
        }
        if (Input.GetKeyUp(KeyCode.W))
        {
            kartAnim.ResetTrigger("Kart_WheelSpin_Forward");
        }

        if (Input.GetKey(KeyCode.S))
        {
            kartAnim.SetTrigger("Kart_WheelSpin_Backward");
        }
        if (Input.GetKeyUp(KeyCode.S))
        {
            kartAnim.ResetTrigger("Kart_WheelSpin_Backward");
        }

        if (Input.GetKey(KeyCode.A))
        {
            Left();
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            kartAnim.ResetTrigger("Kart_TurnLeft");
            characterIdle();
        }

        if (Input.GetKey(KeyCode.D))
        {
            Right();
        }
        if (Input.GetKeyUp(KeyCode.D))
        {
            kartAnim.ResetTrigger("Kart_TurnRight");
            characterIdle();
        }
    }

    void characterIdle()
    {
        characterAnim.ResetTrigger("TurnLeft");
        characterAnim.ResetTrigger("TurnRight");
        characterAnim.SetTrigger("Idle");
    }

    void Right()
    {
        kartAnim.ResetTrigger("Kart_TurnLeft");
        kartAnim.ResetTrigger("Kart_WheelSpin_Forward");
        kartAnim.ResetTrigger("Kart_WheelSpin_Backward");

        characterAnim.ResetTrigger("Idle");
        characterAnim.ResetTrigger("TurnLeft");

        characterAnim.SetTrigger("TurnRight");

        kartAnim.SetTrigger("Kart_TurnRight");
    }

    void Left()
    {
        kartAnim.ResetTrigger("Kart_TurnRight");
        kartAnim.ResetTrigger("Kart_WheelSpin_Forward");
        kartAnim.ResetTrigger("Kart_WheelSpin_Backward");

        characterAnim.ResetTrigger("Idle");
        characterAnim.ResetTrigger("TurnRight");

        characterAnim.SetTrigger("TurnLeft");

        kartAnim.SetTrigger("Kart_TurnLeft");
    }

    private void UpdateNKP()
    {
        noKeyPress = (Input.GetKey(KeyCode.W) == false && Input.GetKey(KeyCode.S) == false) && (Input.GetKey(KeyCode.A) == false && Input.GetKey(KeyCode.D) == false);
    }
}
