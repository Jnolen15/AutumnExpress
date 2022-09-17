using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Passanger Dialogue", menuName = "Passanger Dialogue")]
public class PassangerDialogue : ScriptableObject
{
    public DialogueDisplayController.Speakers passangerName;
    public Sprite characterPortraitArt;
    [TextArea]
    public string[] enterDialogue;
    [TextArea]
    public string[] mainDialogue; // only one string, needs to be more
    [TextArea]
    public string[] exitDialogue;
    [TextArea]
    public string[] missedStopDialogue;
}
