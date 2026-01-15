using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardControl : EnemyControl
{
    // Start is called before the first frame update
    private Transform FirePoint;
    new void Start()
    {
        base.Start();
        //远程法师
        EnemyType = 2;
        FireBall = Resources.Load<GameObject>("prefab/FireBall");
        AttackRange = 12;
        WakeRange = 18;
        Speed = 0.5f;
        LeftScaleX = transform.localScale.x;
        FirePoint = transform.GetChild(0);
    }


    //不行，太饿了，要润了，今天白天玩太久了导致进度不太好，现在这个怪物做完之后，就可以粘贴复制更多怪物
    //同时，地形要加快出来，顺手解决音乐bug，在解决完毕之后要制作boos（设置好攻击范围多几个碰撞控制）
    //多找点背景音乐，要中性音乐，商城最后做ok
    // Update is called once per frame
    void Update()
    {
        if (IsLive)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < WakeRange)
            {
                MoveTimer += Time.deltaTime;
                if (MoveTimer > 1f)
                {
                    MoveTimer = 0;
                    //调整一次方向
                    if (player.transform.position.x > transform.position.x)
                        transform.localScale = new Vector3(-LeftScaleX, transform.localScale.y, transform.localScale.z);
                    else
                        transform.localScale = new Vector3(LeftScaleX, transform.localScale.y, transform.localScale.z);
                }

                AttackTimer += Time.deltaTime;
                if (Vector3.Distance(transform.position, player.transform.position) < AttackRange)
                {
                    if (AttackTimer > 1.5f)
                    {
                        an.SetTrigger("Attack");
                        AttackTimer = 0;
                        Invoke("Attack", 0.4f);
                    }
                }
                else
                {
                    //只会水平方向移动
                    transform.Translate(transform.localScale.x * Vector3.left * Time.deltaTime * Speed);
                }
            }
        }
    }

    private void Attack()
    {
        //创建一个火球，或者近身攻击
        GameObject g = Instantiate(FireBall, FirePoint.position, Quaternion.identity);
        g.GetComponent<EnemyBall>().SetData(bowhurt, 7 * Speed);
        g.GetComponent<EnemyBall>().SetPlayer(player.transform);
        g.GetComponent<FireBall>().SetDirection(transform.localScale.x > 0);
    }
}
