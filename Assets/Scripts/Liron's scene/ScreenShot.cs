using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class ScreenShot : MonoBehaviour
{
    [SerializeField] RenderTexture PhoneTexture;

    private Texture2D photoCapture;
    public Camera phoneCamera;
    PhotoJson  photoHolder;
    string picName;
    string json;
    //int i;
    //private bool viewingPhoto = true;

    public void Start()
    {
        //i = 0;
        photoHolder = new PhotoJson();
        photoCapture = new Texture2D(PhoneTexture.width, PhoneTexture.height);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
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

        photoCapture.ReadPixels(new Rect(0, 0, PhoneTexture.width, PhoneTexture.height), 0, 0);
        photoCapture.Apply();

        RenderTexture.active = prevRenderTexture;



        photoHolder.img = photoCapture;
        json = JsonUtility.ToJson(photoHolder);
        Debug.Log(json);





        //saves to png
        //byte[] bytes = photoCapture.EncodeToPNG();

        //File.WriteAllBytes(Application.dataPath + "/Screenshot.png", bytes);
    }

    private class PhotoJson
    {
        public Texture2D img;
    }
}
