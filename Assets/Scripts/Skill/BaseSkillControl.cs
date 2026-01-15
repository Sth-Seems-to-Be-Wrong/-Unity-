using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//技能基类，该类使用update更新技能时间+创建预设体
//技能基类也许要调整…………
abstract public class BaseSkillControl : MonoBehaviour
{

    protected float Timer;
    protected float SkillTime;
    private Image CoolImage;

    //不赋值，执行技能的时候再获取player
    protected Transform Player;
    protected float costMp;

    //设置技能持续时间和buff叠加效果
    protected float BuffTime;
    protected float BuffScale;

    //设置为protect，子类可以继续使用
    protected void Awake()
    {
        Timer = 0;
        CoolImage = transform.GetChild(0).GetComponent<Image>();
        CoolImage.fillAmount = 0;
        Player = null;
    }

    protected void Update()
    {
        if (Timer > 0) Timer -= Time.deltaTime;
        else Timer = 0;
        //随着时间增加逐渐变白，=0是全白的，使用完毕之后会变成全黑 timer = skilltime
        CoolImage.fillAmount = Timer / SkillTime;
    }

    public bool IsOK()
    {
        return Timer == 0;
    }

    public void EndUse()
    {
        Timer = SkillTime;
    }

    public void SetInfo(float bufftime,float buffscale, float mp,float cooltime)
    {
        BuffTime = bufftime;
        BuffScale = buffscale;
        costMp = mp;
        SkillTime = cooltime;
    }

    //这个方法需要重写，定义为抽象方法，同时这个方法需要动态获取玩家（仅一次）
    abstract public void UseSkill();
}
