using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainView : MonoBehaviour
{

    public Button testBtn;

    public void initData()
    {
        initEvent();
    }
    private void initEvent()
    {
        testBtn.onClick.AddListener(openBag);
    }
    private void openBag()
    {
        BagControl.Instance.openBag(null);
    }
























    //外部初始化调用方法
    //public static void initCreateMainView()
    //{
    //    mainview = null;
    //    GameObject obj = Resources.Load<GameObject>("Entity/MainView");
    //    var view = Instantiate(obj);
    //    view.name = "MainView";
    //    view.transform.SetParent(GameObject.Find("Canvas").transform, false);
    //    mainview = view.GetComponent<MainView>();
    //    mainview.initData();
    //    mainview.gameObject.SetActive(true);
    //}
    

}
