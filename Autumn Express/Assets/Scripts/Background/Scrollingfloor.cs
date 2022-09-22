using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrollingfloor : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private TramControl tramControl;
    public Transform FirstFloorPiece;

    public GameObject bottomDespawner;

    private Vector3 endingPosition;
    private Vector3 startingPosition;
    private float changingZ;
    private float speedOfTram;
    private float speedOfBakcground;
    private Vector3 goBackPosition;

    private bool reachEnd;
    void Start()
    {
        //tramControl = GameObject.Find("TramControls").GetComponent<TramControl>();
        startingPosition = gameObject.transform.position;
        
        goBackPosition = FirstFloorPiece.position;
        
        endingPosition = bottomDespawner.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        speedOfTram = (tramControl.speed / 2);
        ScrollDecorations();
    }

    private void ScrollDecorations()
    {
        speedOfBakcground += speedOfTram * Time.deltaTime;
        if (gameObject.transform.position.z != endingPosition.z)
        {
            changingZ = Mathf.MoveTowards(startingPosition.z, endingPosition.z, speedOfBakcground);
        }
        else
        {
            changingZ = goBackPosition.z;
            startingPosition.z = goBackPosition.z;
            //reachend = true;
            speedOfBakcground = 1;
        }
        gameObject.transform.position = new Vector3(startingPosition.x, startingPosition.y, changingZ);
        

    }
}
