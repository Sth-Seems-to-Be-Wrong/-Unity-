using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NoticeControl : MonoBehaviour
{
    //确认盒子，一般不会绑定任何函数
    void Start()
    {
        gameObject.SetActive(false);
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }
}
