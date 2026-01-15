using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleManager : MonoBehaviour
{
    public string firstSceneName;
    public GameObject startButton;
    public GameObject continueButton;

    private void Start()
    {
        string sceneName = PlayerPrefs.GetString("LastScene");
        if(sceneName == "")
        {
            continueButton.GetComponent<Button>().interactable = false;
        }
        else
        {
            continueButton.GetComponent <Button>().interactable = true;
        }
    }
    public void StartButtonClicked()
    {
        PlayerPrefs.DeleteAll();
        PlayerPrefs.SetInt("PlayerHp", 3);
        PlayerPrefs.SetString("LastScene", firstSceneName);
        RoomManager.doorNum = 0;
        SceneManager.LoadScene(firstSceneName);
    }

    public void ContinueButtonClicked()
    {
        string sceneName = PlayerPrefs.GetString("LastScene");  //저장된 씬
        RoomManager.doorNum = PlayerPrefs.GetInt("LastDoor");   //문번호
        SceneManager.LoadScene(sceneName);
    }


}
