using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBall : MonoBehaviour
{
    protected Transform Player;
    //转向时间计时器
    protected float timer;
    //存活时间计时器
    protected float LiveTimer;
    protected float Speed=2;
    protected float Hurt;
    // Start is called before the first frame update
    void Start()
    {
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //碰到地面销毁，如果销毁游戏难度太低
        //if (collision.tag == "Ground")
        //{
        //    Destroy(gameObject);
        //}
    }
    public void SetData(float h,float s)
    {
        Hurt = h;
        Speed = s;
    }
    public float GetHurt()
    {
        return Hurt;
    }
    public void SetPlayer(Transform t)
    {
        Player = t;
    }
}
