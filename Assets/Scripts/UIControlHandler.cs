using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class UIControlHandler : MonoBehaviour
{
    public GameObject crane, trolley, cable, hook;
    CraneRotation craneRotation;
    TrolleyMovement trolleyMovement;
    HookMovement hookMovement;
    void Start()
    {
        craneRotation = crane.GetComponent<CraneRotation>();
        trolleyMovement = trolley.GetComponent<TrolleyMovement>();
        hookMovement = hook.GetComponent<HookMovement>();
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
        trolleyMovement.SetPosition(value);
    }

    public void SetCableLength(float value)
    {
        hookMovement.SetHookHeight(1-value);

    }
    void Update()
    {
        
    }
}
