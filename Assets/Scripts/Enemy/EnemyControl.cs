using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyControl : MonoBehaviour
{
    protected Animator an;
    protected GameObject player;
    protected float Speed;
    protected float AttackRange;
    protected float WakeRange;

    //控制怪物调节方向，和攻击的频率
    protected float MoveTimer;
    protected float AttackTimer;

    protected float Hp;
    protected bool IsLive;
    //怪物分为：小怪、精英、boos之类，爆率不同
    protected int quality;
    //追踪子弹
    protected GameObject FireBall;
    //死亡掉落
    protected GameObject DieItem;
    //用来实现被打击到的效果
    protected SpriteRenderer sr;
    protected Color color;
    //这里可以创建血条跟随

    //这里设置怪物类型：远程、近程、boos根据等级确定血量和攻击力
    //1,近 2.远 3.boos
    protected float EnemyType;

    //每个不同的怪物远程攻击和近战伤害都不同
    protected float hurt;
    protected float bowhurt;

    //对于需要转身的怪物设置缩放值
    protected float LeftScaleX;
    protected void Start()
    {
        an = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        player = GameObject.FindWithTag("Player");
        DieItem = Resources.Load<GameObject>("prefab/Item/Item");
        color = sr.color;
        IsLive = true;
        AttackTimer = 0;
        MoveTimer = 0;
    }

    void Update()
    {
       
    }

    //这里不写碰撞器，碰撞器由玩家和武器那里写，同时要求怪物必须有死亡动画、胶囊碰撞器
    public void beHurt(float hurt)
    {
        Hp -= hurt;
        sr.color = Color.red;
        Invoke("FlashColor", 0.2f);
        if (IsLive&&Hp <= 0)
        {
            IsLive = false;
            an.SetBool("isDie", true);
            Item drop = InitAllitems.EnemyDieDrop(quality);
            if (drop != null)
            {
                GameObject tmp=Instantiate(DieItem, transform.position, Quaternion.identity);
                //我们这里普通怪物只能爆出一件，boss能爆出来多件
                tmp.GetComponent<ItemData>().SetDetaild(drop,1);
            }
            //同时具有经验、金币加成
            InitAllitems.EnemyDieCoin(quality);
            player.GetComponent<PlayerContorl>().AddExperience(InitAllitems.EnemyDieExp(quality));
            Destroy(gameObject.GetComponent<CapsuleCollider2D>());
            Rigidbody2D rigid = gameObject.GetComponent<Rigidbody2D>();
            if (rigid != null) Destroy(rigid);
            Destroy(gameObject, 0.5f);
        }
    }

    public void FlashColor()
    {
        sr.color = color;
    }

    //用来返回碰撞伤害，远程伤害在火球中以及定义好了
    public float GetHurt()
    {
        //这里只统计近战伤害
        return hurt;
    }

    //设置怪物等级，并且根据等级设置血量和伤害
    public void SetQuility(int q)
    {
        quality = q;
        //Debug.Log(EnemyType);
        if(EnemyType == 1)
        {
            Hp = 100 * q;
            hurt = 20 * q;
            bowhurt = 0;
        }else if(EnemyType == 2)
        {
            Hp = 60 * q;
            hurt = 0;
            bowhurt = 15 * q;
        }else if (EnemyType == 3)
        {
            Hp = 300 * q;
            hurt = 30 * q;
            bowhurt = 20 * q;
        }
    }

    //如果这个东西掉下去碰到陷阱也会直接死亡，但是不会掉落东西捏
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Water")
        {
            IsLive = false;
            Destroy(gameObject);
        }
    }
}
