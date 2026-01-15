using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemData:MonoBehaviour
{
    public Item item;
    private SpriteRenderer sr;
    public int num;
    public Sprite img;
    private void Start()
    {
        
    }

    public void SetDetaild(Item tmp,int number)
    {
        num = number;
        item = tmp;
        sr = GetComponent<SpriteRenderer>();
        img = Item.GetItemImage(item);
        sr.sprite = img;
    }
}
