using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour
{
    // public variables
    public Vector3 target;
    public float speed;

    public float value;

    // Private variables
    [SerializeField] private UnityEvent triggeredEvent;
    private bool pressed = false;
    private float distance;
    private Vector3 defaultPos;

    private void Start()
    {
        defaultPos = transform.position;
        distance = Vector3.Distance(defaultPos, target);
    }

    private void Update()
    {
        if (pressed)
        {
            if (value < 95)
                transform.position = Vector3.Lerp(transform.position, target, Time.deltaTime * speed);
            else
                transform.position = target;
        } else
        {
            if (value > 5)
                transform.position = Vector3.Lerp(transform.position, defaultPos, Time.deltaTime * speed);
            else
                transform.position = defaultPos;
        }

        CalculateValue();

        if (value > 95)
        {
            triggeredEvent.Invoke();
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

    private float CalculateValue()
    {
        var curDist = Vector3.Distance(transform.localPosition, defaultPos);
        var pos = curDist / distance;
        value = Mathf.Abs(pos) * 100;
        return value;
    }
}
