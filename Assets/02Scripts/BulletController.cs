using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float deleteTime = 3f;

    private void Start()
    {
        Destroy(gameObject, deleteTime);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerController playerController = collision.gameObject.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.GetDamage(collision.gameObject);
            }
        }
        Destroy(gameObject);
    }
}
