using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class Dialogue
{
    public string name;
    public bool pickRandom = false;
    public int functionAtSentence = 0;
    public UnityEvent function;

    [TextArea(3, 10)]
    public string[] sentences;
}