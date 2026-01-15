using System;
using UnityEngine;
[Serializable]
public class Item
{
    public int ID;
    public string Name;
    public int Type;
    public int HP;
    public int MP;
    public int Hurt;
    public int BowHurt;
    public string Describe;
    public int Quality;
    public int Price;

    public Item()
    {
        //默认创建认为是空物体
        ID = -1;
    }

    public static Sprite GetItemImage(Item t)
    {
        string str = "Pictures/Equipment/";
        if (t != null)
        {
            switch (t.Type)
            {
                case 0:
                    if (t.HP == 0)
                    {
                        str = str + "P_Blue0" + t.Quality.ToString();
                    }
                    else
                    {
                        str = str + "P_Red0" + t.Quality.ToString();
                    }
                    break;
                case 1:
                    str = str + "Ac_Gloves0" + t.Quality.ToString();
                    break;
                case 2:
                    str = str + "W_Sword0" + t.Quality.ToString();
                    break;
                case 3:
                    str = str + "A_Shoes0" + t.Quality.ToString();
                    break;
                case 4:
                    str = str + "C_Elm0" + t.Quality.ToString();
                    break;
                case 5:
                    str = str + "A_Armour0" + t.Quality.ToString();
                    break;
                case 6:
                    str = str + "W_Bow0" + t.Quality.ToString();
                    break;
            }
        }
        else
        {
            str += "?";
        }
        return Resources.Load<Sprite>(str);
    }
    public static string GetDescribe(Item t)
    {
        string str = "";
        if (t != null)
        {
            str = t.Name;
            str = str + "\n类型：";
            switch (t.Type)
            {
                case 0:
                    str += "治疗物品";
                    break;
                case 1:
                    str += "护手";
                    break;
                case 2:
                    str += "大剑";
                    break;
                case 3:
                    str += "鞋子";
                    break;
                case 4:
                    str += "头盔";
                    break;
                case 5:
                    str += "护甲";
                    break;
                case 6:
                    str += "弓箭";
                    break;
            }
            str = str + "\n血量：" + t.HP.ToString();
            str = str + "\n蓝量：" + t.MP.ToString();
            str = str + "\n近伤：" + t.Hurt.ToString();
            str = str + "\n远伤：" + t.BowHurt.ToString();
            str = str + "\n品质：" + t.Quality.ToString();
            str = str + "\n描述：" + t.Describe;
            str = str + "\n价格：" + t.Price.ToString();
        }
        else
        {
            str = "???";
        }
        return str;
    }
}