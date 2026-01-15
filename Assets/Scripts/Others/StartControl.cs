using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartControl : MonoBehaviour
{
   //多绑定几个函数，在场景切换时如果找不到对应函数就动态获取

    public void NewStart()
    {
        GameObject.FindWithTag("Others").GetComponent<InitAllitems>().NewEnterGame();
    }

    public void MyStore()
    {
        GameObject.FindWithTag("Others").transform.GetChild(0).GetComponent<PanelControl>().PanelShow();
    }

    public void GoOnStart()
    {
        GameObject.FindWithTag("Others").GetComponent<InitAllitems>().EnterGame();
    }
}
