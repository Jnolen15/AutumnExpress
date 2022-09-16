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

    private bool boardingPassengers = false;

    void Update()
    {
        // IF there is an NPC boarding and the door is open, assign the NPC a path and send it
        if (npcBoarding != null && tramControl.doorIsOpen && !boardingPassengers)
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
            npcScript.AddStep(Seats[NPCList.Count-1]);

            npcScript.WalkPath();
        }

        // Once NPC has sat, clear boarding stuff and call TramControl.leaveStop
        if (npcBoarding != null && boardingPassengers)
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

    // Adds NPC to list and sets the waiting NPC
    public void AddNPC(GameObject npc)
    {
        NPCList.Add(npc);

        npcBoarding = npc;
    }
}
