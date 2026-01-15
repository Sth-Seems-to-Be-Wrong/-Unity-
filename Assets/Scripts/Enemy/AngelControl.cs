using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngelControl : EnemyControl
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        FireBall = Resources.Load<GameObject>("prefab/ChaseFireBall");
        //远程怪物
        EnemyType = 2;
        AttackRange = 10;
        WakeRange = 20;
        Speed = 0.6f;
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
                        //创建一个火球，或者近身攻击
                        GameObject g = Instantiate(FireBall, transform.position, Quaternion.identity);
                        g.GetComponent<EnemyBall>().SetData(bowhurt, 8*Speed);
                        g.GetComponent<EnemyBall>().SetPlayer(player.transform);
                    }
                }
                else
                {
                    
                    transform.Translate(Vector3.Normalize(player.transform.position - transform.position)* 13 * Time.deltaTime * Speed);
                }
            }
        }
    }
}
