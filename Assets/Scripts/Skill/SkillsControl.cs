using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//下面对应了三个技能，我们按下一个技能按键就会触发选择其中一个技能
//由于是两个技能面板，每个下面有三个技能技能控制栏目目前设置为空，我们需要写一个父类来适用于所有技能
//之后每个技能有刷新时间和对应的显示，不同技能需要
public class SkillsControl : MonoBehaviour
{

    protected Transform[] Skills;
    protected void Start()
    {
        Skills = new Transform[3];
        for (int i = 0; i < 3; i++)
        {
            Skills[i] = transform.GetChild(i+1);
            BaseSkillControl bs = Skills[i].GetComponent<BaseSkillControl>();
        }
    }

    //u=0 i=1 o=2
    public void UseSkill(int index)
    {
        Skills[index].GetComponent<BaseSkillControl>().UseSkill();
    }
}
