using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pully : MonoBehaviour
{
    public Transform contactPoint;
    public float maxPullDist;
    public float pullOffset;
    public float speed;

    private bool pulled = false;
    private bool touching = false;
    private Vector3 target;
    private Vector3 defaultPos;

    private void Start()
    {
        defaultPos = transform.localPosition;
        maxPullDist += transform.localPosition.y;
    }

    private void Update()
    {
        if (touching)
        {
            if (pulled)
            {
                target = new Vector3(transform.localPosition.x, maxPullDist, transform.localPosition.z);

                transform.position = Vector3.Lerp(transform.localPosition, target, Time.deltaTime * speed);
            }
        } else
        {
            transform.position = Vector3.Lerp(transform.localPosition, defaultPos, Time.deltaTime * (speed*2));
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
        if (GrabDistance() < 0) // Pulled down
            pulled = true;
        else
            pulled = false;
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
