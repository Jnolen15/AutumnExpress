using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public TramControl tramControl;
    public DialogueManager dialogueManager;
    public StopControler stopControler;

    [Header("Points for NPC to walk")]
    [SerializeField] private Transform[] Points;
    public List<GameObject> NPCList = new List<GameObject>();

    public GameObject npcBoarding;
    public GameObject npcLeaving;

    private bool boardingPassengers = false;
    private bool leavingPassengers = false;
    private bool doneUnloading = false;
    private bool alertedNextStop = false;

    public float timeToNextStop;
    public float nextStopTimer = 0;

    public float timeToNextConvo;
    public float nextConvoTimer = 0;
    public bool hadConvo = false;

    void Update()
    {
        if (!tramControl.isStopped && !dialogueManager.GetOpen())
        {
            // Stop generation timer
            if (nextStopTimer < timeToNextStop) nextStopTimer += Time.deltaTime;
            else
            {
                stopControler.CreateStop();
                nextStopTimer = 0;
                Debug.Log("Spawning Stop");
            }

            // Conversation Starting
            if (nextConvoTimer < timeToNextConvo) nextConvoTimer += Time.deltaTime;
            else if (!hadConvo && stopControler.currentStop == null)
            {
                StartConversation();
            }

            // Next stop alert
            if (!alertedNextStop)
                StartCoroutine(NextStop());
        }

        // NPC boarding / leaving
        if (tramControl.isStopped)
        {
            nextConvoTimer = 0;
            hadConvo = false;
            alertedNextStop = false;

            if (npcLeaving == null)
                NPCGetOff();

            if (npcLeaving != null)
                NPCLeave();

            if (npcBoarding != null && doneUnloading)
                NPCBoard();
        }

        // If nobody is leaving or boarding, leave stop
        if(npcBoarding == null && npcLeaving == null && tramControl.isStopped && doneUnloading)
            tramControl.LeaveStop();
    }

    // Adds NPC to list, sets the waiting NPC, Gives NPC stop to get off at
    public void AddNPC(GameObject npc)
    {
        Debug.Log("Adding NPC: " + npc);

        NPCList.Add(npc);

        npcBoarding = npc;

        var stop = tramControl.stopsVisited + Random.Range(2, 5);
        npc.GetComponent<NPC>().getOffStop = stop;
    }

    // Board a passenger by assinging a path
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

            var seat = GameObject.Find(npcScript.favoriteSeat).transform;
            if (seat != null)
                npcScript.AddStep(seat);
            else
                Debug.LogError("Seat not found");

            npcScript.WalkPath();

            // Start Boarding Dialogue
            NPCTalk(npcScript, DialogueManager.ConvoStage.Enter);
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
            }
        }
    }

    // leave a passenger by assinging a path
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

            npcScript.WalkPath();

            // Start Leaving Dialogue
            NPCTalk(npcScript, DialogueManager.ConvoStage.Exit);
        }

        // Once NPC has left, clear loading stuff
        if (leavingPassengers)
        {
            var npcScript = npcLeaving.GetComponent<NPC>();
            if (npcScript.state == NPC.State.Sitting)
            {
                Debug.Log("NPC left");
                Destroy(npcLeaving); // Proboably replace this with something else
                leavingPassengers = false;
                npcLeaving = null;
            }
        }
    }

    // Goes through list of NPCs to find ones getting off stop. Will be called until all NPCs leave (if its their stop)
    private void NPCGetOff()
    {
        doneUnloading = false;

        foreach (GameObject npc in NPCList)
        {
            if(npc.GetComponent<NPC>().getOffStop <= tramControl.stopsVisited)
            {
                npcLeaving = npc;
            }
        }

        if (npcLeaving == null)
            doneUnloading = true;
    }

    // Will bring up dialogue about missing a stop
    public void MissedStop()
    {
        nextConvoTimer = 0;
        hadConvo = false;
        alertedNextStop = false;

        StartCoroutine(MissedStopCoroutine());
    }

    IEnumerator MissedStopCoroutine()
    {
        foreach (GameObject npc in NPCList)
        {
            yield return new WaitUntil(() => !dialogueManager.GetOpen());

            if (npc.GetComponent<NPC>().getOffStop <= tramControl.stopsVisited)
            {
                Debug.Log("NPC Telling you you missed their stop");
                NPCTalk(npc.GetComponent<NPC>(), DialogueManager.ConvoStage.MissedStop);
            }
        }
    }

    // Will bring up dialogue about the next stop for an NPC. Will cycle all npcs this is true for
    IEnumerator NextStop()
    {
        alertedNextStop = true;
        foreach (GameObject npc in NPCList)
        {
            yield return new WaitUntil(() => !dialogueManager.GetOpen());

            if (npc.GetComponent<NPC>().getOffStop == tramControl.stopsVisited + 1)
            {
                Debug.Log("NPC Telling you the their stop is coming up");
                NPCTalk(npc.GetComponent<NPC>(), DialogueManager.ConvoStage.NextStop);
            }
        }
    }

    // Find an NPC who hasn't had their conversation yet and start it
    private void StartConversation()
    {
        NPC npcToTalk = null;
        foreach (GameObject npc in NPCList)
        {
            npcToTalk = npc.GetComponent<NPC>();
            if (!npcToTalk.hadConvo)
                break;
            else
                npcToTalk = null;
        }

        if (npcToTalk != null)
        {
            NPCTalk(npcToTalk, DialogueManager.ConvoStage.Main);
            npcToTalk.hadConvo = true;
        }
        else
            Debug.Log("All passengers have already had conversations");

        hadConvo = true;
    }

    // Set npc and type dialogue
    private void NPCTalk(NPC npcScript, DialogueManager.ConvoStage stage)
    {
        dialogueManager.SetAndStartNewConvo(npcScript.passangerDialogue);
        dialogueManager.StartConvoStage(stage);
    }
}
