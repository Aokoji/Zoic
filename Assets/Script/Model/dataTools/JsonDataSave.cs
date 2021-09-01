using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using System.Reflection;
using System.Text;
/// <summary>
/// json数据存储方法
/// 读取顺序  ：  读取方法  JsonReadToolTest--》存储数据  JsonDataSave--》没有则读取csv文件  StaticCSVDataTool
/// </summary>
public class JsonDataSave
{
    //----------------------------------------***    手动添加固定数据变量  无需赋值      ***------------------------------------------
    AllGoodStaticData allGoodData = new AllGoodStaticData();









    //------------------------------------------------***********************---------------------------------------------------------------
    //----以下区域不用操作-------------------
    delegate object csvdelegate();
    /// <summary>
    /// 转换数据方法 目前转换json读取的存储数据
    /// </summary>
    public T setValue<T>(System.Object obj)
    {
        return (T)obj;
    }
    /// <summary>
    ///     初始化json数据
    /// </summary>
    public void createNewJson<T>(string path, string name)
    {
        //假装读取csv得出了初始化类型
        //      invoke方法读取
        StaticCSVDataTool obj = new StaticCSVDataTool();
        Type csvEnt = obj.GetType();
        MethodInfo method = csvEnt.GetMethod("StaticCSVDataTool.loadData_" + name);
        T m = (T)method.Invoke(obj,null);
        /*
        //      代理方法读取
        StaticCSVDataTool obj0 = new StaticCSVDataTool();
        Type t0 = typeof(StaticCSVDataTool);      //获得类
        csvdelegate method0 = (csvdelegate)Delegate.CreateDelegate(t0, obj0, "StaticCSVDataTool.loadData_" + name);
        object m0=method0();
        */
        string json = JsonUtility.ToJson(m);
        byte[] js = Encoding.ASCII.GetBytes(json.ToCharArray());
        File.WriteAllBytes(Application.dataPath + path, js);
    }
}

//反射类型工具
namespace typeTools
{
    public class typeTrans
    {
        /// <summary>
        /// 输入  类型变量   属性名    设置内容.Tostring
        /// 返回  是否成功
        /// </summary>
        public bool setValue(object obj,string name,string v)
        {
            try
            {
                Type ts = obj.GetType();
                object vv = Convert.ChangeType(v, ts.GetProperty(name).PropertyType);
                ts.GetProperty(name).SetValue(obj, vv, null);
                return true;
            }
            catch
            {
                return false;
            }
            
        }
    }
}