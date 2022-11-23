using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ScreenShot : MonoBehaviour
{
    [SerializeField] RenderTexture PhoneTexture;

    public Image[] images= new Image[5];
    private Texture2D []photoCapture;
    public Camera phoneCamera;
    PhotoJson  photoHolder;
    string picName;
    string json;
    int i;
    //private bool viewingPhoto = true;

    public void Start()
    {
        i = 0;
        photoHolder = new PhotoJson();
        photoCapture = new Texture2D[5];
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartCoroutine(CapturePhoto());
        }
    }

    IEnumerator CapturePhoto()
    {
        //viewingPhoto = true;
        yield return new WaitForEndOfFrame();

        var prevRenderTexture = RenderTexture.active;
        RenderTexture.active = PhoneTexture;

        if (i < photoCapture.Length)
        {
            photoCapture[i] = new Texture2D(PhoneTexture.width, PhoneTexture.height);
            photoCapture[i].ReadPixels(new Rect(0, 0, PhoneTexture.width, PhoneTexture.height), 0, 0);
            photoCapture[i].Apply();

            //photoHolder.img = photoCapture[i];
            //json = JsonUtility.ToJson(photoHolder);
            //Debug.Log(json);


            Sprite photoSprite = Sprite.Create(photoCapture[i],
            new Rect(0.0f, 0.0f, photoCapture[i].width, photoCapture[i].height),
            new Vector2(0.5f, 0.5f), 100.0f);
            images[i].sprite = photoSprite;

            i++;
        }
        else
        {
            i = 0;
        }
        RenderTexture.active = prevRenderTexture;
        //Sprite photoSprite = Sprite.Create(photoCapture[i],
        //    new Rect(0.0f, 0.0f, photoCapture[i].width, photoCapture[i].height),
        //    new Vector2(0.5f, 0.5f), 100.0f);
        //images[i].sprite = photoSprite;

        //photoHolder.img = photoCapture[i];
        //json = JsonUtility.ToJson(photoHolder);
        //Debug.Log(json);
        //saves to png
        //byte[] bytes = photoCapture.EncodeToPNG();

        //File.WriteAllBytes(Application.dataPath + "/Screenshot.png", bytes);
    }

    private class PhotoJson
    {
        public Texture2D img;
    }
}
