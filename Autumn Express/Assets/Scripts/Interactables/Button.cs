using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour
{
    // public variables
    public Transform contactPoint;
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
        defaultPos = contactPoint.transform.localPosition;
        distance = Vector3.Distance(defaultPos, target);
    }

    private void Update()
    {
        if (pressed)
        {
            if (value < 95)
                contactPoint.transform.localPosition = Vector3.Lerp(contactPoint.transform.localPosition, target, Time.deltaTime * speed);
            else
                contactPoint.transform.localPosition = target;
        } else
        {
            if (value > 5)
                contactPoint.transform.localPosition = Vector3.Lerp(contactPoint.transform.localPosition, defaultPos, Time.deltaTime * speed);
            else
                contactPoint.transform.localPosition = defaultPos;
        }

        CalculateValue();

        if (value > 95)
        {
            triggeredEvent.Invoke();
            pressed = false;
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
        var curDist = Vector3.Distance(contactPoint.transform.localPosition, defaultPos);
        var pos = curDist / distance;
        value = Mathf.Abs(pos) * 100;
        return value;
    }
}
