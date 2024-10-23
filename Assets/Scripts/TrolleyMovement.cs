using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class TrolleyMovement : MonoBehaviour
{
    public GameObject nearLimitObject, farLimitObject, hook;
    HookMovement hookMovement;
    float nearRadius, farRadius;
    float posY;
    Vector3 cranePosition, craneUpperCenter;
    Quaternion defaultOrientation = Quaternion.Euler(0, 90f, 0);
    float goalPercentage = 1f, currentPercentage = 1f;
    public bool sequenceActive;

    void Start()
    {
        hookMovement = hook.GetComponent<HookMovement>();
    }
    public void InitCranePosition(Vector3 position)
    {
        posY = transform.position.y;

        cranePosition = position;
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
        if (sequenceActive) return;
        goalPercentage = percentage;
    }
    private void Update()
    {
        SetPosition();
    }

    void SetPosition()
    {
        Vector3 posDiff = transform.position - craneUpperCenter;
        Vector3 fromCenterNormalized = posDiff.normalized;
        currentPercentage = Mathf.SmoothStep(currentPercentage, goalPercentage, 0.03f);
        transform.position = craneUpperCenter + fromCenterNormalized * Mathf.Lerp(nearRadius, farRadius, currentPercentage);
        hookMovement.UpdateTransform(transform.position, transform.rotation);
    }
}
