using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Weapon
{
    Bow = 0,
    Sword=1
}

public enum Skill
{
    Red = 0,
    SunkHp = 1,
    MoreFreq = 2,
    MoreArrow =3
}

public class PlayerContorl : MonoBehaviour
{
    //摄像机
    public Camera MainCamera;
    
    //基础属性
    private Rigidbody2D rigid;
    private Weapon weapon = Weapon.Sword;
    private GameObject Bow;
    private GameObject Sword;
    private GameObject SwordPoint;
    public float AttackRidus=1;
    private bool isLive = true;
    private Animator an;

    //背包
    public GameObject Bag;

    //装备栏 6件装备
    private List<GameObject> Equipments;

    //使用血量公式 80+L*30 蓝量公式 30+L*20 经验公式 100+L*L*15 近战伤害每等级+15 远程伤害每次+10
    private float NowHp;
    private float Hurt;
    private float NormalHurt;//用来还原属性
    private float BowHurt;
    private float NowMp;
    private float MaxHp;
    private float MaxMp;
    private int Level=0;
    private float NowExperience=0;
    private float MaxExperience;
    private PlayerDataShow DataShow;
   
    //加上武器后最终折合属性
    private float FinalHurt;
    private float FinalBowHurt;
    private float FinalMaxHp;
    private float FinalNowHp;
    private float FinalMaxMp;
    private float FinalNowMp;
    private PlayerDetailShow DetailShow;

    //每次受伤都会触发一小段无敌时间
    private float NoBeHurtTime = 0.8f;
    private float CanBeHurtTimer = 0;

    //二段跳
    private int MaxJumpCount = 2;
    private int nowJumpCount = 0;
    private bool isGround=true;

    //速度+人物方向（包括还原属性）
    private float HSpeed = 8;
    private float NormalHSpeed = 8;
    private float VSpeed = 20;
    private float LeftScale;

    //攻击频率(包括还原属性)
    private float SwordTimer = 0;
    private float SwordTime = 0.35f;
    private float BowTimer = 0;
    private float BowTime = 0.6f;
    private float NormalBowTime = 0.6f;

    //碰到水直接死亡
    private bool isWater = false;
    public LayerMask Water;

    //冲刺所需
    private float DashSpeed = 15;
    private float maxDashTime = 0.35f;
    private float nowDashTime = 0;
    private bool isDash = false;
    //冲刺特效
    private GameObject RightDash;
    private GameObject LeftDash;

    //使用脚部来控制碰撞
    private Transform feet;
    public float feetRidus;
    public LayerMask Ground;

    //梯子上移动
    private bool isLadder = false;
    private float Gravity = 0;
    public float ladderSpeed = 5;

    //当前闯关数目
    private int MapCount = 1;

    //音乐
    private PlayerMusicControl AudioControl;

    //技能栏目
    private GameObject BowSkills;
    private GameObject SwordSkills;

    //技能特效
    private GameObject RedStatus_effect;
    private GameObject MoreFreq_effect;
    private GameObject MoreArrow_effect;
    private GameObject SunkHp_effect;

    //其他属性以及默认值，使用技能可以改变，技能结束变回默认值（只有近战可以吸血）
    private float SunkScale = 0;
    private float FreqScale = 1;
    private float RedScale = 1;
    private int ArrowCount = 1;
    private float NormalSunkScale = 0;
    private float NormalFreqScale = 1;
    private float NormalRedScale = 1;
    private int NormalArrowCount = 1;


