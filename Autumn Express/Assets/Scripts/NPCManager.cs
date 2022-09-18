using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public TramControl tramControl;

    [Header("Points for NPC to walk")]
    [SerializeField] private Transform[] Points;
    [SerializeField] private Transform[] Seats;
    public List<GameObject> NPCList = new List<GameObject>();

    public GameObject npcBoarding;
    public GameObject npcLeaving;

    private bool boardingPassengers = false;
    private bool leavingPassengers = false;
    private bool doneUnloading = false;

    void Update()
    {
        if (tramControl.isStopped)
        {
            if (npcLeaving == null)
                NPCGetOff();

            if (npcBoarding != null)
                NPCBoard();

            if (npcLeaving != null)
                NPCLeave();
        }

        // If nobody is leaving or boarding, leave stop
        if(npcBoarding == null && npcLeaving == null && tramControl.isStopped && doneUnloading)
            tramControl.LeaveStop();
    }

    private void NPCBoard()
    {
        // IF there is an NPC boarding and the door is open, assign the NPC a path and send it
        if (tramControl.doorIsOpen && !boardingPassengers)
        {
            Debug.Log("NPC Coming Abord");
            tramControl.doorLever.locked = true;
            boardingPassengers = true;
            var npcScript = npcBoarding.GetComponent<NPC>();
            foreach (Transform point in Points)
            {
                npcScript.AddStep(point);
            }

            // MAKE THIS BETTER, IT WILL MAKE PEOPLE TO SIT IN THE SAME SEAT SOMETIMES
            npcScript.AddStep(Seats[NPCList.Count - 1]);

            npcScript.WalkPath();
        }

        // Once NPC has sat, clear boarding stuff
        if (boardingPassengers)
        {
            var npcScript = npcBoarding.GetComponent<NPC>();
            if (npcScript.state == NPC.State.Sitting)
            {
                Debug.Log("NPC is sat");
                boardingPassengers = false;
                npcBoarding = null;
                //tramControl.LeaveStop();
            }
        }
    }

    private void NPCLeave()
    {
        // IF there is an NPC leaving and the door is open, assign the NPC a path and send it
        if (tramControl.doorIsOpen && !leavingPassengers)
        {
            Debug.Log("NPC Getting Off");
            tramControl.doorLever.locked = true;
            leavingPassengers = true;
            var npcScript = npcLeaving.GetComponent<NPC>();
            NPCList.Remove(npcLeaving);
            for (int i = Points.Length-1; i >= 0; i--)
            {
                npcScript.AddStep(Points[i]);
            }

            //npcScript.AddStep(Seats[NPCList.Count - 1]);

            npcScript.WalkPath();
        }

        // Once NPC has left, clear loading stuff
        if (leavingPassengers)
        {
            var npcScript = npcLeaving.GetComponent<NPC>();
            if (npcScript.state == NPC.State.Sitting)
            {
                Debug.Log("NPC left");
                leavingPassengers = false;
                npcLeaving = null;
                //tramControl.LeaveStop();
            }
        }
    }

    // Adds NPC to list, sets the waiting NPC, Gives NPC stop to get off at
    public void AddNPC(GameObject npc)
    {
        Debug.Log("Adding NPC: " + npc);
        
        NPCList.Add(npc);

        npcBoarding = npc;

        var stop = tramControl.stopsVisited + Random.Range(1, 5);
        npc.GetComponent<NPC>().getOffStop = stop;
    }

    // Goes through list of NPCs to find ones getting off stop. Will be called until all NPCs leave (if its their stop)
    private void NPCGetOff()
    {
        doneUnloading = false;

        foreach (GameObject npc in NPCList)
        {
            if(npc.GetComponent<NPC>().getOffStop == tramControl.stopsVisited)
            {
                npcLeaving = npc;
            }
        }

        if (npcLeaving == null)
            doneUnloading = true;
    }
}
