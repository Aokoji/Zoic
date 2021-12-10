using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

//这些存成复用变量池  由单独的控制器集中控制
//实体应该是被动态创建出来的 所以应当附带初始化id     //animator应附在子集
public class EnemyMoveCtrl : MonoBehaviour
{
    public GameObject exclamtionTips;   //标识牌
    public GameObject rolePic;      //显示图片（当前角色）

    private  Animator anim;
    private enum statelist{ idle,patrol,alert,follow,away,normal,acting}   //等待，巡逻，警戒，追击，逃跑
    private statelist curState; //当前状态
    private statelist moveState;    //移动状态
    private bool isIntrigger;   //player是否进入警戒范围
    private bool isNearest;   //player是否进入最短行动距离
    private UnitTypeStaticData _data;
    private bool updatelock;
    private FieldTypeList fieldData;    //警戒类型数据
    private bool isleft;    //正方向

    //移动用参数
    private bool arrivePoint;   //是否到达
    private Vector2 targetpos;  //目标点
    private float idleWaittingCount;      //是否在idle一会计数
    private float waitStandard = GameStaticParamData.patrolTime;    //等待时间
    private float axis;     //速度爬升
    private float runawayTimeLimit = GameStaticParamData.runAwayTime;   //逃跑时间
    private float runawayTimeCount;     //逃跑时间计数
    private bool runningState;  //逃跑延长器
    private bool isplayingTips;     //正在播叹号
    private bool isSubSeq;      //后续判断（叹号专用）
    private bool isConsum;  //耗时（所有动作的中途的等待开关）action状态下强制等待

    private bool isplayTips;    //感叹号期间不允许动作    (很重要的判断）虽然现在还没用上

    private enum animname { moveIdle, movePatrol,moveRun,exclamtion,unExclamtion }

    //直接当做初始化方法了
    private void Start()
    {
        curState = statelist.idle;
        moveState = statelist.normal;
        anim = GetComponentInChildren<Animator>();
        isIntrigger = false;
        updatelock = true;
        runningState = false;
        idleWaittingCount = waitStandard / 2;
    }
    public void initData(int id)
    {
        _data = AllUnitData.Data.getJsonData<UnitTypeStaticData>(GameStaticParamData.unitName.unit, id);
        fieldData = GameStaticParamData.getFieldEnemyTypeData(_data.touchtype);
        changeSprite();
        isplayingTips = false;
        isSubSeq = false;
        isConsum = false;
        isplayTips = false;
        runningState = false;
        arrivePoint = true;
        updatelock = false;
    }
    //初始化正方向
    private void initDirection()
    {

    }

    private void Update()
    {
        if (!updatelock)
        {
            checkTrigger();
            moveFreelyState();
        }
    }

    private void checkTrigger()
    {
        float distance = Vector2.Distance(PlayerControl.Instance.getPosition(), transform.position);
        if (runningState)
        {
            isIntrigger = true;
            isNearest = true;
        }
        else
        {
            isIntrigger = distance <= fieldData.alertLength;
            isNearest = distance <= fieldData.alertMinLength;
        }
        if (!isIntrigger && !isplayingTips)
        {
            isSubSeq = false;
            exclamtionTips.SetActive(false);
        }
    }

    //不断更新状态
    private void moveFreelyState()
    {
        if (isIntrigger)
            alertState();
        else
            normalState();
    }

