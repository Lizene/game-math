using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using Random = Unity.Mathematics.Random;

public class UIControlHandler : MonoBehaviour
{
    public GameObject crane, trolley, cable, hook, concrete;
    [NonSerialized] public bool sequenceActive;
    [NonSerialized] public Vector3 concretePosition;
    CraneRotation craneRotation;
    TrolleyMovement trolleyMovement;
    HookMovement hookMovement;
    static Random rand;
    void Start()
    {
        rand = new Random((uint)DateTime.Now.Millisecond);
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
    public void PerformRelocationSequence(GameObject concrete)
    {
        sequenceActive = true;
        this.concrete = concrete;
        concretePosition = concrete.transform.position; 

        //SequenceStep1
        craneRotation.SequenceStep1();
    }
    public void SequenceStep2()
    {
        trolleyMovement.SequenceStep2();
    }
    public void SequenceStep3()
    {
        hookMovement.SequenceStep3();
    }
    public void WaitForSequenceStep4()
    {
        Invoke("SequenceStep4", 1f);
    }
    public void SequenceStep4()
    {
        hookMovement.SequenceStep4();
    }
    public void SequenceStep5()
    {
        hookMovement.DetachConcrete();
        float randomAngle = rand.NextFloat() * 360f;
        float randomRadius = Mathf.Lerp(trolleyMovement.nearRadius,
            trolleyMovement.farRadius, rand.NextFloat());
        float randomHeight = Mathf.Lerp(10,20,rand.NextFloat());
        Quaternion randomOrientation = Quaternion.Euler(0, rand.NextFloat() * 360f, 0);

        Vector2 newAngleDir2D = new Vector3(Mathf.Sin(Mathf.Deg2Rad*randomAngle), Mathf.Cos(Mathf.Deg2Rad*randomAngle));
        Vector2 crane2Dpos = craneRotation.crane2Dpos;
        Vector2 newConcrete2DPos = crane2Dpos + newAngleDir2D * randomRadius;
        concrete.transform.position = new Vector3(newConcrete2DPos.x, randomHeight, newConcrete2DPos.y);
        concrete.transform.rotation = randomOrientation;

        sequenceActive = false;
    }
}
