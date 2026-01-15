using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//鬼怪类其实和火男差不多，唯一区别在于他是可以穿过障碍物，并且没有移动方向限制
public class GhostControl : EnemyControl
{
    // Start is called before the first frame update
    new void Start()
    {
        base.Start();
        //依靠碰撞攻击别人
        EnemyType = 1;
        FireBall = null;
        AttackRange = 0;
        WakeRange = 15;
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
               transform.Translate(Vector3.Normalize(player.transform.position - transform.position)*11 * Time.deltaTime * Speed);
            }
        }
    }
}
