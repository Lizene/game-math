using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIControlHandler : MonoBehaviour
{
    public GameObject crane, trolley, cable, hook;
    CraneRotation craneRotation;
    void Start()
    {
        craneRotation = crane.GetComponent<CraneRotation>();
    }
    public void RotateCraneClockwise()
    {
        craneRotation.Rotate(-1);
    }
    public void RotateCraneCounterclockwise()
    {
        craneRotation.Rotate(1);
    }
    public void SetTrolleyPos(float value)
    {
        Debug.Log("Trolley: "+ value.ToString());

    }

    public void SetCableLength(float value)
    {
        Debug.Log("Cable: "+value.ToString());

    }
    void Update()
    {
        
    }
}
