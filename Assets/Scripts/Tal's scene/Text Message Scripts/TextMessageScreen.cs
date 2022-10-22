using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextMessageScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI messageText;

    [SerializeField] TextMeshProUGUI responeMessageText;

    public void UpdateTextUI()
    {
        messageText.text = "hey how are you";
        responeMessageText.text = "im good thank you for asking";
    }
}
