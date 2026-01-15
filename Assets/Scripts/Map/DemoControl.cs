using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class DemoControl : MonoBehaviour
{
    private bool isOpen = false;
    private GameObject Lock;
    private Button button;
    public int index;
    void Start()
    {
        //Ä¬ÈÏ¹Ø¿¨¼ÓËø
        Lock = transform.GetChild(1).gameObject;
        Lock.SetActive(true);
        button = GetComponent<Button>();
        button.onClick.AddListener(delegate { EnterDemo(); });
    }

    public void OpenLock()
    {
        isOpen = true;
        Lock.SetActive(false);
    }
    public void EnterDemo()
    {
        if(isOpen)   GameObject.FindWithTag("Others").GetComponent<InitAllitems>().EnterDemo(index);
    }
}
