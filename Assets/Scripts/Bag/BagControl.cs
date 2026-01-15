using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//理论上，该类应该采用单例模式，但是部分函数是运行时执行，故不能全部static
public class BagControl : MonoBehaviour
{
    //当前角色的装备和金币
    private static Dictionary<int, int> Bag = new Dictionary<int, int>();
    //由于dictionary不可以序列化，我们拆分成两个数组
    private static List<int> BagKey = new List<int>();
    private static List<int> BagValue = new List<int>();

    private int allCoins;
    public GameObject ItemButton;
    public void AddItem(int id,int num,Sprite img)
    {
        int t=0;
        if (Bag.ContainsKey(id))
        {
            t = Bag[id] + num;
            Bag.Remove(id);
            Bag.Add(id, t);
            foreach (Transform each in transform)
            {
                ItemControl tmp = each.gameObject.GetComponent<ItemControl>();
                if (tmp.id == id)
                {
                    each.GetComponent<ItemControl>().SetIDetails(id, t, img);
                    break;
                }
            }
        } else {
            Bag.Add(id, num);
            t = num;
            GameObject button = Instantiate(ItemButton, transform);
            ItemControl ic = button.GetComponent<ItemControl>();
            ic.SetIDetails(id, t, img);
            ic.SetText();//设置描述，只需要一次
        }
    }
    public static void OutItem(int id, int num)
    {
        //删除放在子物体上，不需要用父物体控制，仅仅-1即可
        int t = Bag[id];
        Bag.Remove(id);
        if (num < t)
        {
            Bag.Add(id, t - num);
        }else if (num > t)
        {
            Debug.Log("超出范围，数量不够！");
        }
    }

    public void ClearBag()
    {
        Bag.Clear();
        foreach (Transform t in transform)
        {
            Destroy(t.gameObject);
        }
    }

    public static void SaveDictionary()
    {
        Dictionary<int,int>.KeyCollection k = Bag.Keys;
        Dictionary<int, int>.ValueCollection v = Bag.Values;
        BagKey.Clear();
        BagValue.Clear();
        foreach (int item in k)
        {
            BagKey.Add(item);
        }
        foreach (int item in v)
        {
            BagValue.Add(item);
        }
    }

    public  static List<int> GetBagKey()
    {
        return BagKey;
    }

    public static List<int> GetBagValue()
    {
        return BagValue;
    }

    public void LoadBag(List<int> k,List<int> v)
    {
        //从脚本中读到的数据，首先要清空背包
        ClearBag();
        //之后添加即可
        for (int i = 0; i < k.Count; i++)
        {
            AddItem(k[i], v[i], Item.GetItemImage(InitAllitems.GetById(k[i])));
        }
    }
}
