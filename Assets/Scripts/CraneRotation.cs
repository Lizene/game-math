using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class CraneRotation : MonoBehaviour
{
    public GameObject trolley;
    public float maxAngleSpeed;
    public float accelerationMultiplier = 5;
    [NonSerialized] public UIControlHandler handler;
    TrolleyMovement trolleyMovement;
    float currentAngularVelocity = 0;
    float acceleration;
    float sequenceGoalAngle, currentSequenceAngle;
    int currentAccelerationDir;
    bool sequenceCommenced;
    bool buttonHeld, buttonHeldThisFrame;
    [NonSerialized] public Vector2 crane2Dpos;

    void Start()
    {
        crane2Dpos = new Vector2(transform.position.x, transform.position.z);
        handler = GameObject.FindWithTag("UIHandler").GetComponent<UIControlHandler>();
        acceleration = maxAngleSpeed * accelerationMultiplier;
        trolleyMovement = trolley.GetComponent<TrolleyMovement>();
        trolleyMovement.InitCranePosition(transform.position);
    }

    public void Rotate(int dir)
    {
        if (handler.sequenceActive) return;
        buttonHeldThisFrame = true;
        currentAccelerationDir = dir;
        
    }
    
    void Update()
    {
        if (sequenceCommenced)
        {
            SequenceStep1Rotation();
            return;
        }
        Acceleration();
    }
    void Acceleration()
    {
        
        if (buttonHeld)
        {
            //Accelerate
            if (Mathf.Abs(currentAngularVelocity) < maxAngleSpeed)
            {
                currentAngularVelocity += acceleration*currentAccelerationDir * Time.deltaTime;
                if (currentAngularVelocity > maxAngleSpeed)
                {
                    currentAngularVelocity = maxAngleSpeed;
                }
            }

            //The angular acceleration is dependent on the max speed,
            //to have the same acceleration time regardless of max speed.
        }
        else
        {
            //Decelerate
            if (Mathf.Abs(currentAngularVelocity) > 0)
            {
                currentAngularVelocity += acceleration * (currentAngularVelocity > 0 ? -1 : 1f) * Time.deltaTime;
                if (Mathf.Abs(currentAngularVelocity) < 0.01f)
                {
                    currentAngularVelocity = 0;
                }
            }
        }
        Rotation();
    }
    void Rotation()
    {
        if (Mathf.Abs(currentAngularVelocity) > 0)
        {
            transform.Rotate(Vector3.up, currentAngularVelocity * Time.deltaTime);
            trolleyMovement.UpdateTransform(-transform.right);
        }
    }
    private void LateUpdate()
    {
        buttonHeld = buttonHeldThisFrame;
        buttonHeldThisFrame = false;
    }

    public void SequenceStep1()
    {
        //Wait for the crane to decelerate fully
        if (Mathf.Abs(currentAngularVelocity) > 0.01f)
        {
            Invoke("SequenceStep1", 0.1f);
            return;
        }
        currentAngularVelocity = 0f;
        sequenceCommenced = true;

        Vector3 concretePosition = handler.concretePosition;
        Vector2 concrete2Dpos = new Vector2(concretePosition.x, concretePosition.z);
        Vector2 craneToConcrete = concrete2Dpos-crane2Dpos;
        currentSequenceAngle = transform.eulerAngles.y;
        sequenceGoalAngle = (Vector2.SignedAngle(craneToConcrete, Vector2.left) +360f)%360f;
    }
    void SequenceStep1Rotation()
    {
        currentSequenceAngle = Mathf.SmoothDampAngle(currentSequenceAngle, sequenceGoalAngle,
            ref currentAngularVelocity, 0.3f, maxAngleSpeed);
        transform.rotation = Quaternion.AngleAxis(currentSequenceAngle, Vector3.up);

        trolleyMovement.UpdateTransform(-transform.right);
        if (Mathf.Abs(currentAngularVelocity) < 0.01f)
        {
            currentAngularVelocity = 0;
            currentSequenceAngle = 0; sequenceGoalAngle = 0;
            sequenceCommenced = false;

            handler.SequenceStep2();
        }
    }
}
