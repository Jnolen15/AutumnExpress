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
        MissedStop
    }
    
    // Start is called before the first frame update
    void Start()
    {
        currConvoData = testPassengerDialogue;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse1) && testMode)
        {
            StartConvoStage(ConvoStage.Enter);
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
        StartConvoStage(ConvoStage.Enter);
    }

    public void StartConvoStage(ConvoStage stage)
    {
        currLineNumber = 0;
        dlogDisplay.OpenDialogueBox();
        dlogDisplay.SetSpeakerPortrait(currConvoData.characterPortraitArt);

        if (stage == ConvoStage.Enter)
        {
            currStage = ConvoStage.Enter;
            dlogDisplay.TypeNewDlogLine(currConvoData.enterDialogue[currLineNumber]);
        }
        else if (stage == ConvoStage.Main)
        {

        }
        else if (stage == ConvoStage.Exit)
        {

        }
        else if (stage == ConvoStage.MissedStop)
        {

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
        else
        {
            selectedLineArray = new string[0];
        }

        return selectedLineArray;
    }
}
