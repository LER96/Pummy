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
        Cursor.lockState = CursorLockMode.Confined;
    }


    public void FirstRespone(TextMessageText text)
    {
        responeMessageText.text = text.MessageText;
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void SecondRespone(TextMessageText text)
    {
        responeMessageText.text = text.MessageText;
        Cursor.lockState = CursorLockMode.Locked;
    }
}


