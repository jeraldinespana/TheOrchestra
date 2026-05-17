using UnityEngine;
using UnityEngine.UI;
using System;

public class TitleScreen : MonoBehaviour
{
    public GameObject titleScreenCanvas;
    public DialogueLine[] openingLines;
    public Character firstCharacter;
    public Character secondCharacter;

    public void OnClick()
    {
        titleScreenCanvas.SetActive(false);
        AudioManager.Instance.PlayWaiting();
        DialogueManager.Instance.StartDialogue(openingLines, () =>
        {
            firstCharacter.Talk(() =>
            {
                firstCharacter.gameObject.SetActive(false);
                secondCharacter.Talk(() =>
                {
                    secondCharacter.gameObject.SetActive(false);
                    GameManager.instance.GoToAuditorium(); 
                });
            });
        });

    }
}
