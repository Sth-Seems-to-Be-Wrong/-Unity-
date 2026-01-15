using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BowControl : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject arrow;
    private GameObject BigArrow;
    private Transform Point;

    public float speed = 10;
    void Start()
    {
        Point = transform.GetChild(0);
        BigArrow = Resources.Load<GameObject>("prefab/Skill/BigArrow");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void attack(bool isLeft,float hurt,int count)
    {
        //向上会产生几个
        for (int i = 0; i < (count+1)/2; i++)
        {
            GameObject shot = Instantiate(arrow, Point.position + i * Vector3.up*0.5f, Quaternion.identity);
            ArrowControl aw = shot.GetComponent<ArrowControl>();
            if (isLeft)
            {
                shot.transform.localScale = new Vector3(-shot.transform.localScale.x, shot.transform.localScale.y, shot.transform.localScale.z);
                aw.speed = -speed;
            }
            else
            {
                aw.speed = speed;
            }
            aw.SetHurt(hurt);
        }

        //向下会产生几个
        for (int i = 0; i < count/2; i++)
        {
            GameObject shot = Instantiate(arrow, Point.position - (i + 1) * Vector3.up*0.5f, Quaternion.identity);
            ArrowControl aw = shot.GetComponent<ArrowControl>();
            if (isLeft)
            {
                shot.transform.localScale = new Vector3(-shot.transform.localScale.x, shot.transform.localScale.y, shot.transform.localScale.z);
                aw.speed = -speed;
            }
            else
            {
                aw.speed = speed;
            }
            aw.SetHurt(hurt);
        }
    }

    //技能巨剑
    public void attackBigArrow(bool isLeft, float hurt)
    {
        GameObject shot = Instantiate(BigArrow, Point.position, Quaternion.identity);
        ArrowControl aw = shot.GetComponent<ArrowControl>();
        //设置为技能
        aw.SetSkill();
        if (isLeft)
        {
            //但是技能由于是光效渲染的，因此缩放不可以，只能旋转
            //shot.transform.localScale = new Vector3(-shot.transform.localScale.x, shot.transform.localScale.y, shot.transform.localScale.z);
            shot.transform.localEulerAngles = new Vector3(0, 0, 180);
        }
        //旋转之后此速度就相当于反向
        aw.speed = speed*1.5f;
        aw.SetHurt(hurt);
    }
}
