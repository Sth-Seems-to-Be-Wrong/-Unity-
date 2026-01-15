using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopControl : MonoBehaviour
{
    //存放要销售的物品
    List<Item> ShopItems;
    //最多只能有20件物品，同时100元刷新一次
    int maxCount = 20;
    public GameObject Good;
    int FlashCost = 100;
    void Start()
    {
        ShopItems = new List<Item>();
        InitShop();
        //同时绘制出所有的内容
        //Good = Resources.Load<GameObject>("prefab/Item/ShopItem");
        //Debug.Log(Good.name);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitShop()
    {
        //刷新时也会调用该函数
        //首先需要获取当前的关卡数目，从而获取可能刷出来的物资
        ShopItems.Clear();
        //同时应该清空当前子物体
        foreach (Transform item in transform)
        {
            Destroy(item.gameObject);
        }


        int maxMap = GameObject.FindWithTag("Others").GetComponent<InitAllitems>().GetPlayer().GetMapCount();
        for (int i = 0; i < maxCount; i++)
        {
            Item tmp = null;
            while (tmp == null)
            {
                //随机掉落这里是必定掉落，掉落的内容是该物品的当前所有可能
                tmp = InitAllitems.EnemyDieDrop((int)Random.Range(1, maxMap + 1));
                //同时创建出来超市预制体（超市预制体中包含点击购买+确定按钮，以及卖出后状态变化）
                //如果购买成功，首先会打上标记，从此失去点击功能，而不需要再商店中控制
                //只需要控制物体本身即可，
            }
            GameObject t = Instantiate(Good, transform);
            t.GetComponent<ShopItemControl>().SetIDetails(tmp.ID, 1, Item.GetItemImage(tmp));
            ShopItems.Add(tmp);
        }
    }

    //删除放在子物体上，不需要用父物体控制，仅仅-1即可
    //这里应该控制物体上绘制出（已售出），同时背包里这个物体会消失或者打上标记
    //这里采用打上标记的方式
    //增加背包里面物品,购买结束
        

    public  void flashShop()
    {
        if (CoinControl.CostCoins(FlashCost))
        {
            InitShop();
        }
        else
        {
            //显示：金币不足
            //获取通知栏
            GameObject NoticeBox = GameObject.FindWithTag("Others").transform.GetChild(5).gameObject;
            NoticeBox.transform.GetChild(0).GetComponent<Text>().text = "金币不足，请获取更多！";
            NoticeBox.GetComponent<NoticeControl>().Show();
        }
    }
}
