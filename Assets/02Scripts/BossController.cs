using UnityEngine;

public class BossController : MonoBehaviour         //보스는 가만히 서서 문을 지켜야함 플레이어가 인지범위에 오면 불렛을 발사
{
    public int hp = 10;
    public float reactionDistance = 7f;

    public GameObject bulletPrefab;
    public float shootSpeed = 5f;

    bool isShooting = false;
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        if (hp > 0)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                float distanceToPlayer = Vector2.Distance(transform.position, player.transform.position);
                if (distanceToPlayer <= reactionDistance && !isShooting)
                {
                    isShooting = true;
                    animator.Play("BossAttack");

                }
                else if (distanceToPlayer > reactionDistance && isShooting)
                {
                    isShooting = false;
                    animator.Play("BossIdle");
                }
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
                GetComponent<CircleCollider2D>().enabled = false;
                animator.Play("BossDead");
                Destroy(gameObject, 1f);
            }
        }
        if (collision.gameObject.CompareTag("Player"))
        {
            // Damage the player
        }
    }

    public void Fire()
    {
        Transform firePos = transform.Find("FirePos");
        GameObject FirePos = firePos.gameObject;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            float dx = player.transform.position.x - FirePos.transform.position.x;
            float dy = player.transform.position.y - FirePos.transform.position.y;

            float rad = Mathf.Atan2(dy, dx);
            float angle = rad * Mathf.Rad2Deg;
            GameObject bullet = Instantiate(bulletPrefab, FirePos.transform.position, Quaternion.Euler(0, 0, angle));
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            Vector2 direction = new Vector2(Mathf.Cos(rad), Mathf.Sin(rad)).normalized;
            rb.linearVelocity = direction * shootSpeed;
            

            // Vector2 direction = (player.transform.position - transform.position).normalized;
            //GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            //Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            //rb.linearVelocity = direction * shootSpeed;
        }
        //isShooting = false;
    }
}
