using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class TextMessageScreen : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI firstMessageText;
    [SerializeField] TextMeshProUGUI secondMessageText;

    [SerializeField] TextMeshProUGUI responeMessageText;

    public void UpdateTextUI()
    {
        firstMessageText.text = "hey how are you";
        Cursor.lockState = CursorLockMode.Confined;
    }


    public void FirstRespone(TextMessageText text)
    {
        responeMessageText.text = text.MessageText;
        secondMessageText.text = "1";
    }

    public void SecondRespone(TextMessageText text)
    {
        responeMessageText.text = text.MessageText;
        secondMessageText.text = "2";
    }
}


