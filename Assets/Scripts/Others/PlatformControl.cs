using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformControl : MonoBehaviour
{
    // Start is called before the first frame update

    private PlatformEffector2D platform;
    void Start()
    {
        platform = GetComponent<PlatformEffector2D>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetAxis("Vertical") >= 0)
        {
            platform.rotationalOffset = 0;
        }
        else
        {
            platform.rotationalOffset = 180;
        }
    }
}