    private void Awake()
    {
        Equipments = new List<GameObject>();
        Transform equips = GameObject.FindWithTag("Others").transform.Find("Bag/Equipment");
        int i = 0;
        foreach (Transform item in equips)
        {
            Equipments.Add(item.gameObject);
            //Debug.Log(Equipments[i].name);
            i++;
        }


        an = GetComponent<Animator>();
        rigid = GetComponent<Rigidbody2D>();
        LeftScale = transform.localScale.x;
        Bow = transform.GetChild(0).GetChild(1).GetChild(0).gameObject;
        Sword = transform.GetChild(0).GetChild(1).GetChild(1).gameObject;
        SwordPoint = transform.GetChild(0).GetChild(1).GetChild(1).GetChild(0).gameObject;

        feet = transform.GetChild(3);
        Gravity = rigid.gravityScale;
        AudioControl = GameObject.Find("PlayerMusic").GetComponent<PlayerMusicControl>();
        //左上角血条框应该100%存在
        DataShow = GameObject.Find("Character/PlayerData").GetComponent<PlayerDataShow>();
        DetailShow = GameObject.Find("Others").transform.Find("Bag/PlayerDetail").GetComponent<PlayerDetailShow>();
        
        //技能栏切换
        BowSkills = GameObject.Find("Character/BowSkill");
        SwordSkills = GameObject.Find("Character/SwordSkill");
        SwordSkills.SetActive(true);
        BowSkills.SetActive(false);
        //冲刺特效
        RightDash = transform.Find("RightDash").gameObject;
        LeftDash = transform.Find("LeftDash").gameObject;
        RightDash.SetActive(false);
        LeftDash.SetActive(false);

        //技能特效
        RedStatus_effect = transform.Find("RedStatus").gameObject;
        MoreArrow_effect = transform.Find("MoreArrow").gameObject;
        MoreFreq_effect = transform.Find("MoreFreq").gameObject;
        SunkHp_effect = transform.Find("SunkHP").gameObject;
        SunkHp_effect.SetActive(false);
        MoreFreq_effect.SetActive(false);
        RedStatus_effect.SetActive(false);
        MoreArrow_effect.SetActive(false);

        UpLevel(1);//初始为1级，如果加载了存档会覆盖当前等级
        UpdatePlayerData();
    }

    public void AddExperience(int add)
    {
        NowExperience += add;
    }
    private void UpLevel(int add)
    {
        Level +=add;
        MaxExperience = 100 + Level * Level * 15;
        MaxHp = 80 + Level * 30;
        MaxMp = 30 + Level * 20;
        NowHp = MaxHp;
        NowMp = MaxMp;
        Hurt += 15;
        BowHurt += 10;
        NormalHurt = Hurt;
        UpdatePlayerData();
    }

    public void LoadState(int nowlevel,float nowExperience,int count)
    {
        MapCount = count;
        Level = nowlevel;
        MaxExperience = 100 + Level * Level * 15;
        NowExperience = nowExperience;
        MaxHp = 80 + Level * 30;
        MaxMp = 30 + Level * 20;
        NowHp = MaxHp;
        NowMp = MaxMp;
        Hurt = 15*Level;
        BowHurt = 10*Level;
        NormalHurt = Hurt;
        UpdatePlayerData();
    }
    
    public int  GetMapCount()
    {
        return MapCount;
    }

    //更新关卡数目（在碰到宝箱之后可以进入下一关卡）
    public void NextMap(int count)
    {
        MapCount = MapCount > count ? MapCount : count + 1;
    }


    public int GetLevel()
    {
        return Level;
    }

    public float GetNowExperience()
    {
        return NowExperience;
    }

    // 固定更新
    void Update()
    {
        if (!isLive) return;
        CanBeHurtTimer += Time.deltaTime;
       

        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");
        //等级提升,等级+1，等级上限为10级
        //使用血量公式 50+L*30 蓝量公式 30+L*20 经验公式 100+L*L*15
        if (Level<10&&NowExperience >= MaxExperience)
        {
            NowExperience -= MaxExperience;
            UpLevel(1);
        }

        //调节弓箭攻击频率（技能频率态）
        BowTime = NormalBowTime*FreqScale;
        BowTimer += Time.deltaTime;
        SwordTimer += Time.deltaTime;

        //调节速度、近战伤害(技能红色态)
        HSpeed = NormalHSpeed * RedScale;
        Hurt = NormalHurt * RedScale;

        //实时更新人物信息
        UpdatePlayerData();
        if (FinalNowHp <= 0)
        {
            //补上死亡动画和场景切换
            Debug.Log("玩家死亡，游戏结束！");
        }

        //梯子上重力为0
        if (isLadder)
        {
            rigid.gravityScale = 0;
            if (v != 0)
            {
                rigid.velocity = new Vector2(rigid.velocity.x, v * ladderSpeed);
            }
        }
        else
        {
            rigid.gravityScale = Gravity;
        }

        
        //移动
        if (h != 0)
        {
            transform.localScale = new Vector3(h < 0 ? LeftScale : -LeftScale, transform.localScale.y, transform.localScale.z);
            rigid.velocity = new Vector2(h * HSpeed,rigid.velocity.y);
            //transform.Translate(Vector3.right * h * HSpeed * Time.deltaTime);
            an.SetBool("isRun", true);
        }
        else
        {
            rigid.velocity = new Vector2(0 , rigid.velocity.y);
            an.SetBool("isRun", false);
        }

        //冲刺
        if (nowDashTime==0&&Input.GetKeyDown(KeyCode.L))
        {
            isDash = true;
            AudioControl.Dash();
        }
        if (isDash)
        {
            if (nowDashTime == 0)
            {
             //打开拖影
                if(isLeft())
                    LeftDash.SetActive(true);
                else
                    RightDash.SetActive(true);
            }
            if (nowDashTime < maxDashTime)
            {
                rigid.velocity = Vector2.left * (isLeft() ? 1 : -1) * DashSpeed+Vector2.up * rigid.velocity.y;
                nowDashTime += Time.deltaTime;
               
            }
            else
            {
                rigid.velocity = Vector2.up*rigid.velocity.y;
                nowDashTime = 0;
                isDash = false;
                //关闭拖影
                LeftDash.SetActive(false);
                RightDash.SetActive(false);
            }
        }

        //切换武器
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //切换武器
            if(weapon == Weapon.Bow)
            {
                weapon = Weapon.Sword;
                Bow.SetActive(false);
                Sword.SetActive(true);
                an.SetFloat("whichWeapon", 0);
                BowSkills.SetActive(false);
                SwordSkills.SetActive(true);
            }
            else
            {
                weapon = Weapon.Bow;
                Bow.SetActive(true);
                Sword.SetActive(false);
                an.SetFloat("whichWeapon", 1);
                BowSkills.SetActive(true);
                SwordSkills.SetActive(false);
            }
        }


