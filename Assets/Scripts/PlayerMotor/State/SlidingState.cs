using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlidingState : BaseState
{
    public float slideDuration = 1.0f; // How long the slide will last

    // Collider logic
    private Vector3 initialCenter; // Center of our controller
    private float initialSize; // Its original height
    private float slideStart; // When does the slide start exactly

    public override void Construct()
    {
        slideStart = Time.time; // Start the timer

        initialSize = motor.controller.height; // We need an exact height of the controller
        initialCenter = motor.controller.center; // The exact controller center position

        motor.controller.height = initialSize * 0.5f; // Make the controller shorter by half
        motor.controller.center = initialCenter * 0.5f; // Push the controller's center to half its value downwards
        motor.anim?.SetTrigger("Slide");
    }

    public override void Destruct()
    {
        motor.controller.height = initialSize;  // We cached the original size to reset it when we exit state
        motor.controller.center = initialCenter; // We cached the original center to reset it when we exit state

        motor.anim?.SetTrigger("Running");
    }

    public override void Transition()
    {
        if (InputManager.Instance.SwipeLeft)
        {
            motor.ChangeLane(-1);
        }
        if (InputManager.Instance.SwipeRight)
        {
            motor.ChangeLane(1);
        }
        if (!motor.isGrounded)
        {
            motor.ChangeState(GetComponent<FallingState>());
        }
        if (InputManager.Instance.SwipeUp) // If player swipes up
        {
            motor.ChangeState(GetComponent<JumpingState>());
        }
        if (Time.time - slideStart > slideDuration) // If the slide takes longer then 1s, change back to running state
        {
            motor.ChangeState(GetComponent<RunningState>());
        }
    }

    public override Vector3 ProcessMotion()
    {
        Vector3 m = Vector3.zero;

        m.x = motor.SnapToLane();
        m.y = -1.0f;
        m.z = motor.baseRunSpeed;

        return m;
    }

}
