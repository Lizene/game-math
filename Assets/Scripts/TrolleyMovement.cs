using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrolleyMovement : MonoBehaviour
{
    public GameObject nearLimitObject, farLimitObject;
    float nearRadius, farRadius;
    float posY;
    Vector3 cranePosition, craneUpperCenter;
    void Start()
    {
        
    }
    public void SetCranePosition(Vector3 position)
    {
        posY = transform.position.y;

        cranePosition = position;
        craneUpperCenter = new Vector3(cranePosition.x, posY, cranePosition.z);
        Debug.Log(craneUpperCenter);
        nearRadius = Vector3.Distance(craneUpperCenter,nearLimitObject.transform.position);
        Debug.Log(nearRadius);
        farRadius = Vector3.Distance(craneUpperCenter,farLimitObject.transform.position);
        Debug.Log(farRadius);
    }
    public void UpdateTransform(Vector3 craneForward)
    {
        float distanceFromCrane = Vector3.Distance(craneUpperCenter,transform.position);
        transform.position = craneUpperCenter+craneForward * distanceFromCrane;
        transform.rotation = Quaternion.Euler(0,90f,0)*Quaternion.LookRotation(craneForward);
    }
    public void SetPosition(float percentage)
    {
        Vector3 fromCenterNormalized = (transform.position - craneUpperCenter).normalized;
        transform.position = craneUpperCenter+fromCenterNormalized * Mathf.Lerp(nearRadius,farRadius, percentage);
    }
}
