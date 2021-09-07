using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using System.Text;
using System.Reflection;
/// <summary>
/// 读json测试工具
/// **将配置数据读取到对应存储变量中
/// </summary>
public class JsonReadToolTest
{
    public void readAllJsonData()
    {
        string[] mes = LoadPaths.constantJsonPath;
        JsonDataSave jEntity = JsonDataSave.jsdata;
        Type jType = jEntity.GetType();
        //读取
        for (int i = 0; i < mes.Length; i += 2)
        {
            FieldInfo jLocal = jType.GetField(mes[i + 1]);  //拿到变量
            Type taType = jLocal.GetType();     //实例

            if (!File.Exists(Application.dataPath + mes[i]))
            {
                //不存在
                MethodInfo func = jType.GetMethod("createNewJson");
                func.Invoke(jEntity, new object[] { mes[i], mes[i + 1] });
            }
            //读取
            byte[] jsbt = File.ReadAllBytes(Application.dataPath + mes[i]);
            string read = Encoding.ASCII.GetString(jsbt);
            //转换赋值
            MethodInfo method= jType.GetMethod(mes[i + 1] + "Read");
            method.Invoke(jType, new object[] { read });
        }
    }

    //外部获取数据
    public T getJsonData<T>(string name, int num)
    {
        JsonDataSave jEntity = JsonDataSave.jsdata;
        Type jType = jEntity.GetType();
        FieldInfo jLocal = jType.GetField(name);
        IDictionary dict = jLocal.GetValue(jEntity) as IDictionary;
        if (dict == null)
            PubTool.Instance.gameError("静态数据读取错误！！！", true);
        return (T)dict[num];
    }
}
