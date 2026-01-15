using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public  class ExitGame:MonoBehaviour
{

    public GameObject infoBox;
    private int isReturn = 0;
    private void Start()
    {
        isReturn = 0;
        infoBox.SetActive(false);
    }
    public void NowExitGame()
    {
        #if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
        #else
                Application.Quit();
        #endif
    }
    public void Sure()
    {
        isReturn = 2;
        infoBox.SetActive(false);
    }
    public void Cancel()
    {
        isReturn = 1;
        infoBox.SetActive(false);
    }
    //返回标题（最开始界面，记得保存游戏）
    public void ReturnTitle()
    {
        StartCoroutine(InfoReturn());
    }

    private IEnumerator InfoReturn()
    {
        //要弹窗提示：是否返回菜单
        //决定是否覆盖，出现提示框
        infoBox.SetActive(true);
        infoBox.transform.GetChild(0).GetComponent<Text>().text = "确定返回菜单吗？点击“继\n续游戏”可返回当前进度！";

        //删除绑定函数
        infoBox.transform.GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();
        infoBox.transform.GetChild(2).GetComponent<Button>().onClick.RemoveAllListeners();

        //取消和确定按钮
        infoBox.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(delegate { Sure(); });
        infoBox.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(delegate { Cancel(); });

        //不能设置死循环，不然程序永远无法获得执行权，变成协程，方便后续处理
        while (isReturn == 0)
        {
            yield return 0;
            // 0.2s好像会出现bug，这里每帧询问一次
            //yield return new WaitForSeconds(0.2f);
            //Debug.Log("isCover=" + isCover.ToString());
        }
        //删除绑定函数
        infoBox.transform.GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();
        infoBox.transform.GetChild(2).GetComponent<Button>().onClick.RemoveAllListeners();
        //同意退出
        if (isReturn == 2)
        {
            GameObject.Find("Others").GetComponent<InitAllitems>().ReturnTitle();
            GameObject.Find("Others").transform.GetChild(0).GetComponent<PanelControl>().PanelClose();
        }
        isReturn = 0;
    }
}
