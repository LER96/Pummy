using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvent : MonoBehaviour
{
    public static GameEvent current;

    private void Awake()
    {
        current = this;
    }


}
