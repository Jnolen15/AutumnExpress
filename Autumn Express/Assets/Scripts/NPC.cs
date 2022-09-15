using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC : MonoBehaviour
{
    // Public variables
    public enum State
    {
        Waiting,
        Moving,
        Sitting
    }
    public State state;

    public float walkSpeed;

    // Private variables
    // Traversal
    private List<Transform> Steps = new List<Transform>();
    private bool isWalkingPath;
    private bool doneMoving;
    // Sprite animating
    private GameObject sprite;
    private bool stepping = false;
    private float stepAngle = 5;
    private float stepSpeed = 0.7f;

    void Start()
    {
        state = State.Waiting;

        sprite = transform.GetChild(0).gameObject;
    }

    void Update()
    {
        if (state == State.Waiting)
            LookTo(GameObject.FindGameObjectWithTag("Tram").transform);
        else if (state == State.Moving)
        {
            // Animate movement
            if (!stepping)
            {
                stepping = true;
                stepAngle *= -1;
                StartCoroutine(Step(stepAngle));
            }
        }
    }

    private void LookTo(Transform target)
    {
        Vector3 targetPos = new Vector3(target.position.x, transform.position.y, target.position.z);
        transform.LookAt(targetPos, Vector3.up);
    }

    public void AddToTram(GameObject tram)
    {
        tram.GetComponent<NPCManager>().AddNPC(this.gameObject);
    }

    // Adds a transform step to Steps list
    public void AddStep(Transform step)
    {
        Steps.Add(step);
    }

    // Moves NPC along each step in Steps list
    public void WalkPath()
    {
        if (!isWalkingPath)
        {
            Debug.Log("Starting Path");
            isWalkingPath = true;
            state = State.Moving;
            StartCoroutine(MoveAlongSteps());
        }
    }

    IEnumerator MoveAlongSteps()
    {
        foreach(Transform step in Steps)
        {
            doneMoving = false;
            LookTo(step);
            StartCoroutine(MoveToPos(step));
            yield return new WaitUntil(() => doneMoving);

            Debug.Log("Destination Reached");
        }

        Steps.Clear();
        isWalkingPath = false;
        state = State.Sitting;
        Debug.Log("Path Complete");
    }

    IEnumerator MoveToPos(Transform pos)
    {
        Debug.Log("walking to: " + pos);

        float time = 0;
        var startpos = transform.localPosition;

        while (time < walkSpeed)
        {
            transform.localPosition = Vector3.Lerp(startpos, pos.position, time / walkSpeed);

            time += Time.deltaTime;
            yield return null;
        }

        doneMoving = true;
    }

    IEnumerator Step(float angle)
    {
        float time = 0;
        float halfTime = 0;

        // Angle setup
        Quaternion stepStartpos = sprite.transform.localRotation;
        Quaternion stepEndPos = Quaternion.Euler(0, 0, angle);

        // Height setup
        float hopSpeed = stepSpeed / 2f;
        Vector3 hopStartpos = sprite.transform.localPosition;
        Vector3 hopMidPos = new Vector3(0, 1, 0);
        Vector3 hopEndPos = Vector3.zero;

        while (time < stepSpeed)
        {
            // Lerp angle
            sprite.transform.localRotation = Quaternion.Lerp(stepStartpos, stepEndPos, time / stepSpeed);

            // Lerp height. First up, then down
            if (time < hopSpeed)
            {
                sprite.transform.localPosition = Vector3.Lerp(hopStartpos, hopMidPos, time / hopSpeed);
            }
            else if (halfTime < hopSpeed)
            {
                sprite.transform.localPosition = Vector3.Lerp(hopMidPos, hopEndPos, halfTime / hopSpeed);
                halfTime += Time.deltaTime;
            }

            // Advance Time
            time += Time.deltaTime;
            yield return null;
        }

        stepping = false;
    }
}
