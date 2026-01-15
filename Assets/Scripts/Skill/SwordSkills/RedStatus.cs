using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedStatus : BaseSkillControl
{
    public override void UseSkill()
    {
        if(Player==null)  Player = GameObject.FindWithTag("Player").transform;
        PlayerContorl pc = Player.GetComponent<PlayerContorl>();
        if (IsOK() && pc.SkillsIsOk(costMp))
        {
            // Õ∑≈∫ÏÃ¨
            StartCoroutine(pc.StartSkill(BuffTime,BuffScale,Skill.Red));
            EndUse();
        }
    }
}
