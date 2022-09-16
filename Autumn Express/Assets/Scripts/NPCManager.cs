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

    void Update()
    {
        NPCGetOff();

        if (npcBoarding != null && tramControl.isStopped)
            NPCBoard();

        if (npcLeaving != null && tramControl.isStopped)
            NPCLeave();
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

        // Once NPC has sat, clear boarding stuff and call TramControl.leaveStop
        if (boardingPassengers)
        {
            var npcScript = npcBoarding.GetComponent<NPC>();
            if (npcScript.state == NPC.State.Sitting)
            {
                Debug.Log("NPC is sat, can leave now");
                boardingPassengers = false;
                npcBoarding = null;
                tramControl.LeaveStop();
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
            for(int i = Points.Length-1; i >= 0; i--)
            {
                npcScript.AddStep(Points[i]);
            }

            //npcScript.AddStep(Seats[NPCList.Count - 1]);

            npcScript.WalkPath();
        }

        // Once NPC has sat, clear boarding stuff and call TramControl.leaveStop
        if (leavingPassengers)
        {
            var npcScript = npcLeaving.GetComponent<NPC>();
            if (npcScript.state == NPC.State.Sitting)
            {
                Debug.Log("NPC left, can leave now");
                leavingPassengers = false;
                npcLeaving = null;
                tramControl.LeaveStop();
            }
        }
    }

    // Adds NPC to list and sets the waiting NPC
    public void AddNPC(GameObject npc)
    {
        NPCList.Add(npc);

        npcBoarding = npc;
    }

    // Picks an NPC to get off the tram (WIP CHANGE LATER, JUST FOR TESTING)
    private void NPCGetOff()
    {
        if(NPCList.Count > 0)
            npcLeaving = NPCList[0];
    }
}
