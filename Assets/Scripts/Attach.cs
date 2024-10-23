using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attach : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Hook"))
        {
            other.GetComponent<HookMovement>().AttachObject(gameObject);
        }
    }
}
