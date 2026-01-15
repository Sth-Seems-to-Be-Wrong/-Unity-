using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SunkHp : BaseSkillControl
{
    public override void UseSkill()
    {
        if (Player == null) Player = GameObject.FindWithTag("Player").transform;
        PlayerContorl pc = Player.GetComponent<PlayerContorl>();
        if (IsOK() && pc.SkillsIsOk(costMp))
        {
            //ÊÍ·Å¼¼ÄÜ
            StartCoroutine(pc.StartSkill(BuffTime,BuffScale,Skill.SunkHp));
            EndUse();
        }
    }
}
