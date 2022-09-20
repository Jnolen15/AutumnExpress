using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopControler : MonoBehaviour
{
    public List<GameObject> NPCList = new List<GameObject>();

    [SerializeField] private TramControl tramControl;
    [SerializeField] private GameObject stopPrefab;
    [SerializeField] private Transform spawnStartPosition;
    [SerializeField] private Transform spawnEndPosition;

    public GameObject currentStop;
    public float speedOfTram;

    private int npcnum;
    private bool npcRemoved = false;
    private bool firstStop = true;

    private void Start()
    {
        CreateStop();
    }

    void Update()
    {
        speedOfTram = (tramControl.speed / 2);

        if (currentStop != null)
        {
            // Only remove an NPC from the list if it was picked up
            if (currentStop.GetComponent<TramStop>().npcPickedUp && !npcRemoved)
            {
                npcRemoved = true;
                NPCList.Remove(NPCList[npcnum]);
            }

            ScrollStop();
        }
    }

    public void CreateStop()
    {
        if (currentStop == null)
        {
            var pos = new Vector3(spawnStartPosition.position.x - 10, spawnStartPosition.position.y, spawnStartPosition.position.z);
            currentStop = Instantiate(stopPrefab, pos, Quaternion.identity);

            // Spawn NPC MAKE RANDOM IF NOT
            if (NPCList.Count > 0)
            {
                var rand = 0;
                if (firstStop)
                {
                    firstStop = false;
                    rand = 10;
                } else
                    rand = Random.Range(0, 11);
                if (rand > 6)
                {
                    npcnum = Random.Range(0, NPCList.Count);
                    currentStop.GetComponent<TramStop>().waitingNPC = NPCList[npcnum];
                }
            }
        }
        else
            Debug.Log("Stop already exists");
    }

    private void ScrollStop()
    {
        float speedOfBackround = speedOfTram * Time.deltaTime;
        Vector3 startingPosition = currentStop.transform.position;
        float changingZ;

        if (currentStop.transform.position.z != spawnEndPosition.position.z)
        {
            changingZ = Mathf.MoveTowards(startingPosition.z, spawnEndPosition.position.z, speedOfBackround);
            currentStop.transform.position = new Vector3(startingPosition.x, startingPosition.y, changingZ);
        }
        else
        {
            Destroy(currentStop);
            currentStop = null;
        }
    }
}
