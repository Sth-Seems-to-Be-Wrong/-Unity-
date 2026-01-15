using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigAngelControl : EnemyControl
{
    //大天使也会产生子嗣
    private GameObject son;
    private float skillTimer;
    void Start()
    {
        base.Start();
        EnemyType = 3;
        son = Resources.Load<GameObject>("prefab/Enemy/angel");
        FireBall = Resources.Load<GameObject>("prefab/ChaseFireBall");
        AttackRange = 15;
        WakeRange = 25;
        Speed = 0.5f;
        skillTimer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsLive)
        {
            AttackTimer += Time.deltaTime;
            if (Vector3.Distance(transform.position, player.transform.position) < WakeRange)
            {
                if (Vector3.Distance(transform.position, player.transform.position) < AttackRange)
                {
                    if (AttackTimer > 2.5f)
                    {
                        an.SetTrigger("Attack");
                        AttackTimer = 0;
                        //大天使会创建多个火球
                        Invoke("ProFireBall", 0.2f);
                        Invoke("ProFireBall", 0.5f);
                        Invoke("ProFireBall", 0.7f);
                    }
                }
                else
                {
                    transform.Translate(Vector3.Normalize(player.transform.position - transform.position) * 12 * Time.deltaTime * Speed);
                }
                //15s会释放一个天使
                skillTimer += Time.deltaTime;
                if (skillTimer >= 15)
                {
                    skillTimer = 0;
                    Instantiate(son, transform.position, Quaternion.identity);
                }
            }
        }
    }

    public void ProFireBall()
    {
        GameObject g = Instantiate(FireBall, transform.position, Quaternion.identity);
        g.GetComponent<EnemyBall>().SetData(bowhurt, 8 * Speed);
        g.GetComponent<EnemyBall>().SetPlayer(player.transform);
    }
}
