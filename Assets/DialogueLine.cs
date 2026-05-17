using UnityEngine;
using System;

[Serializable]
public class DialogueLine
{
    public string speakerName;
    [TextArea(2, 4)]
    public string text;
}
