using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    public Camera mainCamera;
    public Transform lookDefault;
    public Transform lookDoor;
    public Transform lookMirror;
    public Transform lookSettings;

    public enum Look
    {
        Default,
        Door,
        Mirror,
        Settings
    }
    public Look look;

    [SerializeField] private float edgeSize;
    [SerializeField] private float lookSpeed;
    private bool moving = false;

    void Update()
    {
        if (!moving)
        {
            // Look at Door (Mouse Right)
            if (Input.mousePosition.x > Screen.width - edgeSize && look != Look.Door)
            {
                look = Look.Door;
                StartCoroutine(TransitionLook(lookDoor));
            }
            // Look at Default (Mouse Left)
            if (Input.mousePosition.x < edgeSize && look != Look.Default)
            {
                look = Look.Default;
                StartCoroutine(TransitionLook(lookDefault));
            }
            // Look at Mirror (Mouse Up)
            if (Input.mousePosition.y > Screen.height - edgeSize && look != Look.Mirror)
            {
                look = Look.Mirror;
                StartCoroutine(TransitionLook(lookMirror));
            }
            // Look away from Mirror (Mouse Down)
            if (Input.mousePosition.y < edgeSize && look == Look.Mirror)
            {
                look = Look.Default;
                StartCoroutine(TransitionLook(lookDefault));
            }
            // Look at Settings (Escape Key)
            if (Input.GetKeyDown(KeyCode.Escape) && look != Look.Settings)
            {
                look = Look.Settings;
                StartCoroutine(TransitionLook(lookSettings));
            }
        }
    }

    IEnumerator TransitionLook(Transform lookto)
    {
        moving = true;
        float time = 0;

        Vector3 startPos = mainCamera.transform.position;
        Quaternion startRot = mainCamera.transform.rotation;

        while (time < lookSpeed)
        {
            float t = time / lookSpeed;
            t = t * t * (3f - 2f * t);

            mainCamera.transform.position = Vector3.Lerp(startPos, lookto.position, t);
            mainCamera.transform.rotation = Quaternion.Lerp(startRot, lookto.rotation, t);

            time += Time.deltaTime;
            yield return null;
        }

        moving = false;
    }

    public void MoveCamera(Transform lookto)
    {
        StartCoroutine(TransitionLook(lookto));
    }
}
