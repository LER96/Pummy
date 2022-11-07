using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class Sanity : MonoBehaviour
{
    [Header("Sanity Dist")]
    FieldOfView fow;
    [SerializeField] Transform enemy;

    [SerializeField] PostProcessVolume posVol;
    [SerializeField] Vignette v;

    private bool look;
    float stateSane;

    // Start is called before the first frame update
    void Start()
    {
        posVol.profile.TryGetSettings(out v);
        fow = GetComponent<FieldOfView>();
    }

    // Update is called once per frame
    void Update()
    {
        
        float far = Vector3.Distance(transform.position, enemy.position);
        stateSane = 10 / far;
        if (fow.doesSee==false)
        {
            if (v.intensity.value < stateSane)
                v.intensity.value += Time.deltaTime;
            else
                v.intensity.value = stateSane;
        }
        else
        {
            if (v.intensity.value > 0)
            {
                v.intensity.value -= Time.deltaTime;
            }
            else
                v.intensity.value = 0;
        }
    }
}
