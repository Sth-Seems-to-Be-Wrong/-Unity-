using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMusicControl : MusicVolueControl
{
    public AudioClip shoot;
    public AudioClip attack;
    public AudioClip jump;
    public AudioClip dash;
    public AudioClip behurt;

    public void Attack()
    {
        AudioControl.PlayOneShot(attack);
    }
    public void Shoot()
    {
        AudioControl.PlayOneShot(shoot);
    }
    public void Jump()
    {
        AudioControl.PlayOneShot(jump);
    }
    public void Dash()
    {
        AudioControl.PlayOneShot(dash);
    }
    
    public void BeHurt()
    {
        AudioControl.PlayOneShot(behurt);
    }
}
