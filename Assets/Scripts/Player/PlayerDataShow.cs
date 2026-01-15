using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDataShow : MonoBehaviour
{
    public Text Name;
    public Image Hp;
    public Image Mp;
    public Text Level;
    public Image Experience;
    
    void Start()
    {
        
    }

    void Update()
    {
        
    }
    public void UpdateName(string str)
    {
        Name.text = str;
    }
    public void UpdateState(float MaxHp,float NowHp,float MaxMp,float NowMp,float MaxExperience,float NowExperience,float NowLevel)
    {
        Hp.fillAmount = NowHp / MaxHp;
        Mp.fillAmount = NowMp / MaxMp;
        Experience.fillAmount = NowExperience / MaxExperience;
        Level.text = NowLevel.ToString();
    }
}
