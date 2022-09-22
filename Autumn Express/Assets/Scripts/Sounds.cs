using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sounds : MonoBehaviour
{
    public AudioSource chugga;
    public AudioSource TramInside;
    public AudioSource TramIdling;
    public AudioSource TramWhistle;
    public AudioSource Grab;

    public TramControl tc;

    public float chuggaTimeBase;
    [SerializeField] private float chuggaTimer;
    [SerializeField] private float chuggarunningTime;

    private void Start()
    {
        chuggaTimer = chuggaTimeBase;
    }

    void Update()
    {
        if (tc.speed != 0)
        {
            // Scale chuggaTimer based on speed
            chuggaTimer = chuggaTimeBase - (tc.speed * 0.01f);

            // Adjust tram inside sounds
            TramInside.volume = (tc.speed * 0.01f);

            // Adjust tram Idling sounds
            TramIdling.volume = 0f;

            // Play chugga sound
            if (chuggarunningTime < chuggaTimer) chuggarunningTime += Time.deltaTime;
            else
            {
                chuggarunningTime = 0;
                chugga.Play();
            }
        }
        else
        {
            // Adjust tram Idling sounds
            TramIdling.volume = 0.4f;
        }
    }

    public void PlayWhistle()
    {
        TramWhistle.Play();
    }

    public void PlayGrab()
    {
        Grab.Play();
    }

}
