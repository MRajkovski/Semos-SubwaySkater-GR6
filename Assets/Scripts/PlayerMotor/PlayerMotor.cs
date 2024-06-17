using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMotor : MonoBehaviour
{
    [HideInInspector] public Vector3 moveVector;
    [HideInInspector] public float verticalVelocity;
    [HideInInspector] public bool isGrounded;
    [HideInInspector] public int currentLane; // -1 0 1

    public float distanceInBetweenLanes = 3.0f;
    public float baseRunSpeed = 5.0f;
    public float baseSidewaySpeed = 10.0f;
    public float gravity = 14.0f;
    public float terminalVelocity = 20.0f;

    public CharacterController controller;
    public Animator anim;

    private BaseState state;
    private bool isPaused;
    


    private void Start()
    {
        isPaused = true;
        anim = GetComponent<Animator>();
        state = GetComponent<RunningState>();
        state.Construct();
        controller = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (!isPaused)
        {
            UpdateMotor();
        }
    }

    private void UpdateMotor()
    {
        // Check if we're grounded
        isGrounded = controller.isGrounded;

        // How should we be moving right now?
        moveVector = state.ProcessMotion();

        // Feed our animator some values
        anim?.SetBool("IsGrounded", isGrounded);
        anim?.SetFloat("Speed", Mathf.Abs(moveVector.z));

        // Are we trying to change state?
        state.Transition();
        // Move the player
        controller.Move(moveVector * Time.deltaTime);
    }

    public void ApplyGravity()
    {
        verticalVelocity -= gravity * Time.deltaTime;
        if(verticalVelocity < -terminalVelocity)
        {
            verticalVelocity = -terminalVelocity;
        }
    }
    public float SnapToLane()
    {
        float r = 0.0f;

        if (transform.position.x != (currentLane * distanceInBetweenLanes)) // If we're not directly on top of a lane
        {
            float deltaToDesiredPosition = (currentLane * distanceInBetweenLanes) - transform.position.x;
            r = (deltaToDesiredPosition > 0) ? 1 : -1;
            r *= baseSidewaySpeed; // r = r*baseSideSpeed

            float actualDistance = r * Time.deltaTime;
            if(Mathf.Abs(actualDistance) > Mathf.Abs(deltaToDesiredPosition))
            {
                r = deltaToDesiredPosition * (1 / Time.deltaTime);
            }
        }
        else
        {
            r = 0;
        }


        return r;
    }

    public void ChangeLane(int direction)
    {
        currentLane = Mathf.Clamp(currentLane + direction, -1, 1);
    }
    public void ChangeState(BaseState s)
    {
        state.Destruct();
        state = s;
        state.Construct();
    }
    public void ResumePlayer()
    {
        isPaused = false;
    }
    public void PausePlayer()
    {
        isPaused = true;
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        string hitLayerName = LayerMask.LayerToName(hit.gameObject.layer);
        if(hitLayerName == "Death")
        {
            ChangeState(GetComponent<DeathState>());
        }
    }

}
