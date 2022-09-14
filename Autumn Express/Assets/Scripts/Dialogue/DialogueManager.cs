using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public Text dlogTextBox;
    public Image dlogBoundBox;

    [Header("Timing Variables")]
    public float typingSpeed = 1f;
    public float delayBeforeNextLine = 1f;

    private bool isCurrentLineFinished = false;

    private IEnumerator currDlogCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.G))
        {
            Debug.Log("Open dialogue");
        }
        else if (Input.GetKeyDown(KeyCode.H))
        {
            Debug.Log("Typing dialogue line");
            TypeNewDlogLine("woah look at me I'm dialogue");
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            Debug.Log("Close dialogue");
        }
    }

    // OpenDialogueBox()

    // CloseDialogueBox()

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
                if (0 == charCount % 2)
                {
                    //playerUIVals.typingSFX.Play();
                }
                charCount++;

                // wait before typing next character
                yield return new WaitForSecondsRealtime(typingSpeed);
            }
            else
            {
                // line is supposed to be finished, so completely type line and put loop at the end
                dlogTextBox.text = line;
                i = line.Length;
            }
        }

        isCurrentLineFinished = true;
    }
}
