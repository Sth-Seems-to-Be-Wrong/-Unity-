using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoreFreq : BaseSkillControl
{
    public override void UseSkill()
    {
        if(Player==null)  Player = GameObject.FindWithTag("Player").transform;
        PlayerContorl pc = Player.GetComponent<PlayerContorl>();
        if (IsOK() && pc.SkillsIsOk(costMp))
        {
            //技能频率增加
            StartCoroutine(pc.StartSkill(BuffTime, BuffScale, Skill.MoreFreq));
            EndUse();
        }
    }
}
