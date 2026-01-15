using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MusicVolueControl : MonoBehaviour
{
    protected AudioSource AudioControl;
    public Slider slider;
    // Start is called before the first frame update
    protected void Start()
    {
        AudioControl = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetVolue()
    {
        AudioControl.volume = slider.value;
    }
}
