using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Pully : MonoBehaviour
{
    // Public variables
    public Transform contactPoint;
    public float maxPullDist;
    public float pullOffset;
    public float speed;
    public bool down;

    public float value;

    // Private variables
    private CustomCursor cursor;
    private Sounds sound;
    [SerializeField] private UnityEvent triggeredEvent;
    private bool pulled = false;
    private bool touching = false;
    private bool returned = false;
    private Vector3 target;
    private Vector3 defaultPos;

    private void Start()
    {
        cursor = GameObject.FindGameObjectWithTag("Manager").GetComponent<CustomCursor>();
        sound = GameObject.FindGameObjectWithTag("Sound").GetComponent<Sounds>();

        defaultPos = transform.localPosition;
        maxPullDist += transform.localPosition.y;
    }

    private void Update()
    {
        if (touching)
        {
            StopAllCoroutines();
            returned = false;

            if (pulled)
            {
                if (value < 99.9)
                {
                    target = new Vector3(transform.localPosition.x, maxPullDist, transform.localPosition.z);
                    transform.localPosition = Vector3.Lerp(transform.localPosition, target, Time.deltaTime * speed);
                } else
                {
                    transform.localPosition = new Vector3(transform.localPosition.x, maxPullDist, transform.localPosition.z);
                }
            }
        } else if (!returned)
        {
            StartCoroutine(Return());
        }

        CalculateValue();

        if (value > 90)
        {
            touching = false;
            triggeredEvent.Invoke();
        }
    }

    private void OnMouseEnter()
    {
        cursor.SetHover();
    }

    private void OnMouseExit()
    {
        cursor.SetNormal();
    }

    private void OnMouseDown()
    {
        touching = true;
        sound.PlayGrab();
    }

    private void OnMouseUp()
    {
        touching = false;
    }

    public void OnMouseDrag()
    {
        if (down)
        {
            if (GrabDistance() < pullOffset) // Pulled down
                pulled = true;
            else
                pulled = false;
        } else
        {
            if (GrabDistance() > pullOffset) // Pulled down
                pulled = true;
            else
                pulled = false;
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

    private float CalculateValue()
    {
        var pos = transform.localPosition.y / maxPullDist;
        value = Mathf.Abs(pos) * 100;
        return value;
    }

    IEnumerator Return()
    {
        returned = true;

        float time = 0;

        while (time < 1f)
        {
            transform.localPosition = Vector3.Lerp(transform.localPosition, defaultPos, time / 1f);

            time += Time.deltaTime;
            yield return null;
        }
    }
}
