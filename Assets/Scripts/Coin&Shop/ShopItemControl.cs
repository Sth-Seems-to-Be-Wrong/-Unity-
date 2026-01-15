using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class ShopItemControl : MonoBehaviour
    , IPointerEnterHandler, IPointerExitHandler
{
    //id是因为要被直接访问到public
    public int id;
    private int num;
    private Item ShopItem;
    private string ItemDescribe;
    private Sprite ItemImg;
    //操作按钮
    public GameObject Describe;
    public GameObject UsePanel;
    
    //价格标签
    public GameObject Price;

    //判断是否卖出
    private bool isSell = false;
    public GameObject Selled;

    public void Start()
    {
        Describe.SetActive(false);
        UsePanel.SetActive(false);
        Selled.SetActive(false);
    }

    public void SetIDetails(int id, int num, Sprite img)
    {
        this.id = id;
        SetText();
        this.num = num;
        //购买价格是卖出价格的三倍（后期可以试试四倍）
        //Debug.Log("当前id为： "+id);
        Price.GetComponent<Text>().text = (ShopItem.Price * 3).ToString();
        ItemImg = img;
        GetComponent<Image>().sprite = img;
    }
    public void SetText()
    {
        Item t = InitAllitems.GetById(id);
        string str = Item.GetDescribe(t);
        ShopItem = t;
        Describe.transform.GetChild(0).GetComponent<Text>().text = str;
        ItemDescribe = str;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Describe.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Describe.SetActive(false);
        UsePanel.SetActive(false);
    }


    public void ClickItem()
    {
        //卖出的物品再点击也是没用的
        if (!isSell)
        {
            //点击物体之后，显示“购买”1按钮，我们这里同样做成子物体
            Describe.SetActive(false);
            UsePanel.SetActive(true);
        }
    }

    public void BuyItem()
    {
        if(CoinControl.CostCoins(ShopItem.Price * 3))
        {
            //卖出去了，同时背包会增加该物品
            CostItem();
        }
        else
        {
            //弹出警告框：你小子钱不够！
            GameObject NoticeBox = GameObject.FindWithTag("Others").transform.GetChild(5).gameObject;
            NoticeBox.transform.GetChild(0).GetComponent<Text>().text = "金币不足，请获取更多！";
            NoticeBox.GetComponent<NoticeControl>().Show();
        }
    }

    public void CostItem()
    {
        isSell = true;
        //同时添加到背包
        GameObject.FindWithTag("Others").GetComponent<InitAllitems>().GetPlayer().BuySth(ShopItem);
        //更换贴图（卖出）
        Selled.SetActive(true);
    }
}
