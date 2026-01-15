using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigGhostControl : EnemyControl
{
    private GameObject son;
    private float skillTimer;
    new void Start()
    {
        base.Start();
        //依靠碰撞攻击别人
        EnemyType = 3;
        FireBall = null;
        son = Resources.Load<GameObject>("prefab/Enemy/ghost");
        AttackRange = 0;
        skillTimer = 0;
        WakeRange = 20;
        Speed = 0.4f;
        LeftScaleX = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsLive)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < WakeRange)
            {
                //an.SetBool("isRun", true);
                MoveTimer += Time.deltaTime;
                if (MoveTimer > 1.5f)
                {
                    MoveTimer = 0;
                    //调整一次方向
                    if (player.transform.position.x > transform.position.x)
                        transform.localScale = new Vector3(-LeftScaleX, transform.localScale.y, transform.localScale.z);
                    else
                        transform.localScale = new Vector3(LeftScaleX, transform.localScale.y, transform.localScale.z);
                }
                transform.Translate(Vector3.Normalize(player.transform.position - transform.position)*10 * Time.deltaTime * Speed);

                //10s会释放一个鬼魂
                skillTimer += Time.deltaTime;
                if (skillTimer >= 10)
                {
                    skillTimer = 0;
                    Instantiate(son, transform.position, Quaternion.identity);
                }
            }
        }
    }
}
