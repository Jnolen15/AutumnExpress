using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueDisplayController : MonoBehaviour
{
    public bool testModeEnabled = false;

    [Header("Important References")]
    public UIReferences uiReferences;
    public Text dlogTextBox;
    public Text dlogNameTagTextBox;
    public Image dlogHeadshotBox;
    public Animator dlogAnimator;

    [Header("Timing Variables")]
    public float typingSpeed = 1f;
    public float delayBeforeNextLine = 1f;

    private bool isDialogueBoxOpen = false;
    private bool isCurrentLineFinished = false;
    private IEnumerator currDlogCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        dlogAnimator.SetBool("isDialogueOpen", false);
        dlogHeadshotBox.color = new Color(0, 0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (testModeEnabled)
        {
            if (Input.GetKeyDown(KeyCode.G))
            {
                Debug.Log("Open dialogue");
                dlogAnimator.SetBool("isDialogueOpen", true);
                TypeNewDlogLine("woah look at me I'm dialogue");

            }
            else if (Input.GetKeyDown(KeyCode.H))
            {
                Debug.Log("Typing dialogue line");
                TypeNewDlogLine("woah look at me I'm dialogue but look seriosuly you can see the animal crossing readout");
            }
            else if (Input.GetKeyDown(KeyCode.J))
            {
                Debug.Log("Close dialogue");
                dlogAnimator.SetBool("isDialogueOpen", false);
            }
            else if (Input.GetKeyDown(KeyCode.K))
            {
                Debug.Log("Skip to end of line");
                SkipToEndOfLine();
            }
        }
    }

    public void OpenDialogueBox()
    {
        dlogAnimator.SetBool("isDialogueOpen", true);
        isDialogueBoxOpen = true;
    }

    public void SetSpeakerPortrait(Sprite portrait)
    {
        dlogHeadshotBox.color = new Color(1, 1, 1, 1);
        dlogHeadshotBox.sprite = portrait;
    }

    public void SetNameTagText(string name)
    {
        dlogNameTagTextBox.text = name;
    }

    public void CloseDialogueBox()
    {
        dlogAnimator.SetBool("isDialogueOpen", false);
        isDialogueBoxOpen = false;
    }

    public void TypeNewDlogLine(string line)
    {
        // stop any coroutines currently running so we can run this one
        if (currDlogCoroutine != null)
        {
            StopCoroutine(currDlogCoroutine);
        }

        // run our coroutine
        currDlogCoroutine = TypeLineCharacters(line);
        StartCoroutine(currDlogCoroutine);
    }

    public void SkipToEndOfLine()
    {
        isCurrentLineFinished = true;
    }

    public bool getIsDialogueBoxOpen()
    {
        return isDialogueBoxOpen;
    }

    public bool getIsCurrentLineFinished()
    {
        return isCurrentLineFinished;
    }

    // --- IEnumerators

    IEnumerator TypeLineCharacters(string line)
    {
        isCurrentLineFinished = false;

        // empty the text box
        dlogTextBox.text = "";
        int charCount = 0;

        // divide the line up into individual letters
        char[] letterArray = line.ToCharArray();

        // pause and then add in letters with pauses between each addition
        yield return new WaitForSecondsRealtime(delayBeforeNextLine);
        for (int i = 0; i < line.Length; i++)
        {
            if (!isCurrentLineFinished)
            {
                // add to the textbox letter by letter
                dlogTextBox.text += letterArray[i];

                // play typing sfx
                if (0 == charCount % 2 && letterArray[i] != ' ')
                {
                    uiReferences.boop.Play();
                }
                charCount++;

                // wait before typing next character
                yield return new WaitForSecondsRealtime(typingSpeed);
            }
            else
            {
                // line is supposed to be finished, so completely type line and put loop at the end
                dlogTextBox.text = line;
                uiReferences.boop.Play();
                i = line.Length;
            }
        }

        isCurrentLineFinished = true;
    }
}
