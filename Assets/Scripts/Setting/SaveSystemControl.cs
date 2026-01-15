using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class SaveSystemControl : MonoBehaviour
{
    private GameObject Player;
    public GameObject Bag;
    public GameObject infoBox;
    public GameObject NoticeBox;
    //0代表没有决断，1表示拒绝覆盖，2表示同意覆盖
    private int isCover = 0;
    void Awake()
    {
        isCover = 0;
        gameObject.SetActive(false);
        NoticeBox.SetActive(false);
        infoBox.SetActive(false);
        //最开始没有执行，等到后续执行时已经晚了
        Player = GameObject.FindWithTag("Player");
        //Debug.Log(Player.name);
    }
    private void Update()
    {
        
    }
    public void Close()
    {
        gameObject.SetActive(false);
    }

    public void Sure()
    {
        isCover = 2;
        infoBox.SetActive(false);
    }
    public void Cancel()
    {
        isCover = 1;
        infoBox.SetActive(false);
    }

    public void GetDataInfo()
    {
        int i = 1;
        //找到第三个孩子
        Transform buttons = transform.GetChild(2);
        foreach (Transform child in buttons)
        {
            int index = new int();
            index = i++;
            //动态加载文件时间
            string path = Path.Combine(Application.dataPath, "GameFile" + index.ToString() + ".ypy");
            if (!File.Exists(path))
            {
                child.GetChild(0).GetComponent<Text>().text = "存档" + index.ToString() + "(空)";
            }
            else
            {
                FileInfo t = new FileInfo(path);
                child.GetChild(0).GetComponent<Text>().text = "存档" + index.ToString() + "\n"+t.LastWriteTime.ToString();
            }
        }
    }


    public void ClickSave()
    {
        //在开始界面是不可以保存的
        if (!InitAllitems.IsStart())
        {
            NoticeBox.transform.GetChild(0).GetComponent<Text>().text = "还未进入游戏，不可保存存档！";
            NoticeBox.GetComponent<NoticeControl>().Show();
            return;
        }
        GetDataInfo();
        int i = 1;
        //找到第三个孩子
        Transform buttons = transform.GetChild(2);
        foreach (Transform child in buttons)
        {
            int index = new int();
            index = i++;
            //动态切换函数（保存或者读取）
            child.GetComponent<Button>().onClick.RemoveAllListeners();
            child.GetComponent<Button>().onClick.AddListener(delegate { SaveOne(index); });
        }
        transform.GetChild(0).GetComponent<Text>().text = "保存游戏";
        gameObject.SetActive(true);
    }
    public void ClickLoad()
    {
        //如果正在游戏中是不可以加载的
        if (InitAllitems.IsGaming())
        {
            NoticeBox.transform.GetChild(0).GetComponent<Text>().text = "正在游戏中，不可加载存档！";
            NoticeBox.GetComponent<NoticeControl>().Show();
            return;
        }
        GetDataInfo();
        int i = 1;
        Transform buttons = transform.GetChild(2);
        foreach (Transform child in buttons)
        {
            int index = new int();
            index = i++;
            child.GetComponent<Button>().onClick.RemoveAllListeners();
            child.GetComponent<Button>().onClick.AddListener(delegate { LoadOne(index); });
        }
        transform.GetChild(0).GetComponent<Text>().text = "读取游戏";
        gameObject.SetActive(true);
    }

    public void SaveOne(int index)
    {
        StartCoroutine(SaveOne2(index));
    }
    public IEnumerator SaveOne2(int index)
    {
        Debug.Log("进入存储系统");
        //默认未选择，直到选择框选择sure
        isCover = 0;

        string path = Path.Combine(Application.dataPath, "GameFile" + index.ToString() + ".ypy");
        bool isWrite = false;
        if (File.Exists(path))
        {
            //决定是否覆盖，出现提示框
            infoBox.SetActive(true);
            infoBox.transform.GetChild(0).GetComponent<Text>().text = "是否覆盖第" + index.ToString() + "存档?";

            //取消和确定按钮
            infoBox.transform.GetChild(1).GetComponent<Button>().onClick.AddListener(delegate { Sure(); });
            infoBox.transform.GetChild(2).GetComponent<Button>().onClick.AddListener(delegate { Cancel(); });

            //不能设置死循环，不然程序永远无法获得执行权，变成协程，方便后续处理
            while (isCover == 0)
            {
                yield return 0;
                // 0.2s好像会出现bug，这里每帧询问一次
                //yield return new WaitForSeconds(0.2f);
                Debug.Log("isCover=" + isCover.ToString());
            }
                //同意覆盖
            if (isCover == 2)
            {
                isWrite = true;
            }
            //删除绑定函数
            infoBox.transform.GetChild(1).GetComponent<Button>().onClick.RemoveAllListeners();
            infoBox.transform.GetChild(2).GetComponent<Button>().onClick.RemoveAllListeners();

            Debug.LogWarning("可能覆盖旧存档？");
        }
        //不存在默认书写
        if(isWrite||!File.Exists(path))
        {
            PlayerContorl pc =Player.GetComponent<PlayerContorl>();
            GameDataFile gf = new GameDataFile();
            gf.level = pc.GetLevel();
            gf.MapCount = pc.GetMapCount();
            gf.NowExperience = pc.GetNowExperience();
            gf.Equipment = pc.SaveEquipments();
            //最后需要拿到背包里面的东西
            BagControl.SaveDictionary();
            gf.BagKeys = BagControl.GetBagKey();
            gf.BagValues = BagControl.GetBagValue();
            gf.Coins = CoinControl.GetnowCoins();
            string str = JsonUtility.ToJson(gf);
            //文件打不开认为没有存档，直接创建新文件，同时提示保存成功
            File.WriteAllText(path, str);

            Debug.Log(path + "保存成功");
        }
        gameObject.SetActive(false);
    }

    

    //理论上在游戏中不能读取只能保存，只有在主页面能够读取
    public void LoadOne(int index)
    {
        string path = Path.Combine(Application.dataPath, "GameFile"+index.ToString()+".ypy");
        if (File.Exists(path))
        {
            string str = File.ReadAllText(path);
            GameDataFile t = JsonUtility.FromJson<GameDataFile>(str);
            PlayerContorl pc = Player.GetComponent<PlayerContorl>();

            pc.LoadEquipments(t.Equipment);
            pc.LoadState(t.level, t.NowExperience, t.MapCount);
            Bag.GetComponent<BagControl>().LoadBag(t.BagKeys, t.BagValues);
            CoinControl.SetCoins(t.Coins);
            Debug.Log(str);
            //要提示读取成功
            Debug.Log(path + "读取成功");
            //同时需要刷新地图
            StartCoroutine(GameObject.FindWithTag("Others").GetComponent<InitAllitems>().FindMap());
            gameObject.SetActive(false);
        }
        else
        {
            //显示读取失败，不会消失
            //Debug.LogError("文件不存在！");
        }
    }
}


[System.Serializable]
public class GameDataFile {
    //1.记录当前等级和当前装备，血量信息不保存，退出游戏以后关卡重新挑战
    //2.记录当前通关的关卡数目
    public int level;
    public int MapCount;
    public float NowExperience;
    public int Coins;
    public List<int> BagKeys;
    public List<int> BagValues;
    public List<Item> Equipment;
}


