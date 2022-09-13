using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMovement : MonoBehaviour
{
    // Start is called before the first frame update
    // Spawners and Despawners
    public GameObject topSpawner;
    public GameObject bottomDespawner;

    //Increase speed of bacground movement
    public float speedOfBackround;
    public float speedOfTram;

    private Vector3 startingPosition;
    private Vector3 endingPosition;
    private Vector3 spawnPosition;

    private bool reachend = false;

    private float changingZ;
    void Start()
    {
        startingPosition = transform.position;
        spawnPosition = topSpawner.transform.position;
        endingPosition = bottomDespawner.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        speedOfBackround += speedOfTram * Time.deltaTime;

            //transform.position = new Vector3(transform.position.x, transform.position.y, topSpawner.transform.position.z);
            //
        //}
        //else if(changingZ != endingPosition.z)
        //{
        if (!reachend)
        {
            changingZ = Mathf.MoveTowards(startingPosition.z, endingPosition.z, speedOfBackround);
        }
            
            
        
        transform.position = new Vector3(startingPosition.x, startingPosition.y, changingZ);
        


    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject == bottomDespawner)
        {
            changingZ = spawnPosition.z;
            startingPosition.z = spawnPosition.z;
            reachend = true;
            speedOfBackround = 1;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == bottomDespawner)
        {
            reachend = false;
        }
    }

}
