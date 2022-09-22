using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public DialogueDisplayController dlogDisplay;
    public bool testMode = false; // Right click to open box, left click to play lines
    public PassangerDialogue testPassengerDialogue;

    private ConvoStage currStage;
    private int currLineNumber;
    private PassangerDialogue currConvoData;
    
    public enum ConvoStage
    {
        Enter,
        Main,
        Exit,
        MissedStop,
        NextStop
    }
    
    void Start()
    {
        currConvoData = testPassengerDialogue;
    }

    void Update()
    {
        if (testMode)
        {
            if (Input.GetKeyDown(KeyCode.Y))
            {
                StartConvoStage(ConvoStage.Enter);
            }
            else if (Input.GetKeyDown(KeyCode.U))
            {
                StartConvoStage(ConvoStage.Main);
            }
            else if (Input.GetKeyDown(KeyCode.I))
            {
                StartConvoStage(ConvoStage.Main);
            }
        }

        if (dlogDisplay.getIsDialogueBoxOpen() && Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (dlogDisplay.getIsCurrentLineFinished())
            {
                if (currLineNumber != GetLinesFromStage(currStage).Length - 1)
                {
                    currLineNumber++;
                    dlogDisplay.TypeNewDlogLine(GetLinesFromStage(currStage)[currLineNumber]);
                }
                else
                {
                    dlogDisplay.CloseDialogueBox();
                }
            }
            else
            {
                dlogDisplay.SkipToEndOfLine();
            }
        }
    }

    public void SetAndStartNewConvo(PassangerDialogue convoData)
    {
        currConvoData = convoData;
        currStage = ConvoStage.Enter;
        currLineNumber = 0;
        //StartConvoStage(ConvoStage.Enter);
    }

    public void StartConvoStage(ConvoStage stage)
    {
        currLineNumber = 0;
        dlogDisplay.OpenDialogueBox();
        dlogDisplay.SetSpeakerPortrait(currConvoData.characterPortraitArt);
        dlogDisplay.SetNameTagText(currConvoData.passangerName);

        switch (stage)
        {
            case ConvoStage.Enter:
                currStage = ConvoStage.Enter;
                dlogDisplay.TypeNewDlogLine(currConvoData.enterDialogue[currLineNumber]);
                break;
            case ConvoStage.Main:
                currStage = ConvoStage.Main;
                dlogDisplay.TypeNewDlogLine(currConvoData.mainDialogue[currLineNumber]);
                break;
            case ConvoStage.Exit:
                currStage = ConvoStage.Exit;
                dlogDisplay.TypeNewDlogLine(currConvoData.exitDialogue[currLineNumber]);
                break;
            case ConvoStage.MissedStop:
                currStage = ConvoStage.MissedStop;
                dlogDisplay.TypeNewDlogLine(currConvoData.missedStopDialogue[currLineNumber]);
                break;
            case ConvoStage.NextStop:
                currStage = ConvoStage.NextStop;
                dlogDisplay.TypeNewDlogLine(currConvoData.nextStopDialogue[currLineNumber]);
                break;
        }
    }

    public string[] GetLinesFromStage(ConvoStage stage)
    {
        string[] selectedLineArray;

        if (stage == ConvoStage.Enter)
        {
            selectedLineArray = currConvoData.enterDialogue;
        }
        else if (stage == ConvoStage.Main)
        {
            selectedLineArray = currConvoData.mainDialogue;
        }
        else if (stage == ConvoStage.Exit)
        {
            selectedLineArray = currConvoData.exitDialogue;
        }
        else if (stage == ConvoStage.MissedStop)
        {
            selectedLineArray = currConvoData.missedStopDialogue;
        }
        else if (stage == ConvoStage.NextStop)
        {
            selectedLineArray = currConvoData.nextStopDialogue;
        }
        else
        {
            selectedLineArray = new string[0];
        }

        return selectedLineArray;
    }

    public bool GetOpen()
    {
        return dlogDisplay.getIsDialogueBoxOpen();
    }
}
