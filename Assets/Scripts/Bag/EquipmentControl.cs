using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class EquipmentControl : MonoBehaviour
    , IPointerEnterHandler, IPointerExitHandler
{
    //设置为public是判断有没有装备的标志
    private int EquipmentId;
    private Sprite EquipmentImg;
    //这里不需要显示信息功能，先不实现，因为当前无法！！！等等不对劲
    public GameObject Describe;
    //预制体获得场上物体需要主动获取
    private GameObject Bag;
    //供玩家获取的属性
    private Item EquipmetItem;
    private void Start()
    {
        Bag = GameObject.Find("ItemsBag");
        Describe.SetActive(false);
        EquipmentId = -1;
        EquipmetItem = null;
    }

    public void AddEquipment(int id,string str,Sprite img,Item equipment)
    {
        if (EquipmentId != -1)
        {
            //需要把当前装备卸载下来
            Bag.GetComponent<BagControl>().AddItem(EquipmentId, 1, EquipmentImg);
        }
        GetComponent<Image>().sprite = img;
        EquipmentImg = img;
        Describe.transform.GetChild(0).GetComponent<Text>().text = str;
        EquipmentId = id;
        EquipmetItem = equipment;
        GameObject.FindWithTag("Others").GetComponent<InitAllitems>().GetPlayer().UpdatePlayerData();
    }

    //这个函数主要是从存档中加载需要，不需要保存当前物品，直接覆盖
    public void AddEquipment(Item equipment)
    {
        if (equipment.ID == -1)
        {
            //存档中为null就为null
            EquipmentId = -1;
            Describe.transform.GetChild(0).GetComponent<Text>().text = "";
            GetComponent<Image>().sprite = null;
            EquipmentImg = null;
            EquipmetItem = null;
            return;
        }
        Sprite img = Item.GetItemImage(equipment);
        GetComponent<Image>().sprite = img;
        EquipmentImg = img;
        Describe.transform.GetChild(0).GetComponent<Text>().text = Item.GetDescribe(equipment);
        EquipmentId = equipment.ID;
        EquipmetItem = equipment;
        GameObject.FindWithTag("Others").GetComponent<InitAllitems>().GetPlayer().UpdatePlayerData();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Describe.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Describe.SetActive(false);
    }
    public void OutEquipment()
    {
        if (EquipmentId != -1)
        {
            //先添加到背包
            Bag.GetComponent<BagControl>().AddItem(EquipmentId, 1, EquipmentImg);
            //主动把当前装备卸载掉
            DeleteEquipment();
        }
        GameObject.FindWithTag("Others").GetComponent<InitAllitems>().GetPlayer().UpdatePlayerData();
    }
    public Item GetEquipment()
    {
        return EquipmetItem;
    }
    public int GetEquipmentId()
    {
        return EquipmentId;
    }

    //删除装备，单纯删除
    public void DeleteEquipment()
    {
        EquipmentId = -1;
        Describe.transform.GetChild(0).GetComponent<Text>().text = "";
        GetComponent<Image>().sprite = null;
        EquipmentImg = null;
        EquipmetItem = null;
    }
}
