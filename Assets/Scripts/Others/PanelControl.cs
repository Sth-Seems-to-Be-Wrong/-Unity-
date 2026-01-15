using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelControl : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        //因为很多界面必须开始要执行（因为需要awake方法）、执行完毕后再关闭
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PanelShow()
    {
        Time.timeScale = 0;
        gameObject.SetActive(true);
    }
    public void PanelClose()
    {
        Time.timeScale = 1f;
        gameObject.SetActive(false);
    }
}
