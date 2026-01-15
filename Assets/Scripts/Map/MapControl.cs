using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapControl : MonoBehaviour
{
    private int DemoCount=5;
    List<DemoControl> Demos;
    private void Start()
    {
        //获取所有的关卡
        Demos = new List<DemoControl>();
        foreach (Transform item in transform)
        {
            Demos.Add(item.GetComponent<DemoControl>());
        }
        for (int i = 0; i < DemoCount; i++)
        {
            Demos[i].index = i ;
        }
    }


    public void UpdateDemo(int index)
    {
        Debug.Log("updateDemo");
        for (int i = 0; i < index; i++)
        {
            Demos[i].OpenLock();
        }
    }
}
