using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraView : MonoBehaviour
{

    //默认分辨率
    public Vector2 resolution;
    public static float pixelToUnit = 100f;
    public static float cameraScale = 1;

    // Start is called before the first frame update
    void Start()
    {
        resolution = new Vector2(1280, 720);
        var camera = GetComponent<Camera>();
        if (camera.orthographic)     //正交摄像机
        {
            cameraScale = Screen.width / resolution.x;
            pixelToUnit *= cameraScale;
            //camera.orthographicSize = (Screen.height / 2) / pixelToUnit; ;        //相机比例恒为  先不管了
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
