using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
public class PubTool : DDOLController<PubTool>
{
    private bool stepLock = false;                      //方法执行中锁
    private bool stepAllow = true;                      //方法执行信号
    private List<Action<Action>> stepList = new List<Action<Action>>();     //公用方法序列
    private StreamWriter sw = null;
    //private string logPath = "Assets/Resources/Data//eventLog.txt";
    //private FileInfo logfile;

    private void Start()
    {
        //logfile = new FileInfo(logPath);
    }
    private void Update()
    {
        updateStep();
    }
    /// <summary>
    /// 延时方法  参数 （延时float   回调action）
    /// </summary>
    public void laterDo(float time,Action action)
    {
        StartCoroutine(lateraction(time, action));
    }
    IEnumerator lateraction(float time,Action action)
    {
        yield return new WaitForSeconds(time);
        action();
    }

    //序列方法函数
    private void updateStep()
    {
        stepAllow = stepList.Count > 0;
        if (!stepLock)
        {
            if (stepAllow)
            {
                stepLock = true;
                Action<Action> callback = stepList[0];
                stepList.Remove(stepList[0]);
                callback(stepNext);
            }
        }
    }
    private void stepNext()
    {
        stepLock = false;
    }
    /// <summary>
    /// 添加执行步骤  必须含有参数action  可以进行下一步时调用  类似callback（其实就是）
    /// </summary>
    public void addStep(Action<Action> action)
    {
        stepList.Add(action);
    }
    /// <summary>
    /// 清理方法序列  执行中方法停不掉
    /// </summary>
    public void clearStep()
    {
        stepList.Clear();
        stepLock = false;
        stepAllow = false;
    }
    /// <summary>
    /// 操作日志
    /// </summary>
    public void addLogger(string context)
    {
        if (!Directory.Exists("Logs")) Directory.CreateDirectory("Logs");       //根目录存在Logs文件夹   不存在则创建
        string path=Path.Combine("Logs", "mainLog.txt");
        string mess = DateTime.Now.ToString("yyyy-MM-dd  HH:mm:ss    ") + context + "\n";
        File.AppendAllText(path, mess);
    }
    /// <summary>
    /// 战斗记录操作日志
    /// </summary>
    public void addCombatLogger(string type, string context)
    {
        if (!Directory.Exists("Logs/combat")) Directory.CreateDirectory("Logs/combat");       // 不存在则创建
        string path = Path.Combine("Logs/combat", type + ".txt");
        string mess = DateTime.Now.ToString("yyyy-MM-dd  HH:mm:ss    ") + context + "\n";
        File.AppendAllText(path, mess);
    }
    public void gameError(string message,bool iskill)
    {
        Debug.LogError(message);
        addLogger(message);
        if (iskill)
        {
            Application.Quit();
        }
    }
}
