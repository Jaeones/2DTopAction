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

    //데미지 처리
    public static int hp = 3;
    public static string gameState;
    bool inDamaged = false;

    void Start()
    {
        hp = PlayerPrefs.GetInt("PlayerHp");
        rb = GetComponent<Rigidbody2D>();
        animator = rb.GetComponent<Animator>();
        oldAni = downAni;

        gameState = "playing";
    }

    void Update()
    {
        // 데미지 입은 상태(inDamaged)일 때도 키 입력을 막기 위해 조건 추가
        if (gameState != "playing" || inDamaged)
        {
            return;
        }

        if (isMoving == false)
        {
            axisH = Input.GetAxisRaw("Horizontal"); // 반응성을 위해 Raw 권장 (원하시면 GetAxis로 유지 가능)
            axisV = Input.GetAxisRaw("Vertical");
        }

        // [기존 코드 유지] 키 입력으로 이동각도 구하기
        Vector2 fromPt = rb.position;
        Vector2 toPt = new Vector2(fromPt.x + axisH, fromPt.y + axisV);

        // 입력이 있을 때만 각도 갱신 (직선 이동 버그 수정 포함)
        if (axisH != 0 || axisV != 0)
        {
            angleZ = GetAngle(fromPt, toPt);

            if (angleZ >= -45 && angleZ < 45)
            {
                nowAni = rightAni;
            }
            else if (angleZ >= -135 && angleZ <= -45)
            {
                nowAni = downAni;
            }
            else if (angleZ >= 45 && angleZ <= 135)
            {
                nowAni = upAni;
            }
            else
            {
                nowAni = leftAni;
            }

            //애니메이션 전환
            if (nowAni != oldAni)
            {
                oldAni = nowAni;
                animator.Play(nowAni);
            }
        }
    }

    private void FixedUpdate()
    {
        if (gameState != "playing") { return; }

        //데미지 받았을 때 연출 처리
        if (inDamaged)
        {
            float val = Mathf.Sin(Time.time * 50);
            if (val > 0)
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = true;
            }
            else
            {
                gameObject.GetComponent<SpriteRenderer>().enabled = false;
            }
            // ★ 중요: 여기서 return을 해야 아래 이동 코드가 실행되지 않고 넉백(AddForce)이 먹힘
            return;
        }

        // [수정] 대각선 이동 시 빨라짐 방지 (.normalized 추가)
        rb.linearVelocity = new Vector2(axisH, axisV).normalized * sp;
    }

    public void SetAxis(float h, float v)
    {
        axisH = h;
        axisV = v;
        if (axisH == 0 && axisV == 0)
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

        // [수정] 직선 이동 시에도 각도가 갱신되도록 ||(OR)로 변경
        if (axisH != 0 || axisV != 0)
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // 무적 상태가 아닐 때만 데미지
        if (collision.gameObject.CompareTag("Enemy") && !inDamaged)
        {
            GetDamage(collision.gameObject);
        }
    }

    public void GetDamage(GameObject enemy)
    {
        if (gameState == "playing")
        {
            hp--;
            PlayerPrefs.SetInt("PlayerHp", hp);
        }

        if (hp > 0)
        {
            // ★ [필수 수정] 넉백 상태 시작을 알림 (이게 없어서 넉백이 안 됐던 것)
            inDamaged = true;

            // 데미지 받을 시 넉백
            rb.linearVelocity = Vector2.zero; // 기존 속도 초기화
            Vector2 toPos = (transform.position - enemy.transform.position).normalized;
            rb.AddForce(toPos * 5f, ForceMode2D.Impulse); // 힘을 4 -> 5로 약간 상향

            Invoke("DamageEnd", 0.5f); // 넉백 시간 0.25 -> 0.5로 조정
        }
        else
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        gameState = "gameOver";
        GetComponent<CircleCollider2D>().enabled = false;
        rb.linearVelocity = Vector2.zero;
        rb.gravityScale = 1f;
        rb.AddForce(new Vector2(0, 5), ForceMode2D.Impulse);
        animator.Play(deadAni);

        Destroy(gameObject, 1f);
    }

    void DamageEnd()
    {
        inDamaged = false;
        gameObject.GetComponent<SpriteRenderer>().enabled = true;

        // [추가] 넉백 끝난 후 미끄러짐 방지
        rb.linearVelocity = Vector2.zero;
    }
}