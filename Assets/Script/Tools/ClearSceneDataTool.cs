using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//场景缓冲  数据清理  gc回收
public class ClearSceneDataTool : MonoBehaviour
{
    private int nextid;
    private void Awake()
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
    private void Start()
    {
        EnvironmentManager.Instance.loadNewSceneOnClearScene();
    }
    private void OnDestroy()
    {
        
    }
}
