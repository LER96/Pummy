using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndOfAnimationTrigger : MonoBehaviour
{
   [SerializeField] GameObject light;

   public void AnimationEnded()
    {
        light.SetActive(false);
    }
}
