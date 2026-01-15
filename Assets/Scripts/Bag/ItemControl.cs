using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class ItemControl : MonoBehaviour
    , IPointerEnterHandler, IPointerExitHandler
{
    //id是因为要被直接访问到public
    public int id;
    private int num;
    private Item Bagitem;
    private string ItemDescribe;
    private Sprite ItemImg;
    //操作按钮
    public GameObject Describe;
    public GameObject UsePanel;
    //数量按钮
    public GameObject number;
    //装备栏,对预制体只能实时获取
    private GameObject EquipmentPanel;
    public void Start()
    {
        Describe.SetActive(false);
        UsePanel.SetActive(false);
        EquipmentPanel = GameObject.Find("Equipment");
    }

    public void SetIDetails(int id,int num,Sprite img)
    {
        this.id = id;
        this.num = num;
        number.GetComponent<Text>().text = num.ToString();
        ItemImg = img;
        GetComponent<Image>().sprite = img;
    }
    public void SetText()
    {
        Item t = InitAllitems.GetById(id);
        string str = Item.GetDescribe(t);
        Bagitem = t;
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
        //点击物体之后，显示“使用、卖掉”两个按钮，我们这里同样做成子物体
        Describe.SetActive(false);
        UsePanel.SetActive(true);
    }

    public void UseItem()
    {
        if (Bagitem.Type == 0)
        {
            //这里是恢复品
            GameObject.FindWithTag("Others").GetComponent<InitAllitems>().GetPlayer().EatSomething(Bagitem.HP, Bagitem.MP);
        }
        else
        {
            //装备代码,添加装备
            EquipmentPanel.transform.GetChild(Bagitem.Type - 1).GetComponent<EquipmentControl>().AddEquipment(id, ItemDescribe, ItemImg,Bagitem);
        }
        CostItem();
    }
    public void SellItem()
    {
        CoinControl.Add(Bagitem.Price);
        CostItem();
    }

    public void CostItem()
    {
        num--;
        number.GetComponent<Text>().text = num.ToString();
        BagControl.OutItem(id, 1);
        if (num == 0)
        {
            Destroy(gameObject);
        }
    }
}
