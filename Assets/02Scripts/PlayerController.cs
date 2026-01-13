using System;
using UnityEngine;

public class PlayerController : MonoBehaviour
{

    public float sp = 3f;

    public string upAni = "PlayerUp";
    public string downAni = "PlayerDown";
    public string leftAni = "PlayerLeft";
    public string rightAni = "PlayerRight";
    public string deadAni = "PlayerDead";

    string nowAni = "";
    string oldAni = "";

    float axisH;
    float axisV;
    public float angleZ = -90f;

    Rigidbody2D rb;
    Animator animator;
    bool isMoving = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = rb.GetComponent<Animator>();
        oldAni = downAni;
    }

    // Update is called once per frame
    void Update()
    {
        if(isMoving == false)
        {
            axisH = Input.GetAxis("Horizontal");
            axisV = Input.GetAxis("Vertical");
        }   
        //키 입력으로 이동각도 구하기
        Vector2 fromPt = rb.position;
        Vector2 toPt = new Vector2(fromPt.x + axisH, fromPt.y + axisV);

        angleZ = GetAngle(fromPt, toPt);

        if(angleZ >= -45 && angleZ <45)
        {
            nowAni = rightAni;
        }
        else if(angleZ >= -135 && angleZ <= -45)
        {
            nowAni = downAni;
        }
        else if(angleZ >= 45 && angleZ <= 135)
        {
            nowAni = upAni;
        }
        else
        {
            nowAni = leftAni;
        }
        
        //애니메이션 전환
        if(nowAni != oldAni)
        {
            oldAni = nowAni;
            animator.Play(nowAni);
        }
    }

    private void FixedUpdate()
    {
        rb.linearVelocity = new Vector2(axisH, axisV) * sp;
    }

    public void SetAxis(float h, float v)
    {
        axisH = h;
        axisV =v;
        if(axisH == 0 && axisV == 0)
        {
            isMoving = false;
        }
        else
        {
            isMoving = true;
        }
    }

    private float GetAngle(Vector2 fromPt, Vector2 toPt)
    {
        float angle;

        if(axisH !=0 || axisV != 0)
        {
            float dx = toPt.x - fromPt.x;
            float dy = toPt.y - fromPt.y;

            float rad = Mathf.Atan2(dy, dx);
            angle = rad * Mathf.Rad2Deg;
        }
        else
        {
            angle = angleZ;
        }
        return angle;
    }
}
