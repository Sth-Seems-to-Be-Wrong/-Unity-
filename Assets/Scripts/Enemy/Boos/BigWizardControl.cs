using System.Collections;
using System.Collections.Generic;
using UnityEngine;

class BigWizardControl : EnemyControl
{
    private Transform FirePoint;
    //大巫师仅仅是属性高+远程攻击多
    new void Start()
    {
        base.Start();
        EnemyType = 3;
        FireBall = Resources.Load<GameObject>("prefab/FireBall");
        AttackRange = 15;
        WakeRange = 20;
        Speed = 0.5f;
        LeftScaleX = transform.localScale.x;
        FirePoint = transform.GetChild(0);
    }


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
                    if (AttackTimer > 2f)
                    {
                        an.SetTrigger("Attack");
                        AttackTimer = 0;
                        Invoke("Attack", 0.4f);
                        Invoke("Attack", 0.8f);
                        Invoke("Attack", 1.2f);
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
        g.GetComponent<FireBall>().SetDirection(LeftScaleX > 0);
    }
}

