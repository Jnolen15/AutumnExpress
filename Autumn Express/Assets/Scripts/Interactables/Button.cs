using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour
{
    public Vector3 target;
    public float speed;

    private bool pressed = false;
    private Vector3 defaultPos;

    private void Start()
    {
        defaultPos = transform.position;
    }

    private void Update()
    {
        if (pressed)
        {
            transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * speed);
        } else
        {
            transform.position = Vector3.Lerp(transform.position, defaultPos, Time.deltaTime * speed);
        }
    }

    private void OnMouseDown()
    {
        pressed = true;
    }

    private void OnMouseUp()
    {
        pressed = false;
    }
}
