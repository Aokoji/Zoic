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
        //读取
        for (int i = 0; i < mes.Length; i += 3)
        {
            JsonDataSave jEntity = JsonDataSave.jsdata;
            Type jType = jEntity.GetType();
            FieldInfo jLocal = jType.GetField(mes[i + 1]);  //拿到变量
            Type taType = jLocal.GetType();     //实例

            if (!File.Exists(Application.dataPath + mes[i]))
            {
                //不存在
                MethodInfo func = jType.GetMethod("createNewJson").MakeGenericMethod(taType);
                func.Invoke(jEntity, new object[] { mes[i], mes[i + 1] });
            }
            //读取
            byte[] jsbt = File.ReadAllBytes(Application.dataPath + mes[i]);
            string read = Encoding.ASCII.GetString(jsbt);
            object b = JsonUtility.FromJson<object>(read);
            //转换赋值
            //MethodInfo method = jType.GetMethod("setValue").MakeGenericMethod(taType);
            /*
            MethodInfo method = jType.GetMethod("convertType").MakeGenericMethod(taType);
            jLocal.SetValue(jType, method.Invoke(jEntity, new object[] { b }));
            */
            MethodInfo method= jType.GetMethod(mes[i + 1] + "Trans");
            method.Invoke(jType, new object[] { b });
        }
    }

    //外部获取数据
    public T getJsonData<T>(string name, int num)
    {
        Type jType = typeof(JsonDataSave);
        FieldInfo jLocal = jType.GetField(name);
        Type dic = jLocal.GetType();
        Dictionary<int, T> data = (Dictionary<int, T>)(dic.GetProperty("childDic").GetValue(jLocal, null));
        T sNode = data[num];
        return sNode;
    }
}
