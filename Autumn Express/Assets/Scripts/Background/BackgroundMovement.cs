using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMovement : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] private GameObject prefab;
    [SerializeField] private int numToInstantiate;
    [SerializeField] private Transform spawnStartPosition;
    [SerializeField] private Transform spawnEndPosition;

    // Spawners and Despawners
    public GameObject topSpawner;
    public GameObject bottomDespawner;

    //Increase speed of bacground movement
    //public float speedOfBackround;
    public float speedOfTram;
    public float spawnCollisionCheckRadius;

    //Getting all of the children
    public List<GameObject> children = new List<GameObject>();

    //private Vector3 startingPosition;
    private Vector3 endingPosition;
    private Vector3 spawnPosition;

    private bool reachend = false;

    //private float changingZ;

    private void Awake()
    {
        //Spawn number of background objects between the spawnStart and end
        List<Vector3> spawns = new List<Vector3>();
        for (int i = 0; i < numToInstantiate; i++)
        {
            bool goodToParent = false;
            Vector3 randomPoint = new Vector3(Random.Range(spawnStartPosition.position.x, spawnEndPosition.position.x), 1, Random.Range(spawnStartPosition.position.z, spawnEndPosition.position.z));
            spawns.Add(randomPoint);
            foreach (Vector3 tree in spawns)
            {
                if (Vector3.Distance(randomPoint, tree) > spawnCollisionCheckRadius)
                {
                    goodToParent = true;
                }
                    
            }

            
            if (goodToParent) 
            {
                var newTree = Instantiate(prefab, randomPoint, Quaternion.identity);
                newTree.transform.parent = gameObject.transform;
            }
            
            //currentPos += posOffset;
        }
    }
    void Start()
    {
        //startingPosition = transform.position;
        spawnPosition = topSpawner.transform.position;
        endingPosition = bottomDespawner.transform.position;
        for(int i = 0; i < transform.childCount; i++)
        {
            children.Add(gameObject.transform.GetChild(i).gameObject);
        }

        Debug.Log(children.Count);
    }

    // Update is called once per frame
    void Update()
    {
        foreach (GameObject backgroundObject in children)
        {
            float speedOfBackround = speedOfTram * Time.deltaTime;
            Vector3 startingPosition = backgroundObject.transform.position;
            //transform.position = new Vector3(transform.position.x, transform.position.y, topSpawner.transform.position.z);
            //
            //}
            //else if(changingZ != endingPosition.z)
            //{
            float changingZ = 0;
            if (backgroundObject.transform.position.z != endingPosition.z)
            {
                changingZ = Mathf.MoveTowards(startingPosition.z, endingPosition.z, speedOfBackround);
            }
            else
            {
                changingZ = spawnPosition.z;
                startingPosition.z = spawnPosition.z;
                //reachend = true;
                speedOfBackround = 1;
            }



            backgroundObject.transform.position = new Vector3(startingPosition.x, startingPosition.y, changingZ);
        }

    }

}
