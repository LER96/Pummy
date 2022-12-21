using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LockCode : MonoBehaviour
{
    [SerializeField] string password;
    [SerializeField] TMP_Text _lockerCode;
    [SerializeField] GameObject lockCamera;
    [SerializeField] GameObject door;
    public bool IsLockPressed = false;
    string codeText = "";

    private void Start()
    {
        lockCamera.SetActive(false);
    }

    private void Update()
    {
        if (IsLockPressed == true)
        {
            Cursor.lockState = CursorLockMode.Confined;
            lockCamera.SetActive(true);
        }

        _lockerCode.text = codeText;

        if (codeText == password)
        {
            lockCamera.SetActive(false);
            door.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
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
        IsLockPressed = false; 
        lockCamera.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }
}
