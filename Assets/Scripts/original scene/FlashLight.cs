using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlashLight : MonoBehaviour
{
    [SerializeField]
    GameObject flashLight;
    public GameObject phoneScreen;
    private bool isFlashlightOn;

    public void Start()
    {
        flashLight.SetActive(false);
        phoneScreen.SetActive(true);
    }

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (isFlashlightOn == false)
            {
                phoneScreen.SetActive(false);
                flashLight.SetActive(true);
                isFlashlightOn = true;

            }
            else
            {
                phoneScreen.SetActive(true);
                flashLight.SetActive(false);
                isFlashlightOn = false;
            }
        }
    }
}