        //是否在地面上
        isGround = Physics2D.OverlapCircle(feet.position, feetRidus, Ground);
        if (isGround)
        {
            //Debug.Log("Ground");
            nowJumpCount = 0;
            an.SetBool("isJump", false);
        }
        else
        {
            an.SetBool("isJump", true);
        }

        //是否在水上，如果在水上直接死亡
        isWater = Physics2D.OverlapCircle(feet.position, feetRidus, Water);
        if (isWater)
        {
            Die();
            //Debug.Log("Water");
        }

        //二段跳
        if (Input.GetKeyDown(KeyCode.K)&& nowJumpCount < MaxJumpCount)
        {
            AudioControl.Jump();
            nowJumpCount++;
            rigid.velocity = Vector2.up * VSpeed;
        }
        //这行代码是因为，在第一次跳跃时，本来count应该=1，但是由于此时还在地面上时被归零了一次，往往跳跃按键还在按下状态
        //因此，如果在按住跳跃时count=0，就=1
        if (Input.GetKey(KeyCode.K)&&nowJumpCount==0)
        {
            nowJumpCount = 1;
        }

        

        //攻击以即攻击频率
        if (Input.GetKeyDown(KeyCode.J))
        {
            if (weapon == Weapon.Bow&&BowTimer>BowTime)
            {
                BowTimer = 0;
                an.SetTrigger("attack");
                AudioControl.Shoot();
                //射箭、可能一次多发
                Bow.GetComponent<BowControl>().attack(isLeft(),FinalBowHurt,ArrowCount);
            }

            if (weapon == Weapon.Sword && SwordTimer > SwordTime)
            {
                SwordTimer = 0;
                an.SetTrigger("attack");
                AudioControl.Attack();
                Collider2D[]t = Physics2D.OverlapCircleAll(SwordPoint.transform.position, AttackRidus);
                foreach (Collider2D tmp in t)
                {
                    if(tmp.tag == "Enemy")
                    {
                        tmp.gameObject.GetComponent<EnemyControl>().beHurt(FinalHurt);
                        //近战触发吸血
                        NowHp += FinalHurt * SunkScale;
                        if (NowHp > MaxHp) NowHp = MaxHp;
                    }
                }
            }
        }


        //技能，一共六个，目前基本完成
        int SkillCount = -1;
        if (Input.GetKey(KeyCode.U)) SkillCount = 0;
        if (Input.GetKey(KeyCode.I)) SkillCount = 1;
        if (Input.GetKey(KeyCode.O)) SkillCount = 2;
        if (SkillCount >= 0)
        {
            if (weapon == Weapon.Bow) BowSkills.GetComponent<SkillsControl>().UseSkill(SkillCount);
            else SwordSkills.GetComponent<SkillsControl>().UseSkill(SkillCount);
        }


