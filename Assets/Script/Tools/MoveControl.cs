using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class MoveControl : MonoBehaviour
{
    private string selfname;
    float moveAxis;
    public float moveSpeed;
    private Animator anim;
    private int movestate;
    private Transform[] childs;
    private GameObject body;
    private bool isFaceLeft = true;       //面朝左

    public bool stepCalculate = false;      //野区遇怪  步数计算开关
    public int stepLenght = 0;              //遇怪步长
    public int stepProb = 0;                  //步长概率
    private float stepunit = 0;

    public bool moveCtrl = true;

    enum State {IDLE=0,WALK=1 }

    // Start is called before the first frame update
    public void initData()
    {
        selfname = transform.name;
        setChildsGameObject();
        moveSpeed = 2;
        movestate = (int)State.IDLE;
        moveCtrl = true;
    }
    /// <summary>
    /// 获得单位朝向  true为左 false为右
    /// </summary>
    public bool getFaceDirection() { return isFaceLeft; }

    //激活步数计数器
    public void activateStepCalculate()
    {
        setStepCalculate(true,1,1);     //步数计数器默认值
    }

    // Update is called once per frame
    void Update()
    {
        move();
    }

    /// <summary>
    /// 设置步数计数器参数
    /// sw-开关   、len-步长 、prob-步长概率
    /// </summary>
    public void setStepCalculate(bool sw,int len,int prob)
    {
        stepCalculate = sw;
        stepLenght = len;
        stepProb = prob;
    }
    //内部计算步长工具
    private void stepCalulateTool()
    {
        stepunit += Time.deltaTime;
    }

    //移动方法
    private void move()
    {
        if (!moveCtrl) { return;  }
        moveAxis = Input.GetAxis("Horizontal");
        if (moveAxis != 0)
        {
            if (stepCalculate)  //是否计算步长
            {
                stepCalulateTool();
            }
            isFaceLeft = moveAxis > 0;
            if (movestate != (int)State.WALK)
            {
                movestate = (int)State.WALK;
                changeAnim(selfname + "Walk");
            }
            transform.Translate(Vector2.right * moveAxis * moveSpeed * Time.deltaTime, Space.World);
        }
        else
        {
            if (movestate != (int)State.IDLE)
            {
                movestate = (int)State.IDLE;
                changeAnim(selfname + "Idle");
            }
        }
    }
    //播放动画
    private void changeAnim(string name)
    {
        Action changeDirection = delegate ()
          {
                if (isFaceLeft) gameObject.transform.localScale = new Vector3(Mathf.Abs(gameObject.transform.localScale.x)*-1, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
                else gameObject.transform.localScale = new Vector3(Mathf.Abs(gameObject.transform.localScale.x), gameObject.transform.localScale.y, gameObject.transform.localScale.z);
          };
        if (movestate == (int)State.WALK)
            PubTool.Instance.laterDo(0.25f, changeDirection);
        anim.Play(name);
    }

    //初始化获取子物体
    private void setChildsGameObject()
    {
        anim = GetComponent<Animator>();
        childs = GetComponentsInChildren<Transform>();
        foreach(var obj in childs)
        {
            if (obj.name == "body") { body = obj.gameObject; }
        }
    }
}
