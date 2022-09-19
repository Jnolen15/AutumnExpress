using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TramStop : MonoBehaviour
{
    public GameObject waitingNPC;

    private bool busArrived = false;

    void Start()
    {
        if(waitingNPC != null)
            waitingNPC = Instantiate(waitingNPC, this.transform.position, this.transform.rotation, this.transform);
    }

    // If Tram is detected and its speed is less than 20; Make it stop
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Tram")
        {
            if (other.GetComponentInChildren<TramControl>().speed < 20 && !busArrived)
            {
                TramArrived(other.gameObject);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Tram")
        {
            if (!busArrived)
            {
                other.GetComponentInChildren<TramControl>().stopsVisited++;
                Debug.Log("STOP MISSED");
            }
        }
    }

    // Call MakeStop in TramControl and SendNPC to NPCManager
    private void TramArrived(GameObject tram)
    {
        Debug.Log("Tram Arrived");
        busArrived = true;
        tram.GetComponentInChildren<TramControl>().MakeStop();

        // If NPC is waiting, Send them to tram
        if (waitingNPC != null)
            SendNPC(tram.GetComponentInChildren<NPCManager>());
    }

    // Gives waiting NPC to NPCManager
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
