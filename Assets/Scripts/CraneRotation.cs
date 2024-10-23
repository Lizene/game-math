using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class CraneRotation : MonoBehaviour
{
    public GameObject trolley;
    TrolleyMovement trolleyMovement;
    UIControlHandler handler;
    public float maxAngleSpeed;
    float currentAngleSpeed = 0;
    float accelerationMultiplier = 6;
    float acceleration;
    bool buttonHeld, buttonHeldThisFrame;
    int currentAccelerationDir;
    public bool sequenceActive;
    bool sequenceCommenced;
    public Vector3 concretePosition;
    Vector2 crane2Dpos;
    private float startAngle;
    private float goalAngle;
    float sequenceT;
    float sequenceRotationSpeed = 1/4f;

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
        if (sequenceActive) return;
        buttonHeldThisFrame = true;
        currentAccelerationDir = dir;
        
    }
    void Update()
    {
        if (sequenceCommenced)
        {
            float currentSequenceAngle = Mathf.LerpAngle(startAngle, goalAngle, sequenceT);
            transform.rotation = Quaternion.AngleAxis(currentSequenceAngle,Vector3.up);
            trolleyMovement.UpdateTransform(-transform.right);
            sequenceT += Time.deltaTime * sequenceRotationSpeed;
            if (sequenceT >= 1f)
            {
                sequenceCommenced = false;
                sequenceT = 0f;
                handler.SequenceStep2();
            }

            return;
        }
        Acceleration();
    }
    void Acceleration()
    {
        
        if (buttonHeld)
        {
            //Accelerate
            if (Mathf.Abs(currentAngleSpeed) < maxAngleSpeed)
            {
                currentAngleSpeed += acceleration*currentAccelerationDir * Time.deltaTime;
                if (currentAngleSpeed > maxAngleSpeed)
                {
                    currentAngleSpeed = maxAngleSpeed;
                }
            }

            //The angular acceleration is dependent on the max speed,
            //to have the same acceleration time regardless of max speed.
        }
        else
        {
            //Decelerate
            if (Mathf.Abs(currentAngleSpeed) > 0)
            {
                currentAngleSpeed += acceleration * (currentAngleSpeed > 0 ? -1 : 1f) * Time.deltaTime;
                if (Mathf.Abs(currentAngleSpeed) < 0.01f)
                {
                    currentAngleSpeed = 0;
                }
            }
        }
        Rotation();
    }
    void Rotation()
    {
        if (Mathf.Abs(currentAngleSpeed) > 0)
        {
            transform.Rotate(Vector3.up, currentAngleSpeed * Time.deltaTime);
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
        if (Mathf.Abs(currentAngleSpeed) > 0.01f)
        {
            Invoke("SequenceStep1", 0.1f);
            return;
        }
        currentAngleSpeed = 0f;
        sequenceCommenced = true;

        Vector2 concrete2Dpos = new Vector2(concretePosition.x, concretePosition.z);
        Vector2 craneToConcrete = concrete2Dpos-crane2Dpos;
        startAngle = transform.rotation.y;
        goalAngle = Vector2.SignedAngle(Vector2.right, craneToConcrete);

    }
}
