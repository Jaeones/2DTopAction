using System;
using UnityEngine;

public class ArrowShot : MonoBehaviour
{
    public float shotSp = 12f;
    public float shotDelay = 0.25f;
    public GameObject bowPrefab;
    public GameObject arrowPrefab;

    bool inAttack = false;
    GameObject bowObj;
    PlayerController plmv;
    


    private void Start()
    {
        Vector3 pos = transform.position;
        bowObj = Instantiate(bowPrefab, pos, Quaternion.identity);
        bowObj.transform.SetParent(transform);
        

        plmv = GetComponent<PlayerController>();
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            Attack();
        }
        float bowZ = -1f;
        if(plmv.angleZ > 30 && plmv.angleZ < 150)
        {
            bowZ = 1f;
        }

        bowObj.transform.rotation = Quaternion.Euler(0, 0, plmv.angleZ);
        bowObj.transform.position = new Vector3(transform.position.x, transform.position.y, bowZ);
        
    }

    private void Attack()
    {
        if(ItemKeeper.hasArrows > 0 && inAttack == false)
        {
            ItemKeeper.hasArrows -= 1;
            inAttack = true;

            float angleZ = plmv.angleZ;
            Quaternion r = Quaternion.Euler(0, 0, angleZ);
            GameObject arrowObj = Instantiate(arrowPrefab, transform.position, r);

            float x = Mathf.Cos(angleZ * Mathf.Deg2Rad);
            float y = Mathf.Sin(angleZ * Mathf.Deg2Rad);

            Vector3 v = new Vector2 (x, y) * shotSp;
            arrowObj.GetComponent<Rigidbody2D>().AddForce (v, ForceMode2D.Impulse);

            Invoke("StopAttack", shotDelay);
        }
    }

    public void StopAttack()
    {
        inAttack = false;
    }
}
