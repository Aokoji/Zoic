using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
//环境管理器
public class EnvironmentManager : DDOLController<EnvironmentManager>
{
    private SceneInterface sceneScript;     //暂存当前组件的实体
    private GameObject scenePrefab;
    private int sceneID;        //当前场景id        （大环境） (如果有小场景加载，大环境的scene切换显得不是很必要）
    private int loadID;         //背景组件id        （小环境）
    private bool isfirst;
    private string sceneloadPath = "Entity/scene/scene";

    public void initData()
    {
        //初始化并创建实例  目前还不知道初始化啥
        loadID = GameData.Data.playerBridge.getLastScene();
    }
    //获得当前场景id
    public int getCurSceneID() { return sceneID; }
    //=====================================     内调      ==========
    //初始化最初场景
    ///没有场景数据  则为第一次进入  需要先加载 等待剧情结束再调用显示
    private void initalGameEnvironment()
    {
        sceneID = 101;
        loadID = 101;
        isfirst = false;
    }
    //================================      当前场景组件管理器       ==================（小场景管理器）

    //------------------------------------转场部分------------------
    private float curtainRun=0f;   //跑条
    private float curtainTime = 2f;  //黑幕时间（加载时间）(最低时间）
    private Action curtainAction;
    //切屏幕布   目前为固定时间 (仅限普通转场)           外调！
    public void changeSceneCurtain(int id,Action callback)
    {
        loadID = id;
        curtainRun = 0;
        curtainAction = callback;
        ViewController.Instance.playSceneChangeCurtain(true,startRunCurtain);
    }
    //启动
    private void startRunCurtain()
    {
        StartCoroutine(runCurtainSceen());
        changePrefabScene();
        sceneUnloadClear();//   清理gc工具
    }
    //启动后 开始切换
    public void changePrefabScene()
    {
        if (scenePrefab != null)
        {
            Destroy(scenePrefab);
            scenePrefab = null;
            sceneScript = null;
        }
        GameObject loadobj = Resources.Load<GameObject>(sceneloadPath + loadID);
        scenePrefab = Instantiate(loadobj);
        ViewController.instance.addToBaseMod_Load(scenePrefab);
        sceneScript = scenePrefab.GetComponent<SceneInterface>();
        sceneScript.initData(loadID);
    }
    IEnumerator runCurtainSceen()
    {
        curtainRun += Time.deltaTime;
        yield return null;
        if (curtainRun >= curtainTime)
        {
            changeWaitEnd();
        }
        else
            StartCoroutine(runCurtainSceen());
    }
    //结束
    private void changeWaitEnd()
    {
        EventTransfer.Instance.loadNewSceneAction();      //派发加载新场景完成事件
        PubTool.Instance.addLogger(loadID + "  加载场景完成。");
        if (isfirst)
        {
            isfirst = false;
            changeCurtainEnd();
            ViewController.Instance.setChangeCurtainVisible(false);//隐藏幕布  为后续剧情准备
        }
        else
            ViewController.Instance.playSceneChangeCurtain(false, changeCurtainEnd);
        //+++读取坐标点 纠正玩家位置(和相机位置)
    }
    //最终
    private void changeCurtainEnd()
    {
        curtainAction?.Invoke();    //一般这种回调都是放开控制器什么的
        curtainAction = null;
    }


    //------------------------------------------场景管理器   ----------------------------------------
    //-----------           目前暂定 没有切换场景的必要了
    //切换场景方法
    public void changeScene(int id,Action action)
    {//BaseMain
        exitScene();

        sceneID = id;
        //+++ 显示过场动画
        //先去清理场景
        SceneManager.LoadScene("ClearSceneMap");
    }
    //固定点调用  非必要情况不要主动调用
    public void loadNewSceneOnClearScene()
    {
        SceneManager.LoadScene("Map" + sceneID);
        StartCoroutine(waitForLoadScene(sceneID));
    }
    private IEnumerator waitForLoadScene(int sceneid)
    {
        while (SceneManager.GetActiveScene().name != "Map" + sceneid)
        {
            yield return null;
        }
        EventTransfer.Instance.loadNewSceneAction();      //派发加载新场景完成事件
        PubTool.Instance.addLogger(sceneid + "  加载场景完成。");
    }
    public void exitScene()
    {
        //+++保存切图数据 (存到数据层，当执行存储才会全部写文件)
    }
    //加载场景后 场景控制器来调用的赋值方法
    //
    //  因为获取场景控制器比较浪费性能    所以临时想到由加载好的场景控制来主动赋值
    //（太蠢了，但这是目前简单好用的办法）    但还是太蠢了...有机会要换掉
    public void setValueSceneToStart(SceneInterface scene)
    {
        //scenePrefab = scene;
        //loadEnvironment();
        //nextStep?.Invoke();
        //nextStep = null;
    }
    //-------------------------------------------------------------------以上一段  暂时不用了

    /// <summary>
    /// -----------------------------------        调一次的开始特殊方法       --------------------
    /// </summary>
    //进入游戏的场景检查     点开始游戏会调用  只会调用一次
    public void checkStartGameSceneAndDo(Action mid, Action action)
    {
        if (loadID == 0)
            initalGameEnvironment();
        changeSceneCurtain(loadID, mid, action);
    }
    private void changeSceneCurtain(int id,Action midAction, Action callback)
    {
        loadID = id;
        curtainRun = 0;
        curtainAction = callback;
        ViewController.Instance.playSceneChangeCurtain(true,delegate(){ startSpecialfunc(midAction); });
    }
    private void startSpecialfunc(Action step)
    {
        StartCoroutine(runCurtainSceen());
        step();
        changePrefabScene();
        sceneUnloadClear();//   清理gc工具
    }

    //场景清理工具
    private void sceneUnloadClear()
    {
        object[] objary = Resources.FindObjectsOfTypeAll<Material>();
        for (int i = 0; i < objary.Length; ++i)
            objary[i] = null;
        object[] objary2 = Resources.FindObjectsOfTypeAll<Texture>();
        for (int i = 0; i < objary2.Length; ++i)
            objary2[i] = null;
        //卸载没有被引用的资源
        Resources.UnloadUnusedAssets();
        //立即进行垃圾回收
        GC.Collect();
        PubTool.Instance.addLogger("回收场景资源，准备载入场景跳转。");
        GC.WaitForPendingFinalizers();//挂起当前线程，直到处理终结器队列的线程清空该队列为止
        GC.Collect();
    }
}
//需要慢慢调开始的场景清理  要把start的删干净 或者直接做成场景