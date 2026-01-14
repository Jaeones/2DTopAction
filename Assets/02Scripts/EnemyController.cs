using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public int hp = 2;
    public float sp = 0.5f;
    //반응거리
    public float reactionDistance = 4f;

    public string idleAni = "EnemyDown";
    public string downAni = "EnemyDown";
    public string upAni = "EnemyUp";
    public string leftAni = "EnemyLeft";
    public string rightAni = "EnemyRight";
    public string DeadAni = "EnemyDead";

    string nowAni = "";
    string oldAni = "";

    float axisH;
    float axisV;

    Rigidbody2D rb;
    Animator animator;

    bool isActive = false;
    public int arrangeId = 0;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if(hp <= 0) { return; }

        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            float dis = Vector2.Distance(transform.position, player.transform.position);

            if (dis < reactionDistance)
            {
                isActive = true;
            }
            else
            {
                isActive = false;
            }

            if (isActive)
            {
                // 플레이어와의 각도 구하기
                float dx = player.transform.position.x - transform.position.x;
                float dy = player.transform.position.y - transform.position.y;
                float rad = Mathf.Atan2(dy, dx);
                float angle = rad * Mathf.Rad2Deg;

                if (angle > -45f && angle <= 45f)
                {
                    nowAni = rightAni;
                }
                else if (angle > 45f && angle <= 135f)
                {
                    nowAni = upAni;
                }
                else if (angle > -135f && angle <= -45f)
                {
                    nowAni = downAni;
                }
                else
                {
                    nowAni = leftAni;
                }

                axisH = Mathf.Cos(rad) * sp;
                axisV = Mathf.Sin(rad) * sp;
            }
            else
            {
                isActive = false;
                rb.linearVelocity = Vector2.zero;
                animator.Play("EnemyDown");
                oldAni = "EnemyDown";
            }
        }
    }

    private void FixedUpdate()
    {
        if (isActive && hp > 0)
        {
            rb.linearVelocity = new Vector2(axisH, axisV);
            if(nowAni != oldAni)
            {
                oldAni = nowAni;
                animator.Play(nowAni);
            }
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Arrow"))
        {
            hp--;
            if (hp <= 0)
            {
                isActive = false;
                GetComponent<CircleCollider2D>().enabled = false;
                rb.linearVelocity = Vector2.zero;
                animator.Play(DeadAni);

                Destroy(gameObject, 1f);
            }
        }
    }
}



