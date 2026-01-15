using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShowTime : MonoBehaviour
{
    // Start is called before the first frame update
    private float timer;
    private Text text;
    void Start()
    {
        timer = 0;
        text = GetComponent<Text>();
        text.text = "";
        gameObject.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (timer >0)
        {
            timer -= Time.deltaTime;
            int tmp = (int)timer+1;
            text.text = "剩余时间还有" + tmp.ToString() + "秒";
        }
        else
        {
            text.text = "";
        }
    }
    //显示剩余的时间
    public void SetTime(float t)
    {
        timer = t;
    }
}
