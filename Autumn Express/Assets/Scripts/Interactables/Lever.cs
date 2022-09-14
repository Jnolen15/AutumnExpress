using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    // Public variables
    public Transform contactPoint;
    [HeaderAttribute("Lever goes between 0 and maxAngle (<=90)")]
    public float maxAngle;
    public float pullOffset;
    public float speed;
    public bool vertical;
    public bool reversePullDirection;

    public float value;

    // Private variables
    private bool movingUp = false;
    private bool touching = false;
    private Quaternion target;

    private void Update()
    {
        if (touching)
        {
            if (movingUp)
            {
                if (value > 99)
                    transform.localRotation = Quaternion.Euler(maxAngle, transform.localRotation.y, transform.localRotation.z);
                else
                {
                    target = Quaternion.Euler(maxAngle, transform.localRotation.y, transform.localRotation.z);
                    transform.localRotation = Quaternion.Slerp(transform.localRotation, target, Time.deltaTime * speed);
                }
            }
            else
            {
                if (value < 1)
                    transform.localRotation = Quaternion.Euler(0, transform.localRotation.y, transform.localRotation.z);
                else
                {
                    target = Quaternion.Euler(0, transform.localRotation.y, transform.localRotation.z);
                    transform.localRotation = Quaternion.Slerp(transform.localRotation, target, Time.deltaTime * speed);
                }
            }
        }

        CalculateValue();
    }

    private void OnMouseUp()
    {
        touching = false;
    }

    public void OnMouseDrag()
    {
        // If Grabbing the contact point, but havn't dragged it farther than the offset, don't move
        if (GrabDistance() < pullOffset && GrabDistance() > -pullOffset)
        {
            touching = false;
        } 
        // If dragged past offset move in direction pulled
        else
        {
            touching = true;

            if (GrabDistance() > 0)
                movingUp = !reversePullDirection;
            else if (GrabDistance() < 0)
                movingUp = reversePullDirection;
        }
    }

    private Vector3 GetContactScreenPos()
    {
        return Camera.main.WorldToScreenPoint(contactPoint.position);
    }

    private float GrabDistance()
    {
        if (vertical)
            return (Input.mousePosition.y - GetContactScreenPos().y);
        else
            return (Input.mousePosition.x - GetContactScreenPos().x);
    }

    private float CalculateValue()
    {
        var angle = transform.localRotation.eulerAngles.x;
        value = (angle * 100) / maxAngle;
        return value;
    }
}
