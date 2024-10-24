using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UIElements;

public class HookMovement : MonoBehaviour
{
    public GameObject cable;
    GameObject hookedConcrete;
    [NonSerialized] public UIControlHandler handler;
    public float cableMaxScale, cableMinScale;
    public float cableMaxHeight, cableMinHeight;
    public float attachPointHeight;
    float cableScaleRange;
    float cableStartY;
    float goalPercentage = 0f, currentPercentage = 0f;
    float cableMovementRange;
    Quaternion hookDefaultOrientation, cableDefaultOrientation, hookedConcreteDefaultOrientation;


    //Sequence3
    private bool sequence3Commenced;
    float smoothStepT = 0, smoothStepSpeed = 0.4f;
    float startPercentage;

    //Sequence4
    private bool sequence4Commenced;
    float liftVelocity, liftMaxSpeed = 0.17f, liftAccelerationMultiplier = 0.5f, liftAcceleration;
    float liftPercentage;

    void Start()
    {
        liftAcceleration = liftMaxSpeed * liftAccelerationMultiplier;
        handler = GameObject.FindWithTag("UIHandler").GetComponent<UIControlHandler>();
        cableStartY = cable.transform.position.y;
        hookDefaultOrientation = transform.rotation;
        cableDefaultOrientation = cable.transform.rotation;
        cableMovementRange = cableMaxHeight - cableMinHeight;
        cableScaleRange = cableMaxScale - cableMinScale;
        goalPercentage =  (cableMaxHeight - transform.position.y) / cableMovementRange;
    }
    public void UpdateTransform(Vector3 position, Quaternion rotation)
    {
        Vector3 startPos = new Vector3(position.x, cableMaxHeight, position.z);
        transform.position = startPos + currentPercentage*cableMovementRange*Vector3.down;
        transform.rotation = hookDefaultOrientation*rotation;
        cable.transform.position = new Vector3(transform.position.x, cableStartY, transform.position.z);
        cable.transform.rotation = cableDefaultOrientation * rotation;
        if (hookedConcrete is null) return;
        hookedConcrete.transform.position = transform.position + Vector3.up * attachPointHeight;
        hookedConcrete.transform.rotation = hookedConcreteDefaultOrientation * rotation;
    }
    public void SetPercentage(float percentage)
    {
        if (handler.sequenceActive) return;

        goalPercentage = percentage;
        
    }
    public void AttachObject(GameObject go)
    {
        hookedConcrete = go;
        hookedConcreteDefaultOrientation = go.transform.rotation;
    }
    private void Update()
    {
        Sequence3SmoothStep();
        Sequence4GradualLift();
        LerpHookHeight();
    }
    
    void LerpHookHeight()
    {
        if (Mathf.Abs(currentPercentage - goalPercentage) < 0.001f)
        {
            if (sequence3Commenced)
            {
                smoothStepT = 1f;
                sequence3Commenced = false;
                handler.WaitForSequenceStep4();
            }
            return;
        }
        if (!sequence3Commenced)
        {
            currentPercentage = Mathf.Lerp(currentPercentage, goalPercentage, 0.005f);
        }

        Vector3 startPos = new Vector3(transform.position.x, cableMaxHeight, transform.position.z);
        transform.position = startPos + currentPercentage * cableMovementRange * Vector3.down;
        cable.transform.position = new Vector3(transform.position.x, cableStartY, transform.position.z);

        cable.transform.rotation = cableDefaultOrientation *
            Quaternion.Inverse(hookDefaultOrientation) * transform.rotation;

        cable.transform.localScale = new Vector3(1, cableMinScale + currentPercentage * cableScaleRange, 1);
        if (hookedConcrete is null) return;

        hookedConcrete.transform.position = transform.position + Vector3.up * attachPointHeight;
    }

    public void SequenceStep3()
    {
        sequence3Commenced = true;
        float concreteHeight = handler.concretePosition.y;
        float concretePercentage = 1-Mathf.InverseLerp(cableMinHeight, cableMaxHeight, concreteHeight-attachPointHeight+0.05f);
        smoothStepT = 0;
        startPercentage = currentPercentage;
        goalPercentage = concretePercentage;
    }

    void Sequence3SmoothStep()
    {
        if (!sequence3Commenced) return;
        smoothStepT += smoothStepSpeed * Time.deltaTime;
        currentPercentage = Mathf.SmoothStep(startPercentage, goalPercentage, smoothStepT);
    }
    public void SequenceStep4()
    {
        sequence4Commenced = true;
        liftPercentage = currentPercentage;
        
    }
    void Sequence4GradualLift()
    {
        if (!sequence4Commenced) return;
        if (liftVelocity < liftMaxSpeed)
        {
            liftVelocity += liftAcceleration * Time.deltaTime;
            if (liftVelocity > liftMaxSpeed)
            {
                liftVelocity = liftMaxSpeed;
            }
        }
        liftPercentage -= liftVelocity * Time.deltaTime;
        goalPercentage = liftPercentage;
        if (liftPercentage < 0f)
        {
            liftPercentage = 0f;
            goalPercentage = liftPercentage;
            Invoke("CallSequence5", 1f);
            sequence4Commenced = false;
        }
    }
    void CallSequence5()
    {
        handler.SequenceStep5();
    }
    public void DetachConcrete()
    {
        hookedConcrete = null;
    }
}
