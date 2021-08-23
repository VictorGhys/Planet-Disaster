﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    public static readonly DialogueManager instance = new DialogueManager();
    private Queue<string> sentences;

    public void Start()
    {
        sentences = new Queue<string>();
    }

    // Start is called before the first frame update
    public void StartDialogue(Dialogue dialogue)
    {
        Debug.Log("start conversation");
        //update ui
    }
}