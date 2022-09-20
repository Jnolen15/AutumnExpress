using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManager : MonoBehaviour
{
    public GameObject MainMenu;
    public GameObject EndMenu;
    public CameraLook camControl;
    public Transform defaultCamPos;

    public StopControler stopControler;
    public NPCManager npcMan;

    private bool gameEnded = false;

    void Start()
    {
        Debug.Log("Game opened, pausing");
        Time.timeScale = 0;
    }

    void Update()
    {
        if (stopControler.NPCList.Count == 0 && npcMan.NPCList.Count == 0 && !gameEnded)
        {
            EndGame();
        }
    }

    public void StartGame()
    {
        Debug.Log("Game started, un-pausing");
        MainMenu.SetActive(false);
        Time.timeScale = 1;

        camControl.MoveCamera(defaultCamPos);
    }

    public void EndGame()
    {
        Debug.Log("All passengers picked up and dropped off. Game ending");
        gameEnded = true;
        EndMenu.SetActive(true);
        Time.timeScale = 0;
    }
}
