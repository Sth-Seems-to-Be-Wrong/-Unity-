using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoreArrow : BaseSkillControl
{
    public override void UseSkill()
    {
        if (Player == null) Player = GameObject.FindWithTag("Player").transform;
        PlayerContorl pc = Player.GetComponent<PlayerContorl>();
        if (IsOK() && pc.SkillsIsOk(costMp))
        {
            StartCoroutine(pc.StartSkill(BuffTime, BuffScale, Skill.MoreArrow));
            EndUse();
        }
    }
}
