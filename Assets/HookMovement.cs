using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class HookMovement : MonoBehaviour
{
    public GameObject cable;
    GameObject hookedObject;
    public float cableMaxScale, cableMinScale;
    public float cableMaxHeight, cableMinHeight;
    public float attachPointHeight;
    float cableScaleRange;
    float cableStartY;
    float extensionPercent;
    float cableMovementRange;
    Quaternion hookDefaultOrientation, cableDefaultOrientation, hookedObjectDefaultOrientation;
    void Start()
    {
        cableStartY = cable.transform.position.y;
        hookDefaultOrientation = transform.rotation;
        cableDefaultOrientation = cable.transform.rotation;
        cableMovementRange = cableMaxHeight - cableMinHeight;
        cableScaleRange = cableMaxScale - cableMinScale;
        extensionPercent =  (cableMaxHeight - transform.position.y) / cableMovementRange;
    }
    public void UpdateTransform(Vector3 position, Quaternion rotation)
    {
        Vector3 startPos = new Vector3(position.x, cableMaxHeight, position.z);
        transform.position = startPos + extensionPercent*cableMovementRange*Vector3.down;
        transform.rotation = hookDefaultOrientation*rotation;
        cable.transform.position = new Vector3(transform.position.x, cableStartY, transform.position.z);
        cable.transform.rotation = cableDefaultOrientation * rotation;
        if (hookedObject is null) return;
        hookedObject.transform.position = transform.position + Vector3.up * attachPointHeight;
        hookedObject.transform.rotation = hookedObjectDefaultOrientation * rotation;
    }
    public void SetHookHeight(float percentage)
    {
        extensionPercent = percentage;
        Vector3 startPos = new Vector3(transform.position.x, cableMaxHeight, transform.position.z);
        transform.position = startPos + percentage * cableMovementRange * Vector3.down;
        cable.transform.position = new Vector3(transform.position.x, cableStartY, transform.position.z);
        cable.transform.rotation = cableDefaultOrientation * 
            Quaternion.Inverse(hookDefaultOrientation) * transform.rotation;
        cable.transform.localScale = new Vector3(1, cableMinScale + extensionPercent * cableScaleRange, 1);
        if (hookedObject is null) return;
        hookedObject.transform.position = transform.position + Vector3.up * attachPointHeight;
    }
    public void AttachObject(GameObject go)
    {
        hookedObject = go;
        hookedObjectDefaultOrientation = go.transform.rotation;
    }
}
