using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordSkillControl : SkillsControl
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        //三个技能，分别是攻击红态、吸血、飓风，如果技能能够升级这里另算
        Skills[0].GetComponent<BaseSkillControl>().SetInfo(5, 1.8f, 10, 8);
        //吸血效率为30%
        Skills[1].GetComponent<BaseSkillControl>().SetInfo(5, 0.3f, 15, 8);
        Skills[2].GetComponent<BaseSkillControl>().SetInfo(0, 0, 20, 10);
    }
}
