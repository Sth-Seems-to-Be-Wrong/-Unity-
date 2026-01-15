using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontChange : MonoBehaviour
{
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
