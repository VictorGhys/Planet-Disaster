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

    private Queue<string> sentences;

    public void Start()
    {
        sentences = new Queue<string>();
    }

    // Start is called before the first frame update
    public void StartDialogue(Dialogue dialogue)
    {
        Debug.Log("start conversation");
        nameText.text = dialogue.name;
        sentences.Clear();
        foreach (var sentence in sentences)
        {
            sentences.Enqueue(sentence);
        }

        //update ui
    }

    private void DisplayNextSentence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }

        string sentence = sentences.Dequeue();
        Debug.Log(sentence);
        dialogueText.text = sentence;
    }

    private void EndDialogue()
    {
        Debug.Log("end of conversation");
    }
}