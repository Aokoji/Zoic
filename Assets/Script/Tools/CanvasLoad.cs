using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
//这个改为controller 管理base 预制体
public class CanvasLoad : MonoBehaviour
{
    private CameraView camView;     //相机控制的脚本

    public GameObject background;
    public GameObject actor;
    public GameObject pop;
    public GameObject load;
    public GameObject uiPos;
    public GameObject mainCamera;

    public void initData()
    {
        camView = mainCamera.GetComponent<CameraView>();
        initLayout();
        initEvent();
        camView.initBGQuad();
    }
    private void initLayout()
    {
        setGroupsVisible(false);
    }
    private void initEvent()
    {
        EventTransfer.Instance.loadNewSceneEvent += onChangeSceneRefresh;
    }

    private void onChangeSceneRefresh()
    {
        setGroupsVisible(true);
    }


    private void setGroupsVisible(bool visit)
    {
        background.SetActive(visit);
        actor.SetActive(visit);
        pop.SetActive(visit);
        load.SetActive(visit);
    }
    //相机跟随玩家
    public void cameraFollowPlayer()
    {
        camView.cameraFollowPlayer();
    }
    //相机离开玩家
    public void cameraLeavePlayer()
    {
        camView.cameraLeavePlayer();
    }
}
