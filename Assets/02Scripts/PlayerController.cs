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

    //������ ó��
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
        // ������ ���� ����(inDamaged)�� ���� Ű �Է��� ���� ���� ���� �߰�
        if (gameState != "playing" || inDamaged)
        {
            return;
        }

        if (isMoving == false)
        {
            axisH = Input.GetAxisRaw("Horizontal"); // �������� ���� Raw ���� (���Ͻø� GetAxis�� ���� ����)
            axisV = Input.GetAxisRaw("Vertical");
            
            // 대각선 이동 방지: 수평과 수직 입력이 동시에 있을 때 수평 우선
            if (axisH != 0 && axisV != 0)
            {
                axisV = 0; // 수직 입력 무시
            }
        }

        // [���� �ڵ� ����] Ű �Է����� �̵����� ���ϱ�
        Vector2 fromPt = rb.position;
        Vector2 toPt = new Vector2(fromPt.x + axisH, fromPt.y + axisV);

        // �Է��� ���� ���� ���� ���� (���� �̵� ���� ���� ����)
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

            //�ִϸ��̼� ��ȯ
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

        //������ �޾��� �� ���� ó��
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
            // �� �߿�: ���⼭ return�� �ؾ� �Ʒ� �̵� �ڵ尡 ������� �ʰ� �˹�(AddForce)�� ����
            return;
        }

        // [����] �밢�� �̵� �� ������ ���� (.normalized �߰�)
        rb.linearVelocity = new Vector2(axisH, axisV).normalized * sp;
    }

    public void SetAxis(float h, float v)
    {
        axisH = h;
        axisV = v;
        
        // 대각선 이동 방지: 수평과 수직 입력이 동시에 있을 때 수평 우선
        if (axisH != 0 && axisV != 0)
        {
            axisV = 0; // 수직 입력 무시
        }
        
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

        // [����] ���� �̵� �ÿ��� ������ ���ŵǵ��� ||(OR)�� ����
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
        // ���� ���°� �ƴ� ���� ������
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
            // �� [�ʼ� ����] �˹� ���� ������ �˸� (�̰� ��� �˹��� �� �ƴ� ��)
            inDamaged = true;

            // ������ ���� �� �˹�
            rb.linearVelocity = Vector2.zero; // ���� �ӵ� �ʱ�ȭ
            Vector2 toPos = (transform.position - enemy.transform.position).normalized;
            rb.AddForce(toPos * 5f, ForceMode2D.Impulse); // ���� 4 -> 5�� �ణ ����

            Invoke("DamageEnd", 0.5f); // �˹� �ð� 0.25 -> 0.5�� ����
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

        // [�߰�] �˹� ���� �� �̲����� ����
        rb.linearVelocity = Vector2.zero;
    }
}