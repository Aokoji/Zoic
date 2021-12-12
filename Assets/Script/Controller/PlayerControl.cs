using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using XLua;

public class PlayerControl : DDOLController<PlayerControl>
{
    private MoveControl moveCtrl=null;
    private GameObject player;

    public void initData()
    {

    }

    public void initCreatePlayer()
    {
        if (player != null) { Destroy(player); player = null; }
        GameObject baseain = Resources.Load<GameObject>("Entity/Player");
        var baseMain = Instantiate(baseain);
        baseMain.name = "Player";
        //player.transform.position = GameData.Data.LastBornPos;
        ViewController.instance.addToBaseMod_Actor(baseMain);      //待定 需要设置视图层级
        player = baseMain;
        moveCtrl = baseMain.GetComponent<MoveControl>();
        player.SetActive(true);
        player.GetComponent<MoveControl>().initData();
    }

    //   人物控制开关
    public void setControl(bool ctrl)
    {
        //待添加   特殊情况会（比如战斗）消除玩家仇恨（也就是无敌）
        moveCtrl.moveCtrl = ctrl;
    }
    //  人物显示开关
    public void setVisible(bool visit)
    {
        player.SetActive(visit);
    }
    public MoveControl getPlayer()
    {
        return moveCtrl;
    }
    //获得正方向
    public bool getFaceLeft()
    {
        return moveCtrl.getFaceDirection();
    }

    public Vector2 getPosition()
    {
        return player.transform.position;
    }
    













    // Update is called once per frame
    void Update()
    {
 
    }

    void testMethod()
    {
        LuaEnv env = new LuaEnv();
        env.DoString("require  'project/textFile' ");
        LuaFunction fun = env.Global.Get<LuaFunction>("ShowGame");
        fun.Call();
    }

}
