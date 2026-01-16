using UnityEngine;

public class BulletController : MonoBehaviour
{
    public float deleteTime = 3f;
    PlayerController playerController;

    private void Start()
    {
        Destroy(gameObject, deleteTime);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        playerController.GetDamage(collision.gameObject);
        Destroy(gameObject);
    }
}
