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
    private List<Transform> Steps = new List<Transform>();
    private bool isWalkingPath;
    private bool doneMoving;

    void Start()
    {
        state = State.Waiting;
    }

    void Update()
    {
        transform.LookAt(Camera.main.transform.position, Vector3.up);
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
}
