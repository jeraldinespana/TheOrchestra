using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.UI;

public class MiniGame : MonoBehaviour
{
    public TextMeshProUGUI sequenceDisplay;
    public TextMeshProUGUI instructionText;
    public TextMeshProUGUI timerText;
    public int sequenceLength = 6;
    public float timeLimit = 8f;
    public float reduce = 1f;
    public GameObject miniGameCanvas;

    private KeyCode[] possibleKeys = { KeyCode.A, KeyCode.C, KeyCode.E, KeyCode.B, KeyCode.D, KeyCode.G, KeyCode.F};
    private List<KeyCode> sequence = new List<KeyCode>();
    private int currentIndex = 0;
    private float timeRemaining;
    private float currentTimeLimit;
    private int roundsDone;
    private bool gameActive = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void StartMiniGame()
    {
        miniGameCanvas.SetActive(true);
        roundsDone = 0;
        currentTimeLimit = timeLimit;
        StartRound();
    }

    void StartRound()
    {
        if (currentTimeLimit <= 0)
        {
            EndMiniGame();
            return;
        }
        GenerateSequence();
        timeRemaining = currentTimeLimit;
        gameActive = true;
        currentIndex = 0;
        UpdateSequenceDisplay();
        instructionText.text = "Press the keys in order";
    }

    void GenerateSequence()
    {
        sequence.Clear();
        for (int i = 0; i < sequenceLength; i++)
        {
            sequence.Add(possibleKeys[Random.Range(1, possibleKeys.Length)]);
        }
    }

    void UpdateSequenceDisplay()
    {
        Debug.Log("updating display");
        string display = "";
        for (int i = 0; i < sequence.Count; i++)
        {
            if (i < currentIndex)
                display += "<color=green>" + sequence[i].ToString() + "</color> ";
            else if (i == currentIndex)
                display += "<color=white><b>" + sequence[i].ToString() + "</b></color> ";
            else
                display += "<color=grey>" + sequence[i].ToString() + "</color> ";
        }
        sequenceDisplay.text = display;
    }

    void Update()
    {
        if (!gameActive)
        {
            return;
        }
        timeRemaining -= Time.deltaTime;
        timerText.text = Mathf.CeilToInt(timeRemaining) + "s";

        if (timeRemaining <= 0)
        {
            StartCoroutine(TimeRanOut());
            return;
        }

        foreach (KeyCode key in possibleKeys)
        {
            if (Input.GetKeyDown(key))
                {
                if (key == sequence[currentIndex])
                {
                    currentIndex++;
                    UpdateSequenceDisplay();
                    if (currentIndex >= sequence.Count)
                    {
                        StartCoroutine(RoundComplete());
                        return;
                    }
                }
                else
                {
                    StartCoroutine(WrongKey());
                }
            }
        }
    }

    IEnumerator RoundComplete()
    {
        gameActive = false;
        roundsDone++;
        currentTimeLimit -= reduce;
        instructionText.text = "Good job, Next sequence...";
        yield return new WaitForSeconds(1f);
        if (currentTimeLimit <= 0)
        {
            EndMiniGame();
        }
        else
        {
            StartRound();
        }
    }

    IEnumerator TimeRanOut()
    {
        gameActive = false;
        currentTimeLimit -= reduce;
        instructionText.text = "Times Up";
        yield return new WaitForSeconds(1f);

        if (currentTimeLimit <= 0)
        {
            EndMiniGame();
        }
        else
        {
            StartRound();
        }
    }

    IEnumerator WrongKey()
    {
        gameActive = false;
        instructionText.text = "Wrong, start over";
        yield return new WaitForSeconds(0.8f);
        currentIndex = 0;
        UpdateSequenceDisplay();
        instructionText.text = "press the keys";
        gameActive = true;
    }

    void EndMiniGame()
    {
        gameActive = false;
        miniGameCanvas.SetActive(false);
        GameManager.instance.FinishAudition(roundsDone > 6);
    }
}
