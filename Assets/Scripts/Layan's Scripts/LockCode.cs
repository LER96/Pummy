using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LockCode : MonoBehaviour
{
    [SerializeField] string password;
    [SerializeField] Canvas lockerCanvas;
    [SerializeField] TMP_Text _lockerCode;
    string codeText = "";

    private void Start()
    {
        if (lockerCanvas.enabled)
        {
            Cursor.lockState = CursorLockMode.Confined;
        }
    }

    private void Update()
    {
        _lockerCode.text = codeText;

        if (codeText == password)
        {
            lockerCanvas.enabled = false;
        }

        else if (codeText.Length >= 4)
        {
            codeText = "";
        }
    }

    public void ChosenNumber(int digit)
    {
        codeText += digit;
    }

    public void CloseLocker()
    {
        lockerCanvas.enabled = false;
    }
}
