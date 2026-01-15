using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowSkillControl : SkillsControl
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        //三个技能，分别是攻击频率、多箭、绿箭穿透，如果技能能够升级这里另算
        //设置攻击频率，最高也就是2，位于1~2之间
        Skills[0].GetComponent<BaseSkillControl>().SetInfo(5, 1.4f, 10, 8);
        //目前设置为两个弓箭
        Skills[1].GetComponent<BaseSkillControl>().SetInfo(5, 2, 15, 8);
        Skills[2].GetComponent<BaseSkillControl>().SetInfo(0, 0, 20, 10);
    }
}
