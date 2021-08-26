using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Wave
{
    public DialogueTrigger startDialogue;
    public int amountOfDisastersToSpawn = 10;
    public float spawnInterval = 3;
    public bool isBossBattle = false;
}