using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Text Message", menuName = "Text Message")]
public class TextMessageText : ScriptableObject
{
    [SerializeField] private string textMessage;

    public string MessageText
    {
        get
        {
            return textMessage;
        }
    }
}
