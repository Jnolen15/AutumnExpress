using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TracksScrolling : MonoBehaviour
{
    // Code is the same as backgroundMovement but takes out the random spawning and keeps the alpha to see if its needed
    [SerializeField] private TramControl tramControl;

    // Spawners and Despawners
    public GameObject topSpawner;
    public GameObject bottomDespawner;

    //Increase speed of bacground movement
    //public float speedOfBackround;
    public float speedOfTram;

    //Getting all of the children
    public List<GameObject> children = new List<GameObject>();

    //private Vector3 startingPosition;
    private Vector3 endingPosition;
    private Vector3 spawnPosition;
    void Start()
    {
        //startingPosition = transform.position;
        spawnPosition = topSpawner.transform.position;
        endingPosition = bottomDespawner.transform.position;
        for (int i = 0; i < transform.childCount; i++)
        {
            children.Add(gameObject.transform.GetChild(i).gameObject);
        }
    }

    void Update()
    {
        speedOfTram = (tramControl.speed / 2);

        ScrollDecorations();
    }

    private void ScrollDecorations()
    {
        foreach (GameObject backgroundObject in children)
        {
            float speedOfBackround = speedOfTram * Time.deltaTime;
            Vector3 startingPosition = backgroundObject.transform.position;
            //Color forAlpha = backgroundObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color;
            //transform.position = new Vector3(transform.position.x, transform.position.y, topSpawner.transform.position.z);
            //
            //}
            //else if(changingZ != endingPosition.z)
            //{
            float changingZ = 0;
            if (backgroundObject.transform.position.z != endingPosition.z)
            {
                //forAlpha.a = Mathf.Lerp(backgroundObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color.a, 1, speedOfBackround / 10);
                changingZ = Mathf.MoveTowards(startingPosition.z, endingPosition.z, speedOfBackround);
            }
            else
            {
                //forAlpha.a = 0;
                changingZ = spawnPosition.z;
                startingPosition.z = spawnPosition.z;
                //reachend = true;
                speedOfBackround = 1;
            }
            //Fade background Objects when nearing the end
            if (Mathf.Abs(backgroundObject.transform.position.z - endingPosition.z) < 150)
            {
                //Debug.Log("approaching End");
                //forAlpha.a = Mathf.Lerp(backgroundObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color.a, 0, speedOfBackround / 50);
            }

            backgroundObject.transform.position = new Vector3(startingPosition.x, startingPosition.y, changingZ);
            //backgroundObject.transform.GetChild(0).GetComponent<SpriteRenderer>().color = forAlpha;
        }

    }
}
