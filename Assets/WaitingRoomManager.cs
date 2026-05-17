using UnityEngine;
using System;

public class WaitingRoomManager : MonoBehaviour
{
    public static WaitingRoomManager Instance;
    public Character first;
    public Character second;
    public DialogueLine[] return1;
    public DialogueLine[] return2;

    void Awake ()
    {
        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void StartWaitingRoom()
    {
        int attempts = GameManager.instance.auditionAttempts;
        if (attempts == 1)
        {
            first.Talk(return1, () =>
            {
                first.gameObject.SetActive(false);
                GameManager.instance.GoToAuditorium();
            });
        }
        else if (attempts == 2) 
        {
            second.Talk(return2, () =>
            {
                second.gameObject.SetActive(false);
                GameManager.instance.GoToAuditorium();
            });
        }
    }
}
