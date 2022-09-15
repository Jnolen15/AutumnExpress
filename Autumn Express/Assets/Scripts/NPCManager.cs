using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCManager : MonoBehaviour
{
    public TramControl tramControl;

    [Header("Points for NPC to walk")]
    [SerializeField] private Transform[] Points;
    public List<GameObject> NPCList = new List<GameObject>();

    public GameObject npcWaitingtoBoard;

    void Update()
    {
        if (npcWaitingtoBoard != null && tramControl.doorIsOpen)
        {
            Debug.Log("NPC Coming Abord");

            var npcScript = npcWaitingtoBoard.GetComponent<NPC>();
            foreach (Transform point in Points)
            {
                npcScript.AddStep(point);
            }

            npcScript.WalkPath();

            npcWaitingtoBoard = null;
        }
    }

    public void AddNPC(GameObject npc)
    {
        NPCList.Add(npc);

        npcWaitingtoBoard = npc;
    }
}
