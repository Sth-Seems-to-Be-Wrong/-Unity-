using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalTest : MonoBehaviour
{
    private void Awake()
    {

        Application.targetFrameRate = -1;
        if (GameObject.FindGameObjectsWithTag("Global").Length > 1)
        {
            Destroy(gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject);
        }
    }

}
