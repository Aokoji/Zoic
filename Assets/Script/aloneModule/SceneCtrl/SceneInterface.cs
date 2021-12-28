using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//场景组件接口
public interface SceneInterface 
{
    void initData(int id);
    int getSceneID();
    void loadScene();
}
//场景名 统一  Map+编号
