using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraView : MonoBehaviour
{

    //默认分辨率
    public Vector2 resolution;     //基础比例配置
    public float pixelToUnit = 100f;
    public static float cameraScale = 1;
    public float movieSpeed;
    public MoveControl targetPoint=null;
    private Vector3 movelength; //内部常量

    //bg部分
    public GameObject quadBG;
    private Material mat;
    private Vector2 offset = Vector2.zero;
    private Vector2 speed = Vector2.zero;
    private GameObject player;
    private float playerX = 0;

    private void Awake()
    {
        resolution = new Vector2(1280, 720);
    }
    // Start is called before the first frame update
    private void Start()
    {
        movieSpeed = 2f;  //相机跟随移动速度
        var camera = GetComponent<Camera>();
        if (camera.orthographic)     //正交摄像机
        {
            //cameraScale = Screen.width / resolution.x;
            //pixelToUnit *= cameraScale;
            camera.orthographicSize = 3.6f ;        //相机比例恒为  先不管了
        }
    }

    public void initBGQuad()
    {
        mat = quadBG.GetComponent<Renderer>().material;
        offset = mat.GetTextureOffset("_MainTex");
        quadBG.SetActive(false);
        float scaleY = resolution.y / pixelToUnit;/// body.bounds.size.y;
        float scaleX = resolution.x / pixelToUnit;/// body.bounds.size.x;
        quadBG.transform.localScale = new Vector3(scaleX,scaleY,1);
    }

    private void LateUpdate()
    {
        movieFollow();
    }
    private void movieFollow()
    {
        if (targetPoint != null)
        {
            movelength= new Vector3(targetPoint.transform.position.x - transform.position.x + (targetPoint.getFaceDirection() ? -1 : 1) * 2,
                                 targetPoint.transform.position.y - transform.position.y + Vector2.up.y * 0.5f) * movieSpeed * Time.deltaTime;
            transform.position += movelength;
            moveBG();
        }
    }
    public void moveBG()
    {
        //float i = player.transform.position.x - playerX;
        playerX = player.gameObject.transform.position.x;  //差值计算

        speed = Vector2.right * movelength.x * 1f;
        offset += speed * Time.deltaTime;
        mat.SetTextureOffset("_MainTex", offset);//大背景图像位移
    }

    //相机跟随玩家
    public void cameraFollowPlayer()
    {
        targetPoint = PlayerControl.Instance.getPlayer();
        player = targetPoint.gameObject;
        quadBG.SetActive(true);
    }
    //相机离开玩家
    public void cameraLeavePlayer()
    {
        targetPoint = null;
    }
}
