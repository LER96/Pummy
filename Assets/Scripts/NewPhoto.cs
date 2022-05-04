using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NewPhoto : MonoBehaviour
{

    [SerializeField]
    private Image photoDisplay;
    [SerializeField]
    private GameObject photoFrame;
    [SerializeField] RenderTexture PhoneTexture;

    private Texture2D photoCapture;
    public Camera phoneCamera;
    private bool viewingPhoto = true;

    public void Start()
    {
        photoCapture = new Texture2D(PhoneTexture.width, PhoneTexture.height);
    }

    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (viewingPhoto == false)
            {
                StartCoroutine(CapturePhoto());
            }
            else
            {
                RemovePhoto();
            }
        }
    }

    IEnumerator CapturePhoto()
    {
        viewingPhoto = true;
        yield return new WaitForEndOfFrame();

        var prevRenderTexture = RenderTexture.active;
        RenderTexture.active = PhoneTexture;

        photoCapture.ReadPixels(new Rect(0, 0, PhoneTexture.width, PhoneTexture.height), 0, 0);
        photoCapture.Apply();

        RenderTexture.active = prevRenderTexture;
        ShowPhoto();
    }

    public void ShowPhoto()
    {
        Sprite photoSprite = Sprite.Create(photoCapture,
            new Rect(0.0f, 0.0f, photoCapture.width, photoCapture.height),
            new Vector2(0.5f, 0.5f), 100.0f);
        photoDisplay.sprite = photoSprite;
        photoFrame.SetActive(true);
    }

    private void RemovePhoto()
    {
        viewingPhoto = false;
        photoFrame.SetActive(false);
    }
}


