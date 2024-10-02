using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrolleyMovement : MonoBehaviour
{
    public GameObject nearLimitObject, farLimitObject, hook;
    HookMovement hookMovement;
    float nearRadius, farRadius;
    float posY;
    Vector3 cranePosition, craneUpperCenter;
    Quaternion defaultOrientation = Quaternion.Euler(0, 90f, 0);
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
    public void SetPosition(float percentage)
    {
        Vector3 fromCenterNormalized = (transform.position - craneUpperCenter).normalized;
        transform.position = craneUpperCenter+fromCenterNormalized * Mathf.Lerp(nearRadius,farRadius, percentage);
        hookMovement.UpdateTransform(transform.position, transform.rotation);
    }
}
