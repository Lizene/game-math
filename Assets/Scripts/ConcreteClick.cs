using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConcreteClick : MonoBehaviour
{
    UIControlHandler handler;
    void Start()
    {
        handler = GameObject.FindWithTag("UIHandler").GetComponent<UIControlHandler>();
    }

    void OnMouseDown()
    {
        Debug.Log("UOHRGbn�bswer�");

        handler.PerformRelocationSequence(transform.position);
    }
}
