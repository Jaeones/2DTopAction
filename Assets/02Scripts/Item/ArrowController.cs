using UnityEngine;

public class ArrowController : MonoBehaviour
{
    public float deleteTime = 2;
    public GameObject hitEffect;


    private void Start()
    {
        Destroy(gameObject, deleteTime);
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        transform.SetParent(collision.transform);

        GetComponent<Collider2D>().enabled = false;
        GetComponent<Rigidbody2D>().simulated = false;
        Instantiate(hitEffect, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
