using UnityEngine;
using System;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public Camera mainCamera;
    public Character conductor;
    public MiniGame miniGame;
    public DialogueLine[] failed1;
    public DialogueLine[] failed2;
    public DialogueLine[] pass;
    public DialogueLine[] completeFail;
    public DialogueLine[] auditionLines1;
    public DialogueLine[] auditionLines2;
    public DialogueLine[] auditionLines3;
    public DialogueLine[] finalWin;
    public DialogueLine[] finalLoss;
    public GameObject EndScreen;
    public GameObject dialoguePanel;
    public int auditionAttempts;
    public CanvasGroup endScreenGroupWin;
    public CanvasGroup endScreenGroupLose;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void goToRoom(float x)
    {
        Vector3 target = new Vector3(x, mainCamera.transform.position.y, -10f);
        CameraMovement.instance.SlideTo(target);
    }

    public void GoToWaitingRoom()
    {
        Vector3 target = new Vector3(RoomPos.instance.waitingRoomX, mainCamera.transform.position.y, -10f);
        CameraMovement.instance.SlideTo(target, () =>
        {
            AudioManager.Instance.PlayWaiting();
            WaitingRoomManager.Instance.StartWaitingRoom();
        });
    }
    
    public void GoToAuditorium()
    {
        Vector3 target = new Vector3(RoomPos.instance.auditoriumRoomX, mainCamera.transform.position.y, -10f);
        CameraMovement.instance.SlideTo(target, () =>
        {
            AudioManager.Instance.PlayAud();
            DialogueLine[] auditionLines = GetAuditionLines();
            conductor.Talk(auditionLines, () =>
            {
                GoToMinigame();
            });
        });
    }

    DialogueLine[] GetAuditionLines()
    {
        if (auditionAttempts == 0)
        {
            return auditionLines1;
        }
        else if (auditionAttempts == 1)
        {
            return auditionLines2;
        }
        else
        {
            return auditionLines3;
        }
    }

    public void GoToMinigame()
    {
        Vector3 target = new Vector3(RoomPos.instance.minigameX, mainCamera.transform.position.y, -10f);
        CameraMovement.instance.SlideTo(target, () =>
        {
            miniGame.StartMiniGame();
        });
    }

    public void FinishAudition(bool passed)
    {
        auditionAttempts++;
        if (auditionAttempts < 3)
        {
            StartFail();
        }
        else
        {
            if (passed)
            {
                StartPass();
            }
            else
            {
                StartFail();
            }
        }
    }

    public void StartFail()
    {
        Debug.Log(auditionAttempts);
        if (auditionAttempts == 1)
        {
            CameraMovement.instance.SlideTo(new Vector3(RoomPos.instance.auditoriumRoomX, mainCamera.transform.position.y, -10), () =>
            {
                conductor.Talk(failed1, () =>
                {
                    GoToWaitingRoom();
                });
            });
        }
        else if (auditionAttempts == 2)
        {
            CameraMovement.instance.SlideTo(new Vector3(RoomPos.instance.auditoriumRoomX, mainCamera.transform.position.y, -10), () =>
            {
                conductor.Talk(failed2, () =>
                {
                    GoToWaitingRoom();
                });
            });
        }
        else
        {
            CameraMovement.instance.SlideTo(new Vector3(RoomPos.instance.auditoriumRoomX, mainCamera.transform.position.y, -10), () =>
                {
                    conductor.Talk(completeFail, () =>
                    {
                        StartCoroutine(FadeToEndLose());

                    });
                });
        }
    }

    public void StartPass()
    {
        CameraMovement.instance.SlideTo(new Vector3(RoomPos.instance.auditoriumRoomX, mainCamera.transform.position.y, -10), () =>
        {
            conductor.Talk(pass, () =>
            {
                
                StartCoroutine(FadeToEndWin());
                
            });
        });
    }

    IEnumerator FadeToEndWin()
    {
        AudioManager.Instance.musicSource.Stop();
        endScreenGroupWin.gameObject.SetActive(true);
        endScreenGroupWin.alpha = 0f;
        float timeLength = 2f;
        float passed = 0f;
        while (passed < timeLength)
        {
            passed += Time.deltaTime;
            endScreenGroupWin.alpha = passed / timeLength;
            yield return null;
        }
        endScreenGroupWin.alpha = 1f;
        yield return new WaitForSeconds(0.5f);
        dialoguePanel.gameObject.SetActive(true);
        DialogueManager.Instance.StartDialogue(finalWin, null);
    }

    IEnumerator FadeToEndLose()
    {
        AudioManager.Instance.musicSource.Stop();
        endScreenGroupLose.gameObject.SetActive(true);
        endScreenGroupLose.alpha = 0f;
        float timeLength = 2f;
        float passed = 0f;
        while (passed < timeLength)
        {
            passed += Time.deltaTime;
            endScreenGroupLose.alpha = passed / timeLength;
            yield return null;
        }
        endScreenGroupLose.alpha = 1f;
        yield return new WaitForSeconds(0.5f);
        dialoguePanel.gameObject.SetActive(true);
        DialogueManager.Instance.StartDialogue(finalLoss, null);
    }
}

