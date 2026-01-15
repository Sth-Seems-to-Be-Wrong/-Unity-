using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBall : EnemyBall
{

    private Vector3 v3;
    public void SetDirection(bool isleft)
    {
        v3 = isleft?Vector3.left:Vector3.right;
    }
    void Update()
    {
        LiveTimer += Time.deltaTime;
        if (LiveTimer >= 6f)
        {
            Destroy(this.gameObject);
        }
        transform.Translate(Speed * Time.deltaTime * v3);
    }
}
