using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerDetailShow : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Level;
    public GameObject Experience;
    public GameObject Hp;
    public GameObject Mp;
    public GameObject SwordHurt;
    public GameObject BowHurt;



    public void UpdataState(float MaxHp, float NowHp, float MaxMp, float NowMp, float MaxExperience, float NowExperience, float NowLevel,float NowSwordHurt,float NowBowHurt)
    {
        Level.GetComponent<Text>().text = "等级：" + NowLevel.ToString();
        Experience.GetComponent<Text>().text = "经验：" + NowExperience.ToString()+"/"+MaxExperience.ToString();
        Hp.GetComponent<Text>().text = "血量：" + NowHp.ToString() + "/" + MaxHp.ToString();
        Mp.GetComponent<Text>().text = "蓝量：" + NowMp.ToString() + "/" + MaxMp.ToString();
        SwordHurt.GetComponent<Text>().text = "近伤：" +  NowSwordHurt.ToString();
        BowHurt.GetComponent<Text>().text = "远伤：" + NowBowHurt.ToString();
    }
}
