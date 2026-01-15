using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BGMusicControl : MusicVolueControl
{
    //
    public AudioClip[] bgs;
    new void Start()
    {
        base.Start();
        AudioControl.loop = true;
        AudioControl.clip = bgs[0];
        AudioControl.Play();
    }

    public void PlayMusic(int index)
    {
        AudioControl.clip = bgs[index];
        AudioControl.Play();
        AudioControl.loop = true;
    }
}
