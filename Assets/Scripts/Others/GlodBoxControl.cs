using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GlodBoxControl : MonoBehaviour
{
    private float timer = 0;
    private float MaxTime = 0.5f;
    public bool success = false;
    private Animator an;
    private int MapCount;
    private int DropCount;
    public GameObject DropItem;
    private ShowTime showtime;
    public bool IsOpen=false;

    void Start()
    {
        success = false;
        IsOpen = false;
        DropCount = 2;
        //测试默认质量为1
        char[] tmp = SceneManager.GetActiveScene().name.ToCharArray();
        //根据场景名字自动获取场景序号
        MapCount = tmp[tmp.Length-1]- '0';
        Debug.Log("NowMap: " + MapCount.ToString());
        an = GetComponent<Animator>();
        showtime = GameObject.FindGameObjectWithTag("Others").transform.Find("ShowTime").GetComponent<ShowTime>();
        //根据当前关卡给怪物设置等级
        GameObject[] t = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var item in t)
        {
           item.GetComponent<EnemyControl>().SetQuility(MapCount);
        }
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        if (timer > MaxTime)
        {
            timer = 0;
            GameObject[] t = GameObject.FindGameObjectsWithTag("Enemy");
            if (t.Length == 0)
            {
                success = true;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (success&&collision.tag=="Player"&&!IsOpen)
        {
            IsOpen = true;
            an.SetTrigger("Open");
            //同时会掉落物品，这里默认2个物品，此外还会给5个标准怪物的金币
            for (int i = 0; i < DropCount; i++)
            {
                Debug.Log("掉落");
                Item tmp = null;
                while (tmp == null)
                {
                    //随机掉落这里是必定掉落
                    tmp = InitAllitems.EnemyDieDrop(MapCount);
                }
                GameObject t =  Instantiate(DropItem, transform.position+Vector3.left*(i+1)*1.5f, Quaternion.identity);
                t.GetComponent<ItemData>().SetDetaild(tmp, 1);
            }

            //设置玩家当前的关卡数目,更新关卡
            if (MapCount < 5)
            {
                collision.GetComponent<PlayerContorl>().NextMap(MapCount);
            }

            //5秒钟后跳转回主菜单，同时在ui上提示
            showtime.SetTime(5);
            Invoke("ReturnMenu1", 3);
            Invoke("ReturnMenu2", 5);
            Debug.Log("掉落结束");
        }
    }

    public void ReturnMenu2()
    {
        GameObject.FindWithTag("Others").GetComponent<InitAllitems>().EnterGame();
    }

    public void ReturnMenu1()
    {
        GameObject.FindWithTag("Player").GetComponent<PlayerContorl>().MustUseSword();
    }

    public void SetMap(int nowMap)
    {
        MapCount = nowMap+1;
    }
}
