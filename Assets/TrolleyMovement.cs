using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrolleyMovement : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateTransform(Vector3 cranePosition, Vector3 craneForward)
    {
        Vector3 position = transform.position;
        float distanceFromCrane = Mathf.Sqrt(Mathf.Pow(position.x-cranePosition.x,2) + Mathf.Pow(position.y,2));
        transform.position = craneForward * distanceFromCrane;
        transform.rotation = Quaternion.LookRotation(craneForward);
    }
}
