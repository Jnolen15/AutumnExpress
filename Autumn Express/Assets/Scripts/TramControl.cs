using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class TramControl : MonoBehaviour
{
    [SerializeField] private AudioMixer mixer;

    public Lever speedLever;
    public Lever doorLever;
    public GameObject door;
    public Lever wwLever;
    public GameObject wiper;
    public GameObject wiperTwo;
    public ParticleSystem windshieldParticles;
    public SliderKnob musicSlider;
    public SliderKnob soundSlider;

    public float speed;
    public int stopsVisited;
    public bool isStopped;
    public bool doorIsOpen;

    // Wiper stuff
    [SerializeField] private float wipeRate;
    private float prevRotation;
    private float maxParticles = 60;
    [SerializeField] private float particleMod = 0;

    void Update()
    {
        // Speed
        speed = speedLever.value;

        // Door
        doorIsOpen = DoorCheck();
        door.transform.rotation = Quaternion.Euler(door.transform.rotation.x, doorLever.value/1.25f, door.transform.rotation.z);

        // Windsheild Wiper
        RainClear();

        // Volume
        SetVolume("MusicVolume", musicSlider.value / 100);
        SetVolume("SoundVolume", soundSlider.value / 100);
    }

    // True if door is open, false otherwise
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

    // Reduces windshild particles based on wiper activity
    private void RainClear()
    {
        // Rotate wiper's with lever, store wipe rate
        wiper.transform.rotation = Quaternion.Euler(wiper.transform.rotation.x, wiper.transform.rotation.y, -(wwLever.value * 2) - 90);
        wiperTwo.transform.rotation = Quaternion.Euler(wiperTwo.transform.rotation.x, wiperTwo.transform.rotation.y, -(wwLever.value * 2) - 90);
        if(wipeRate < 600)
            wipeRate += Mathf.Abs(wwLever.value - prevRotation);
        prevRotation = wwLever.value;

        // Slowly reduce wipe rate
        if(wipeRate > 0)
            wipeRate -= (Time.deltaTime*10);

        // Reduce number of particles based on wipe rate
        var parEmission = windshieldParticles.emission;
        particleMod = maxParticles - (wipeRate / 10);
        if (particleMod < 0)
            particleMod = 0;
        parEmission.rateOverTime = particleMod;
    }

    // Sets speed to 0, locks speed and door levers, Sets isStopped true
    public void MakeStop()
    {
        Debug.Log("Made Stop");
        isStopped = true;
        speed = 0;
        stopsVisited++;
        speedLever.SetValue(0);
        speedLever.locked = true;
    }

    // unlocks speed and door levers, Sets isStopped false
    public void LeaveStop()
    {
        Debug.Log("Can Leave Stop");
        isStopped = false;
        speedLever.locked = false;
        doorLever.SetValue(0);
        doorLever.locked = false;
    }

    // Adjusts mixer volumes
    public void SetVolume(string type, float vol)
    {
        if (type == "MusicVolume" || type == "SoundVolume")
        {
            if (vol == 0) // It can't be 0
                vol = 0.0001f;
            mixer.SetFloat(type, Mathf.Log10(vol) * 20);
        }
        else
        {
            Debug.LogError("Unknown Mixer Type");
            return;
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game");
        Application.Quit();
    }

    public void ToggleFullscreen()
    {
        Debug.Log("Toggling Fullscreen");
        Screen.fullScreen = !Screen.fullScreen;
    }
}