        //碰撞检测，防止人物出现抖动，放在速度最后
        //人物半径为0.5
        Vector3 newPosition = transform.position + new Vector3(rigid.velocity.x * Time.deltaTime, rigid.velocity.y * Time.deltaTime);
        Vector3 newBody = newPosition + transform.right * 0.5f * (isLeft() ? -1 : 1);
        //Debug.Log(newBody);
        if (Physics2D.Linecast(newPosition, newBody, Ground))
        {
            rigid.velocity = Vector2.up * rigid.velocity.y;
        }
    }

    private void LateUpdate()
    {
        MainCamera.GetComponent<CameraControl>().MoveByPlayer(transform.position);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        //这里有碰撞到敌人扣血的情况，扣除nowhp，nowmp即可
    }
    private void OnCollisionExit2D(Collision2D collision)
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
       
    }

    //下面碰撞包含了捡到物品，但是这里购买物品也需要背包
    public void BuySth(Item tmp)
    {
        Bag.GetComponent<BagControl>().AddItem(tmp.ID, 1, Item.GetItemImage(tmp));
    }


    //只有梯子是无限检测
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ladder")
        {
            if (Input.GetAxis("Vertical")!=0)
            {
                isLadder = true;

            }
        }
        if (collision.gameObject.tag == "Item")
        {
            //碰到物品直接捡起
            ItemData tmp = collision.gameObject.GetComponent<ItemData>();
            Bag.GetComponent<BagControl>().AddItem(tmp.item.ID, tmp.num, tmp.img);
            Destroy(collision.gameObject);
        }
        //是否受伤
        if (CanBeHurtTimer < NoBeHurtTime) return;
        CanBeHurtTimer = 0;
        float tmphp = NowHp;
        if (collision.tag == "EnemyBall")
        {
            NowHp -= collision.GetComponent<EnemyBall>().GetHurt();
            //销毁火球
            Destroy(collision.gameObject);
        }
        if (collision.tag == "Enemy")
        {
            NowHp -= collision.GetComponent<EnemyControl>().GetHurt();
        }
        //受到伤害
        if (tmphp > NowHp)
        {
            AudioControl.BeHurt();
            StartCoroutine(MainCamera.GetComponent<CameraControl>().Shake(0.3f, 0.5f));
            //这里比较的是finalhp因为nowhp不包含装备,需要更新finalhp
            FinalNowHp -= (tmphp - NowHp);
            if (isLive && FinalNowHp <= 0)
            {
                Die();
                //触发其他死亡特效以及结局
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Ladder")
        {
            isLadder = false;
        }
    }

    //玩家死亡
    public void Die()
    {
        FinalNowHp = 0;
        an.SetTrigger("Die");
        isLive = false;
        //rigid.gravityScale = 0;
        rigid.velocity = Vector2.zero;
        //rigid.isKinematic = true;
        UpdatePlayerData();
        //3s后返回主菜单
        GameObject.FindGameObjectWithTag("Others").transform.Find("ShowTime").GetComponent<ShowTime>().SetTime(3);
        //死亡之前切换为剑形态（下回合开始便是）
        Invoke("MustUseSword", 1);
        Invoke("ReturnMenu", 3);
    }

    //玩家死亡后返回主菜单
    public void ReturnMenu()
    {
        GameObject.FindWithTag("Others").GetComponent<InitAllitems>().EnterGame();
    }

    public void MustUseSword()
    {
        //必须切换成为近战，不然会出现问题
        CloseAllSkills();
        weapon = Weapon.Sword;
        Bow.SetActive(false);
        Sword.SetActive(true);
        an.SetFloat("whichWeapon", 0);
        BowSkills.SetActive(false);
        SwordSkills.SetActive(true);
        an.SetTrigger("newGame");
    }

    //之后刷新状态(满血、满蓝量状态)
    public void FlashStatus()
    {
        NowHp = MaxHp;
        NowMp = MaxMp;
        isLive = true;
        UpdatePlayerData();
        rigid.isKinematic = false;
        //transform.localRotation = Quaternion.identity;
        //transform.rotation = Quaternion.identity;
    }

    public void EatSomething(int addHP,int addMp)
    {
        NowHp = NowHp + addHP >= MaxHp ? MaxHp : NowHp + addHP;
        NowMp = NowMp + addMp >= MaxMp ? MaxMp : NowMp + addMp;
        UpdatePlayerData();
    }

    public void GetFinalPlayerData()
    {
        FinalBowHurt = BowHurt;
        FinalHurt = Hurt;
        FinalNowHp = NowHp ;
        FinalNowMp = NowMp;
        FinalMaxHp = MaxHp;
        FinalMaxMp = MaxMp;
        foreach (GameObject go in Equipments)
        {
            EquipmentControl t= go.GetComponent<EquipmentControl>();
            if (t.GetEquipmentId()!= -1&&t.GetEquipment()!=null)
            {
                Item tmp = t.GetEquipment();
                FinalBowHurt += tmp.BowHurt;
                FinalHurt += tmp.Hurt;
                FinalNowHp +=tmp.HP;
                FinalNowMp += tmp.MP;
                FinalMaxHp += tmp.HP;
                FinalMaxMp += tmp.MP;
            }
        }
    }

    public void UpdatePlayerData()
    {
        GetFinalPlayerData();
        if(DataShow!=null)     DataShow.UpdateState(FinalMaxHp, FinalNowHp, FinalMaxMp, FinalNowMp, MaxExperience, NowExperience, Level);
        if(DetailShow!=null)   DetailShow.UpdataState(FinalMaxHp, FinalNowHp, FinalMaxMp, FinalNowMp, MaxExperience, NowExperience, Level, FinalHurt, FinalBowHurt);
        if (DetailShow == null)
        {
            Debug.Log("现在detail为null");
        }
        if (DataShow == null)
        {
            Debug.Log("现在data为null");
        }
    }

    public List<Item> SaveEquipments()
    {
        List<Item> res = new List<Item>();
        foreach (GameObject go in Equipments)
        {
            EquipmentControl t = go.GetComponent<EquipmentControl>();
            if (t.GetEquipment()==null)
            {
                Item tmp = new Item();
                res.Add(tmp);
            }else   res.Add(t.GetEquipment());
        }
        return res;
    }

    //清空装备栏
    public void ClearEquipmens()
    {
        foreach (GameObject go in Equipments)
        {
             EquipmentControl t = go.GetComponent<EquipmentControl>();
             t.DeleteEquipment();
        }
    }


    public void LoadEquipments(List<Item> ll)
    {
        int i = 0;
        //Debug.Log(Equipments.Count);
        //Debug.Log(Sword.name);
        foreach (GameObject go in Equipments)
        {
            Item tmp = ll[i];
            //Debug.Log(go.name);
            EquipmentControl t = go.GetComponent<EquipmentControl>();
            t.AddEquipment(tmp);
            i++;
        }
    }


    //用于判断角色当前是不是向左，很常用
    public bool isLeft()
    {
        return transform.localScale.x == LeftScale;
    }
    
    //判断是否能够释放技能
    public bool SkillsIsOk(float desMp)
    {
        if (FinalNowMp >= desMp)
        {
            NowMp -= desMp;
            FinalNowMp -= desMp;
            return true;
        }
        else return false;
    }

    //关闭所有技能
    public void CloseAllSkills()
    {
        //避免出现玩家死亡忘记关闭特效
        RedScale = NormalRedScale;
        RedStatus_effect.SetActive(false);
        SunkScale = NormalSunkScale;
        SunkHp_effect.SetActive(false);
        FreqScale = NormalFreqScale;
        MoreFreq_effect.SetActive(false);
        ArrowCount = NormalArrowCount;
        MoreArrow_effect.SetActive(false);
    }
    //技能描述
    //触发技能喽！,根据情况不同释放不同buff技能
    public IEnumerator StartSkill(float time,float scale, Skill num)
    {
        switch (num)
        {
            case Skill.Red:
                RedScale = scale;
                RedStatus_effect.SetActive(true);
                break;
            case Skill.SunkHp:
                SunkScale = scale;
                SunkHp_effect.SetActive(true);
                break;
            case Skill.MoreFreq:
                //获得加速倍速
                FreqScale = 2*NormalFreqScale-scale;
                MoreFreq_effect.SetActive(true);
                break;
            case Skill.MoreArrow:
                ArrowCount = (int)scale;
                MoreArrow_effect.SetActive(true);
                break;
        }
        yield return new WaitForSeconds(time);
        switch (num)
        {
            case Skill.Red:
                RedScale = NormalRedScale;
                RedStatus_effect.SetActive(false);
                break;
            case Skill.SunkHp:
                SunkScale = NormalSunkScale;
                SunkHp_effect.SetActive(false);
                break;
            case Skill.MoreFreq:
                FreqScale = NormalFreqScale;
                MoreFreq_effect.SetActive(false);
                break;
            case Skill.MoreArrow:
                ArrowCount = NormalArrowCount;
                MoreArrow_effect.SetActive(false);
                break;
        }
    }


    //测试近战共计范围、待修正
    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.DrawSphere(SwordPoint.transform.position, AttackRidus);
    //}
}
