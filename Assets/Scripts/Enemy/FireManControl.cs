using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//本类也适用于蜘蛛、粘液怪
public class FireManControl : EnemyControl
{
    
    new void Start()
    {
        base.Start();
        //火男依靠碰撞攻击别人
        EnemyType = 1;
        FireBall = null;
        AttackRange = 0;
        WakeRange = 15;
        Speed = 0.6f;
        LeftScaleX = transform.localScale.x;
    }

    void Update()
    {
        if (IsLive)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < WakeRange)
            {
                an.SetBool("isRun",true);
                MoveTimer += Time.deltaTime;
                if(MoveTimer > 1f)
                {
                    MoveTimer = 0;
                    //调整一次方向
                    if (player.transform.position.x > transform.position.x)
                        transform.localScale = new Vector3(-LeftScaleX,transform.localScale.y,transform.localScale.z);
                    else
                        transform.localScale = new Vector3(LeftScaleX, transform.localScale.y, transform.localScale.z);
                }
                //只会水平方向移动
                transform.Translate(transform.localScale.x * Vector3.left * Time.deltaTime * Speed);
            }
        }
    }


}
