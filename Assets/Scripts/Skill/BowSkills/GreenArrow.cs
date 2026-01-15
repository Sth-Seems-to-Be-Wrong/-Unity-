using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreenArrow : BaseSkillControl
{

    public override void UseSkill()
    {
        if (Player == null) Player = GameObject.FindWithTag("Player").transform;
        PlayerContorl pc = Player.GetComponent<PlayerContorl>();
        if (IsOK() && pc.SkillsIsOk(costMp))
        {
            //释放绿箭，伤害为等级*15
            Player.Find("body/Weapon/Bow").GetComponent<BowControl>().attackBigArrow(pc.isLeft(),pc.GetLevel()*15);
            EndUse();
        }
    }
}
