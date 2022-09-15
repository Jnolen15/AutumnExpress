using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TramStop : MonoBehaviour
{
    public GameObject waitingNPC;

    void Start()
    {
        waitingNPC = Instantiate(waitingNPC, this.transform.position, this.transform.rotation, this.transform);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Tram")
        {
            Debug.Log("Tram Arrived");
            SendNPC(other.GetComponentInChildren<NPCManager>());
        }
    }

    private void SendNPC(NPCManager npcMan)
    {
        if (npcMan != null)
        {
            Debug.Log(waitingNPC.gameObject.name + " is now waiting");

            npcMan.AddNPC(waitingNPC);

            waitingNPC.transform.SetParent(npcMan.gameObject.transform);

            waitingNPC = null;
        } else
        {
            Debug.LogError("NPCManager script not found");
        }
    }
}
