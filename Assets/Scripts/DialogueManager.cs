using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

public class DialogueManager : Singleton<DialogueManager>
{
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private TMP_Text skipButtonText;
    [SerializeField] private Animator animator;
    [SerializeField] private float timeBetweenLetterAnimation = 0.5f;
    [SerializeField] private AudioSource skipSFX;
    [SerializeField] private AudioSource blipSFX;

    private Queue<string> sentences;
    private int timesClickedOnSkip = 0;
    private int amountOfSentencesAtStart;
    private int functionAtSentence = 0;

    private UnityEvent function;

    public void Awake()
    {
        sentences = new Queue<string>();
    }

    // Start is called before the first frame update
    public void StartDialogue(Dialogue dialogue)
    {
        functionAtSentence = dialogue.functionAtSentence;
        function = dialogue.function;

        animator.SetBool("IsOpen", true);
        Time.timeScale = 0;
        if (dialogue.name != "")
        {
            nameText.text = dialogue.name;
        }
        if (sentences != null)
        {
            sentences.Clear();
        }

        if (dialogue.pickRandom)
        {
            int r = Random.Range(0, dialogue.sentences.Length);
            sentences.Enqueue(dialogue.sentences[r]);
            amountOfSentencesAtStart = 1;
        }
        else
        {
            foreach (var sentence in dialogue.sentences)
            {
                sentences.Enqueue(sentence);
            }

            amountOfSentencesAtStart = sentences.Count;
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

        if (functionAtSentence == amountOfSentencesAtStart - sentences.Count)
        {
            if (function != null)
            {
                function.Invoke();
            }
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
        blipSFX.Play();
        foreach (var letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            //yield return null;
            yield return new WaitForSecondsRealtime(timeBetweenLetterAnimation);
        }
        blipSFX.Stop();
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
                skipSFX.Play();
                break;
        }
    }
}