using UnityEngine;


public enum ExitDirection { left, right, up, down }

public class Exit : MonoBehaviour
{
    public string sceneName = "";
    public int doorNum = 0;     //¹®¹øÈ£
    public int targetDoorNum = 0;
    public ExitDirection exitDirection = ExitDirection.down;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(doorNum == 100)
            {
                GameObject.FindObjectOfType<UIManager>().GameClear();
            }
            else
            {
                string nowScene = PlayerPrefs.GetString("LastScene");
                SaveDataManager.SaveArrangeData(nowScene);
                RoomManager.ChangeScene(sceneName, targetDoorNum);
            }
        }
    }
}