    //无警戒移动（普通移动和待机）
    private void normalState()
    {
        //随机一个方向 移动固定距离
        if (arrivePoint)
        {
            randomAPoint();
            arrivePoint = false;
        }
        if (idleWaittingCount < waitStandard)
        {
            idleWaittingCount += Time.deltaTime;
            if (curState != statelist.idle)
            {
                curState = statelist.idle;
                playselfAnimation(animname.moveIdle.ToString());
            }
            return;
        }
        if (curState != statelist.patrol)
        {
            randomAPoint();
            //处理正方向
            setLeftForword(transform.position.x - targetpos.x > 0);
            //处理移动状态
            curState = statelist.patrol;
            playselfAnimation(animname.movePatrol.ToString());
        }
        //移动
        transform.Translate((isleft ? Vector2.left : Vector2.right) * _data.moveSpeed * Time.deltaTime, Space.World);
        if (Vector2.Distance(targetpos, transform.position) < 1)
        {
            arrivePoint = true;
            idleWaittingCount = 0;
        }
    }
    //进入警戒
    private void alertState()
    {
        //最优先逃跑   
        //其次最近距离
        //其次警戒
        if (!isSubSeq)
        {
            //叹号牌
            if(!isplayingTips)
                playExclamationAnim();
            return;
        }
        if (isNearest)
        {
            if (fieldData.isrunAway)
                //逃跑动作
                runawayAction();
            else
                //进攻动作
                followAction();
        }
        else
        {
            if (fieldData.isAlert)
                //警戒动作
                alertAction();
            else
            {
                if (fieldData.isrunAway)
                    //逃跑动作
                    runawayAction();
                else
                    //进攻动作
                    followAction();
            }
        }
    }
    //逃跑动作
    private void runawayAction()
    {
        //状态普通  需要改变移动状态
        if (moveState == statelist.normal)
        {
            //先判断正方向
            setLeftForword(transform.position.x - PlayerControl.Instance.getPosition().x > 0);
            //播放动画
            playselfAnimation(animname.moveRun.ToString());
            moveState = statelist.acting;
            runawayTimeCount = 0;
            runningState = true;
            //+++亮逃跑牌
        }
        if (!isConsum)
        {
            runawayTimeCount += Time.deltaTime;
            transform.Translate((isleft ? Vector2.left : Vector2.right) * _data.moveSpeed *1.5f* Time.deltaTime, Space.World);
            if (runawayTimeCount >= runawayTimeLimit)
            {
                exclamtionTips.SetActive(false);
                //逃跑结束  一秒的延迟  再接一小会的叹号  再重复
                playselfAnimation(animname.moveIdle.ToString());
                isConsum = true;    //打开耗时等待开关
                PubTool.Instance.laterDo(0.8f, delegate ()
                {
                    moveState = statelist.normal;
                    runningState = false;
                    isSubSeq = false;   //叹号牌开关
                    isConsum = false;
                });
            }
        }
    }
    //警戒动作
    private void alertAction()
    {
        if (curState != statelist.alert)
        {
            curState = statelist.alert;
            moveState = statelist.normal;
            setLeftForword(transform.position.x - PlayerControl.Instance.getPosition().x < 0);
            //+++亮警戒标
        }
        transform.Translate((isleft ? Vector2.left : Vector2.right) * _data.moveSpeed * Time.deltaTime, Space.World);
    }
    //进攻动作
    private void followAction()
    {
        if (curState != statelist.follow)
        {
            curState = statelist.follow;
            moveState = statelist.acting;
            setLeftForword(transform.position.x - PlayerControl.Instance.getPosition().x > 0);
        }
        transform.Translate((isleft ? Vector2.left : Vector2.right) * _data.moveSpeed*1.5f * Time.deltaTime, Space.World);
    }

    //感叹号动画
    private void playExclamationAnim()
    {
        exclamtionTips.SetActive(true);
        isplayingTips = true;
        playselfAnimation(animname.exclamtion.ToString(),delegate() {
            if (isIntrigger) isSubSeq = true;
            else
            {
                isSubSeq = false;
                exclamtionTips.SetActive(false);
            }
            isplayingTips = false;
        });
    }

    //===========================        trigger     =================

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.name == "player")
        {
            updatelock = true;  //锁移动
            //碰撞  发送战斗事件
        }
    }

    //=============     内部功能        =====================
    private void changeSprite()
    {
        rolePic.GetComponent<Image>().sprite = Resources.Load("Picture/load/" + _data.name, typeof(Sprite)) as Sprite;
    }
    private void setLeftForword(bool isleft)
    {
        this.isleft = isleft;
        transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x) * (isleft?1:-1), transform.localScale.y, transform.localScale.z);
    }
    private void randomAPoint()
    {
        Vector2 pos=gameObject.transform.position;
        pos.x = UnityEngine.Random.Range(0, 1) == 1 ? pos.x + UnityEngine.Random.Range(5, 15) : pos.x - UnityEngine.Random.Range(5, 15);
        targetpos = pos;
    }
    private void playselfAnimation(string aniName)
    {

    }
    private void playselfAnimation(string aniName,Action callback)
    {

    }
}
