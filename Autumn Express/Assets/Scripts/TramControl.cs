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
    public SliderKnob musicSlider;
    public SliderKnob soundSlider;

    public float speed;
    public bool doorIsOpen;

    void Start()
    {
        
    }

    void Update()
    {
        // Speed
        speed = speedLever.value;

        // Door
        doorIsOpen = DoorCheck();
        door.transform.rotation = Quaternion.Euler(door.transform.rotation.x, doorLever.value/1.25f, door.transform.rotation.y);

        // Volume
        SetVolume("MusicVolume", musicSlider.value / 100);
        SetVolume("SoundVolume", soundSlider.value / 100);
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

    public void SetVolume(string type, float vol)
    {
        if(type == "MusicVolume" || type == "SoundVolume")
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
}
