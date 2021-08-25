using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : Singleton<DialogueManager>
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private TMP_Text skipButtonText;
    [SerializeField] private Animator animator;
    [SerializeField] private float timeBetweenLetterAnimation = 0.5f;

    private Queue<string> sentences;
    private int timesClickedOnSkip = 0;

    public void Awake()
    {
        sentences = new Queue<string>();
    }

    // Start is called before the first frame update
    public void StartDialogue(Dialogue dialogue)
    {
        animator.SetBool("IsOpen", true);
        Time.timeScale = 0;
        nameText.text = dialogue.name;
        if (sentences != null)
        {
            sentences.Clear();
        }
        foreach (var sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSentence();
    }

    public void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        //Debug.Log(sentence);
        //dialogueText.text = sentence;
        StopAllCoroutines();
        StartCoroutine(TypeSentence(sentence));
    }

    private IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (var letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            //yield return null;
            yield return new WaitForSecondsRealtime(timeBetweenLetterAnimation);
        }
    }

    private void EndDialogue()
    {
        Time.timeScale = 1;
        animator.SetBool("IsOpen", false);
        timesClickedOnSkip = 0;
        skipButtonText.text = "Skip";
    }

    public void SkipDialogue()
    {
        timesClickedOnSkip++;
        switch (timesClickedOnSkip)
        {
            case 1:
                skipButtonText.text = "Sure?";
                break;

            case 2:
                EndDialogue();
                break;
        }
    }
}