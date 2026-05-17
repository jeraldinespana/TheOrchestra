using UnityEngine;
using System.Collections;
using System;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
    public GameObject dialoguePanel;
    public TextMeshProUGUI speakerNameText;
    public TextMeshProUGUI dialogueText;
    public GameObject nextButton;

    public float typeSpeed = 0.05f;
    private DialogueLine[] currentLines;
    private int currentLineIndex;
    private bool isTyping;
    private Coroutine typingcoro;
    private Action onDialogueEnd;

    void Awake () { Instance = this; }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dialoguePanel.SetActive(false);  
    }

    public void StartDialogue(DialogueLine[] lines, Action onEnd = null)
    {
        Debug.Log("StartCalled , onend" + (onEnd == null));
        currentLines = lines;
        currentLineIndex = 0;
        onDialogueEnd = onEnd;
        dialoguePanel.SetActive(true);
        nextButton.SetActive(false);
        ShowLine(currentLineIndex);
    }

    public void ShowLine(int index)
    {
        speakerNameText.text = currentLines[index].speakerName;
        if (typingcoro != null) StopCoroutine(typingcoro);
        typingcoro = StartCoroutine(TypeLine(currentLines[index].text));
    }

    IEnumerator TypeLine(string line)
    {
        isTyping = true;
        nextButton.SetActive(false);
        dialogueText.text = "";

        foreach (char c in line)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }
        
        isTyping = false;
        nextButton.SetActive(true);
    }

    public void NextButtonClicked()
    {
        if (isTyping)
        {
            StopCoroutine(typingcoro);
            dialogueText.text = currentLines[currentLineIndex].text;
            isTyping = false;
            nextButton.SetActive(true);
            return;
        }
        currentLineIndex++;
        if (currentLineIndex < currentLines.Length)
            ShowLine(currentLineIndex);
        else
            EndDialogue();
    }

    void EndDialogue()
    {
        Debug.Log("ending Called" + (onDialogueEnd == null));
        dialoguePanel.SetActive(false);
        Action callback = onDialogueEnd;
        onDialogueEnd = null;
        callback?.Invoke();
    }
}
