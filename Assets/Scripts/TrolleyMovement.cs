using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class TrolleyMovement : MonoBehaviour
{
    public GameObject nearLimitObject, farLimitObject, hook;
    HookMovement hookMovement;
    [NonSerialized] public UIControlHandler handler;
    [NonSerialized] public float nearRadius, farRadius;
    float posY;
    Vector3 cranePosition, craneUpperCenter;
    Vector2 cranePosition2D;

    Quaternion defaultOrientation = Quaternion.Euler(0, 90f, 0);
    float goalPercentage = 1f, currentPercentage = 1f;
    bool sequenceCommenced;

    float smoothStepT = 0.1f;

    void Start()
    {
        handler = GameObject.FindWithTag("UIHandler").GetComponent<UIControlHandler>();
        hookMovement = hook.GetComponent<HookMovement>();
    }
    public void InitCranePosition(Vector3 position)
    {
        posY = transform.position.y;

        cranePosition = position;
        cranePosition2D = new Vector2(cranePosition.x, cranePosition.z);
        craneUpperCenter = new Vector3(cranePosition.x, posY, cranePosition.z);
        nearRadius = Vector3.Distance(craneUpperCenter,nearLimitObject.transform.position);
        farRadius = Vector3.Distance(craneUpperCenter,farLimitObject.transform.position);
    }
    public void UpdateTransform(Vector3 craneForward)
    {
        float distanceFromCrane = Vector3.Distance(craneUpperCenter,transform.position);
        transform.position = craneUpperCenter+craneForward * distanceFromCrane;
        transform.rotation = defaultOrientation*Quaternion.LookRotation(craneForward);
        hookMovement.UpdateTransform(transform.position, transform.rotation);
    }
    public void SetPercentage(float percentage)
    {
        if (handler.sequenceActive) return;
        goalPercentage = percentage;
    }
    private void Update()
    {
        LerpPosition();
    }

    void LerpPosition()
    {
        if (Mathf.Abs(currentPercentage - goalPercentage) < 0.01f)
        {
            if (sequenceCommenced)
            {
                sequenceCommenced = false;
                handler.SequenceStep3();
            }
            return;
        }

        Vector3 posDiff = transform.position - craneUpperCenter;
        Vector3 fromCenterNormalized = posDiff.normalized;

        currentPercentage = Mathf.Lerp(currentPercentage, goalPercentage, 0.005f);
        transform.position = craneUpperCenter + fromCenterNormalized * Mathf.Lerp(nearRadius, farRadius, currentPercentage);
        hookMovement.UpdateTransform(transform.position, transform.rotation);

        
    }

    public void SequenceStep2()
    {
        Vector3 concretePosition = handler.concretePosition;
        Vector2 concretePosition2D = new Vector2(concretePosition.x, concretePosition.z);
        float concreteDistance = (concretePosition2D-cranePosition2D).magnitude;
        float concretePercentage = Mathf.InverseLerp(nearRadius, farRadius, concreteDistance);
        goalPercentage = concretePercentage;
        sequenceCommenced = true;
    }
}
