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
        trolleyMovement.SetPercentage(value);
    }
    public void SetCableLength(float value)
    {
        hookMovement.SetPercentage(1-value);
    }
    public void PerformRelocationSequence(Vector3 concretePosition)
    {
        craneRotation.sequenceActive = true;
        trolleyMovement.sequenceActive = true;
        hookMovement.sequenceActive = true;
        craneRotation.concretePosition = concretePosition;
        craneRotation.SequenceStep1();
    }
    public void SequenceStep2()
    {

    }
}
