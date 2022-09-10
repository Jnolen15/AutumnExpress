using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slider : MonoBehaviour
{
    public Transform contactPoint;
    public float maxDist;
    public float minDist;
    public float pullOffset;
    public float speed;

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
                target = new Vector3(minDist, transform.localPosition.y, transform.localPosition.z);
                transform.position = Vector3.Lerp(transform.localPosition, target, Time.deltaTime * speed);
            }
        }
        else
        {
            //transform.position = Vector3.Lerp(transform.localPosition, defaultPos, Time.deltaTime * (speed * 2));
        }
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
        // If Grabbing the contact point, but havn't dragged it farther than the offset, don't move
        if (GrabDistance() < pullOffset && GrabDistance() > -pullOffset)
        {
            touching = false;
        }
        // If gragged past offset move in direction pulled
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
}
