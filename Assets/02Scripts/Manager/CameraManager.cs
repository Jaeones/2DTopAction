using UnityEngine;

public class CameraManager : MonoBehaviour
{
    GameObject player;

    public GameObject otherTarget;

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        if (player != null)
        {
            if (otherTarget != null)
            {
                //float midX = (player.transform.position.x + otherTarget.transform.position.x) / 2;
                //float midY = (player.transform.position.y + otherTarget.transform.position.y) / 2;
                //transform.position = new Vector3(midX, midY, transform.position.z);
                Vector2 pos = Vector2.Lerp(player.transform.position, otherTarget.transform.position, 0.5f);
                transform.position = new Vector3(pos.x, pos.y, transform.position.z);
            }
            else
            {
                transform.position = new Vector3(player.transform.position.x, player.transform.position.y, transform.position.z);
            }               
        }
    }
}
