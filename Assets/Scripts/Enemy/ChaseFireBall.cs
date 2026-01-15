using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChaseFireBall : EnemyBall
{
    //其他方法和属性在父类中设置，这里只重写update
    void Start()
    {
        timer = 10;
    }

    // Update is called once per frame
    void Update()
    {
        LiveTimer += Time.deltaTime;
        timer += Time.deltaTime;
        if (LiveTimer >5f)
        {
            Destroy(gameObject);
        }
        if (timer > 1.2f)
        {
            Vector2 t = Player.position - transform.position;
            //transform.rotation = Quaternion.Euler(0,0,Mathf.Atan2(t.x,t.y)*Mathf.Rad2Deg);
            transform.right = t;
            timer = 0;
        }
        transform.Translate(Vector3.right * Time.deltaTime * Speed);
    }
}
