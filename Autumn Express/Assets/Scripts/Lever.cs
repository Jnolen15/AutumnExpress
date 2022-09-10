using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : MonoBehaviour
{
    public Transform contactPoint;
    public float maxUpAngle;
    public float maxDownAngle;
    public float pullOffset;
    public float speed;

    private bool movingUp = false;
    private bool touching = false;
    private Quaternion target;

    private void Update()
    {
        if (touching)
        {
            if(movingUp)
                target = Quaternion.Euler(maxUpAngle, transform.rotation.y, transform.rotation.z);
            else
                target = Quaternion.Euler(maxDownAngle, transform.rotation.y, transform.rotation.z);

            transform.rotation = Quaternion.Slerp(transform.rotation, target, Time.deltaTime * speed);
        }
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
        // If gragged past offset move in direction pulled
        else
        {
            touching = true;

            if (GrabDistance() > 0) // Pulled up
                movingUp = true;
            else if (GrabDistance() < 0) // Pulled down
                movingUp = false;
        }
    }

    private Vector3 GetContactScreenPos()
    {
        return Camera.main.WorldToScreenPoint(contactPoint.position);
    }

    private float GrabDistance()
    {
        return (Input.mousePosition.y - GetContactScreenPos().y);
    }
}
