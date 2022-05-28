using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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

    public byte[] bytes;
    string fileName;
    public GameObject[] ImageHolder = new GameObject[1];

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

        bytes = photoCapture.EncodeToPNG();
        File.WriteAllBytes(Application.dataPath + "/screenshot.png", bytes);
    }

    public void ShowPhoto()
    {
        //Sprite photoSprite = Sprite.Create(photoCapture,
        //    new Rect(0.0f, 0.0f, photoCapture.width, photoCapture.height),
        //    new Vector2(0.5f, 0.5f), 100.0f);
        //photoDisplay.sprite = photoSprite;
        //photoFrame.SetActive(true);

        var imagesToLoad = Directory.GetFiles(Application.dataPath + "/screenshot.png");
        for (int i = 0; i < imagesToLoad.Length; i++)
        {
            photoCapture = new Texture2D(100, 100); 
            fileName = imagesToLoad[i];
            bytes = File.ReadAllBytes(fileName);
            photoCapture.LoadImage(bytes);
            photoCapture.name = fileName;
            ImageHolder[i].GetComponent<RawImage>().texture = photoCapture;
        }
    }
    private void RemovePhoto()
    {
        viewingPhoto = false;
        photoFrame.SetActive(false);
    }
}


