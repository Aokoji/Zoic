using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraView : MonoBehaviour
{

    //默认分辨率
    public Vector2 resolution;
    public static float pixelToUnit = 100f;
    public static float cameraScale = 1;
    public float movieSpeed;
    public MoveControl targetPoint=null;

    // Start is called before the first frame update
    private void Start()
    {
        movieSpeed = 2f;  //相机跟随移动速度
        resolution = new Vector2(1280, 720);
        var camera = GetComponent<Camera>();
        if (camera.orthographic)     //正交摄像机
        {
            cameraScale = Screen.width / resolution.x;
            pixelToUnit *= cameraScale;
            //camera.orthographicSize = (Screen.height / 2) / pixelToUnit; ;        //相机比例恒为  先不管了
        }
    }

    private void Update()
    {
        movieFollow();
    }
    private void movieFollow()
    {
        if (targetPoint != null)
        {
            transform.position += new Vector3(targetPoint.transform.position.x - transform.position.x + (targetPoint.getFaceDirection()?-1:1) * 2,
                                 targetPoint.transform.position.y - transform.position.y + Vector2.up.y*0.5f ) * movieSpeed * Time.deltaTime;
        }
    }
    //相机跟随玩家
    public void cameraFollowPlayer()
    {
        targetPoint = PlayerControl.Instance.getPlayer();
    }
    //相机离开玩家
    public void cameraLeavePlayer()
    {
        targetPoint = null;
    }
}
