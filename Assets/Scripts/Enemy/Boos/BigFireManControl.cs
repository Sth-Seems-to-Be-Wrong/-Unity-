using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigFireManControl : EnemyControl
{
    //大火男会产生子嗣
    private GameObject son;
    private float skillTimer;
    new void Start()
    {
        base.Start();
        EnemyType = 3;
        son = Resources.Load<GameObject>("prefab/Enemy/Fireman");
        AttackRange = 0;
        WakeRange = 15;
        Speed = 0.5f;
        skillTimer = 0;
        LeftScaleX = transform.localScale.x;
    }


    void Update()
    {
        if (IsLive)
        {
            if (Vector3.Distance(transform.position, player.transform.position) < WakeRange)
            {
                an.SetBool("isRun", true);
                MoveTimer += Time.deltaTime;
                if (MoveTimer > 0.3f)
                {
                    MoveTimer = 0;
                    //调整一次方向
                    if (player.transform.position.x > transform.position.x)
                        transform.localScale = new Vector3(-LeftScaleX, transform.localScale.y, transform.localScale.z);
                    else
                        transform.localScale = new Vector3(LeftScaleX, transform.localScale.y, transform.localScale.z);
                }
                //只会水平方向移动
                transform.Translate(transform.localScale.x * Vector3.left * Time.deltaTime * Speed);
                skillTimer += Time.deltaTime;

                //每隔8s出一只火男
                if (skillTimer >= 8)
                {
                    skillTimer = 0;
                    Instantiate(son, transform.position, Quaternion.identity);
                }
            }
        }
    }


}
