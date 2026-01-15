using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//主要使用静态方法，因为这些方法经常被使用
public class InitAllitems : MonoBehaviour
{
    private static AllItems AllItems;
    private static List<List<Item>> QualityItems;
    //当前爆率，也许可以修改，此外还有经验率之类的，默认爆率10%，分为5个等级，最高爆率为20%
    //如果修改了爆率，那么起步就会使20%之类的
    private static int EquipmentProb =1;

    private static int CoinProb = 1;
    private static int ExperienceProb = 1;

    //初始化前需要控制玩家不显示，只有进入游戏之后才能显示
    private GameObject Player;

    //判断是否在游戏中
    private static bool isGameing = false;

    //判断是否进入游戏
    private static bool isStart = false;

    //控制所有关卡的位置以及玩家会出现的位置 6元组（地图4个，玩家x、y）
    List<List<float>> Position = new List<List<float>>();

    //控制音乐播放
    public GameObject BgMusic;


    //只执行一次，在start执行，方便其他脚本的awake函数获取到玩家信息再将玩家隐藏
    void Start()
    {

        Player = GameObject.FindWithTag("Player");

        Player.SetActive(false);

        string str = Resources.Load<TextAsset>("Json/GameData_item").text;
        AllItems = JsonUtility.FromJson <AllItems>(str);
        QualityItems = new List<List<Item>>();
        for (int i = 0; i < 5; i++)
        {
            QualityItems.Add(new List<Item>());
        }
        foreach (Item item in AllItems.data)
        {
            QualityItems[item.Quality-1].Add(item);
        }
        //分别为5个关卡设置参数
        Position.Add(new List<float> { 400, -25, -16, -22, -10, -17 });
        Position.Add(new List<float> { 400, -25, -16, -22, -10, -17 });
        Position.Add(new List<float> { 400, -25, -16, -22, -10, -17 });
        Position.Add(new List<float> { 400, -25, -16, -22, -10, -17 });
        Position.Add(new List<float> { 400, -25, -16, -22, -10, -17 });
    }

    public static Item GetById(int id)
    {
        if (id > AllItems.data.Count||id==0)
            return null;
        return AllItems.data[id-1];
    }
    public static Item EnemyDieDrop(int level)
    {
        //在初始化的时候就应该修改好品质背包，测试状态设置爆率100%，目前为10%
        if (Random.Range(0, 100) < EquipmentProb*10)
        {
            //只会爆该等级怪物对应的东西，随机爆一个
            return QualityItems[level-1][(int)(Random.Range(0,QualityItems[level-1].Count))];
        }
        else return null;
    }

    public static void EnemyDieCoin(int level)
    {
        CoinControl.Add(level * 6 * CoinProb);
    }
    public static int EnemyDieExp(int level)
    {
        return ExperienceProb * 6 * level;
    }
    public void SetProb(int EqP,int CP,int ExP)
    {
        EquipmentProb = EqP;
        CoinProb = CP;
        ExperienceProb = ExP;
    }

    //新的开始，目标是清空所有信息
    public void NewEnterGame()
    {
        isGameing = false;
        isStart = true;
        //玩家等级清零
        Player.GetComponent<PlayerContorl>().LoadState(1, 0, 1);
        //清空背包
        Player.GetComponent<PlayerContorl>().Bag.GetComponent<BagControl>().ClearBag();
        //清空装备栏
        Player.GetComponent<PlayerContorl>().ClearEquipmens();
        //清空钱币
        CoinControl.Clear();

        Player.SetActive(false);
        SceneManager.LoadScene("MyGame");
        //该函数也许加载不完毕
        //Debug.Log(SceneManager.GetActiveScene().name);
        //进入界面之后打开锁，但是可能加载场景需要时间，因此使用协程
        //播放音乐
        BgMusic.GetComponent<BGMusicControl>().PlayMusic(0);
        StartCoroutine(FindMap());
    }



    //进入关卡选择界面，同时更新当前的锁
    public void EnterGame()
    {
        isGameing = false;
        isStart = true;
        Player.SetActive(false);
        SceneManager.LoadScene("MyGame");
        //该函数也许加载不完毕
        //Debug.Log(SceneManager.GetActiveScene().name);
        //进入界面之后打开锁，但是可能加载场景需要时间，因此使用协程
        //播放音乐
        BgMusic.GetComponent<BGMusicControl>().PlayMusic(0);
        StartCoroutine(FindMap());
    }

    //返回游戏最开始界面
    public void ReturnTitle()
    {
        isGameing = false;
        isStart = false;
        Player.SetActive(false);
        SceneManager.LoadScene("GameStart");
        //播放音乐
        BgMusic.GetComponent<BGMusicControl>().PlayMusic(0);
    }

    public IEnumerator FindMap()
    {
        GameObject tmp = GameObject.FindWithTag("Map");
        while (tmp == null) {
            yield return new WaitForSeconds(0.2f);
            tmp = GameObject.FindWithTag("Map");
        }
        //真正进入游戏界面，更新地图+刷新人物状态
        tmp.GetComponent<MapControl>().UpdateDemo(Player.GetComponent<PlayerContorl>().GetMapCount());
        Player.GetComponent<PlayerContorl>().FlashStatus();
    } 

    //进入具体关卡
    public void EnterDemo(int index)
    {
        isGameing = true;
        Player.SetActive(true);
        
        //加载位置信息
        Player.transform.position = new Vector3(Position[index][4], Position[index][5], Player.transform.position.z);
        CameraControl.SetLimit(Position[index][0], Position[index][1], Position[index][2], Position[index][3]);
        index++;
        //播放音乐
        BgMusic.GetComponent<BGMusicControl>().PlayMusic(index);
        SceneManager.LoadScene("Demo"+index.ToString());
    }

    //判断是否在游戏中
    public static bool IsGaming()
    {
        return isGameing;
    }

    public static bool IsStart()
    {
        return isStart;
    }

    //任何需要玩家信息的内容可以从这里获取
    public PlayerContorl GetPlayer()
    {
        return Player.GetComponent<PlayerContorl>();
    }
}

public class AllItems
{
    public List<Item> data;
}
