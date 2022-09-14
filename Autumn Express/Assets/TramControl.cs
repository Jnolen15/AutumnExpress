using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TramControl : MonoBehaviour
{
    // Public variables
    public Lever speedLever;
    public Lever doorLever;

    public float speed;
    public bool doorIsOpen;

    // Private variables


    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        speed = speedLever.value;

        doorIsOpen = DoorCheck();
    }

    private bool DoorCheck()
    {
        if (doorLever.value > 80)
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
