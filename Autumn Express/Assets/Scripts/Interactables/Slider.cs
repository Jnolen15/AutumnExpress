using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slider : MonoBehaviour
{
    // Public variables
    public Transform contactPoint;
    public float maxDist;
    public float pullOffset;
    public float speed;

    // Private variables
    [SerializeField] private float value;
    private bool movingUp = false;
    private bool touching = false;
    private Vector3 target;

    private void Update()
    {
        if (touching)
        {
            if (movingUp)
            {
                target = new Vector3(maxDist, transform.localPosition.y, transform.localPosition.z);
                transform.position = Vector3.Lerp(transform.localPosition, target, Time.deltaTime * speed);
            } else
            {
                target = new Vector3(0, transform.localPosition.y, transform.localPosition.z);
                transform.position = Vector3.Lerp(transform.localPosition, target, Time.deltaTime * speed);
            }
        }

        CalculateValue();
    }

    private void OnMouseDown()
    {
        touching = true;
    }

    private void OnMouseUp()
    {
        touching = false;
    }

    public void OnMouseDrag()
    {
        if (GrabDistance() < pullOffset && GrabDistance() > -pullOffset)
        {
            touching = false;
        }
        else
        {
            touching = true;

            if (GrabDistance() > 0)
                movingUp = true;
            else if (GrabDistance() < 0)
                movingUp = false;
        }
    }

    private Vector3 GetContactScreenPos()
    {
        return Camera.main.WorldToScreenPoint(contactPoint.position);
    }

    private float GrabDistance()
    {
        return (Input.mousePosition.x - GetContactScreenPos().x);
    }

    private float CalculateValue()
    {
        var pos = transform.localPosition.x / maxDist;
        value = Mathf.Abs(pos) * 100;
        return value;
    }
}
