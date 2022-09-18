using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopControler : MonoBehaviour
{
    [SerializeField] private TramControl tramControl;
    [SerializeField] private GameObject stopPrefab;
    [SerializeField] private Transform spawnStartPosition;
    [SerializeField] private Transform spawnEndPosition;

    public GameObject currentStop;
    public float speedOfTram;

    private void Start()
    {
        CreateStop();
    }

    void Update()
    {
        speedOfTram = (tramControl.speed / 2);

        if (currentStop != null)
            ScrollStop();
    }

    public void CreateStop()
    {
        if (currentStop == null)
        {
            var pos = new Vector3(spawnStartPosition.position.x - 10, spawnStartPosition.position.y, spawnStartPosition.position.z);
            currentStop = Instantiate(stopPrefab, pos, Quaternion.identity);
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
        }
        else
        {
            changingZ = spawnStartPosition.position.z;
            startingPosition.z = spawnStartPosition.position.z;
        }

        currentStop.transform.position = new Vector3(startingPosition.x, startingPosition.y, changingZ);
    }
}
