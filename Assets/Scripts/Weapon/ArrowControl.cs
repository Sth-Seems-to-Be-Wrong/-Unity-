using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowControl : MonoBehaviour
{
    public float speed = 5;
    private float timer = 0;
    private float hurt;
    //判断是不是技能，是技能碰撞不销毁
    private bool isSkill = false;
    void Start()
    {
        isSkill = false;
    }

    public void SetSkill()
    {
        isSkill = true;
    }
    void Update()
    {
        transform.Translate(Vector3.right * speed*Time.deltaTime);
        timer += Time.deltaTime;
        if (timer > 2)
        {
            Destroy(gameObject);
        }
    }
    public void SetHurt(float h)
    {
        hurt = h;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.tag);
        if (collision.gameObject.tag == "Enemy")
        {
            collision.GetComponent<EnemyControl>().beHurt(hurt);
            if(!isSkill)    Destroy(gameObject);
        }else if (collision.gameObject.tag == "Ground")
        {
            if (!isSkill) Destroy(gameObject);
        }
    }


}
