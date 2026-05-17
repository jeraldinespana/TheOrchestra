using UnityEngine;
using System;

public class Character : MonoBehaviour
{
    public DialogueLine[] lines;

    public void Talk(Action onDone = null)
    {
        Debug.Log("Talked on" + gameObject.name);
        gameObject.SetActive(true);
        DialogueManager.Instance.StartDialogue(lines,onDone);
    }

    public void Talk(DialogueLine[] newLines, Action onDone = null)
    {
        Debug.Log("Talk called on " + gameObject.name);
        gameObject.SetActive(true);
        DialogueManager.Instance.StartDialogue(newLines,onDone);
    }
}
