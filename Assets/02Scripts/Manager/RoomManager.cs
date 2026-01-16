using UnityEngine;
using UnityEngine.SceneManagement;

public class RoomManager : MonoBehaviour
{
    public static int doorNum = 0;

    private void Start()
    {
        GameObject[] enters = GameObject.FindGameObjectsWithTag("Exit");
        for (int i = 0; i < enters.Length; i++)
        {
            GameObject doorObj = enters[i];
            Exit exit = doorObj.GetComponent<Exit>();
            if(doorNum == exit.doorNum)
            {
                //같은 문 번호 일때 플레이어 캐릭터를 출입구로 이동
                float x = doorObj.transform.position.x;
                float y = doorObj.transform.position.y;
                if (exit.exitDirection == ExitDirection.up)
                {
                    y += 1;
                }
                else if (exit.exitDirection == ExitDirection.down)
                {
                    y -= 1;
                }
                else if (exit.exitDirection == ExitDirection.left)
                {
                    x -= 1;
                }
                else if (exit.exitDirection == ExitDirection.right)
                {
                    x += 1;
                }
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                player.transform.position = new Vector3(x, y, 0);
                break;
            }
        }
    }

    public static void ChangeScene(string sceneName, int doorNumber)
    {
        doorNum = doorNumber;
        string nowScene = PlayerPrefs.GetString("LastScene");
        if (nowScene != "")
        {
            SaveDataManager.SaveArrangeData(nowScene);
        }

        PlayerPrefs.SetString("LastScene", sceneName);
        PlayerPrefs.SetInt("LastDoor", doorNum);
        ItemKeeper.SaveItem();
        
            
        
        SceneManager.LoadScene(sceneName);
    }
}
