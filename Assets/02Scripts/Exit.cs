using UnityEngine;


public enum ExitDirection { left, right, up, down }

public class Exit : MonoBehaviour
{
    public string sceneName = "";
    public int doorNum = 0;     //¹®¹øÈ£
    public ExitDirection exitDirection = ExitDirection.down;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            RoomManager.ChangeScene(sceneName, doorNum);
        }
    }
}
