using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordWind : BaseSkillControl
{
    private GameObject wind;
    //需要玩家位置释放龙卷风,这里所有技能都需要玩家信息
    //比如技能伤害随着玩家等级上升而升高

    public override void UseSkill()
    {
        if (Player == null) Player = GameObject.FindWithTag("Player").transform;
        PlayerContorl pc = Player.GetComponent<PlayerContorl>();
        if (IsOK()&&pc.SkillsIsOk(costMp))
        {
            windControl wc =  Instantiate(wind,Player.Find("feet").position,Quaternion.identity).GetComponent<windControl>();
            //龙卷风伤害为等级*20
            wc.SetInfo(pc.isLeft(), pc.GetLevel() * 20);
            EndUse();
        }
    }

    void Start()
    {
        wind = Resources.Load<GameObject>("prefab/Skill/wind");
    }
    
}
