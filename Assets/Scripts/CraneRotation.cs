using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneRotation : MonoBehaviour
{
    public GameObject trolley;
    TrolleyMovement trolleyMovement;
    public float maxAngleSpeed;
    float currentAngleSpeed = 0;
    float accelerationMultiplier = 6;
    float acceleration;
    bool buttonHeld, buttonHeldThisFrame;
    int currentAccelerationDir;
    void Start()
    {
        acceleration = maxAngleSpeed * accelerationMultiplier;
        trolleyMovement = trolley.GetComponent<TrolleyMovement>();
        trolleyMovement.InitCranePosition(transform.position);
    }

    public void Rotate(int dir)
    {
        buttonHeldThisFrame = true;
        currentAccelerationDir = dir;
        
    }
    void Update()
    {
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
}
