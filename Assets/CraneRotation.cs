using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraneRotation : MonoBehaviour
{
    public GameObject trolley;
    TrolleyMovement trolleyMovement;
    public float angleSpeed;
    void Start()
    {
        trolleyMovement = trolley.GetComponent<TrolleyMovement>();
    }

    void Update()
    {
        
    }
    public void Rotate(int dir)
    {
        transform.Rotate(Vector3.up, dir * angleSpeed * Time.deltaTime);
        trolleyMovement.UpdateTransform(transform.position, transform.forward);
    }
}
