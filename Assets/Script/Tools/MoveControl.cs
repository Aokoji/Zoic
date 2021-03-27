using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveControl : MonoBehaviour
{
    private string selfname;
    float moveAxis;
    public float moveSpeed;
    private Animator anim;
    private int movestate;
    private Transform[] childs;
    private GameObject body;

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

    // Update is called once per frame
    void Update()
    {
        move();
    }

    private void move()
    {
        if (!moveCtrl) { return;  }
        moveAxis = Input.GetAxis("Horizontal");
        if (moveAxis != 0)
        {
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

    private void changeAnim(string name)
    {
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
