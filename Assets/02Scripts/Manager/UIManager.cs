using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    int hasKeys = 0;
    int hasArrows = 0;
    int hp = 0;

    public Text arrowText;
    public Text keyText;
    public GameObject hpImage;
    public Sprite[] lifeSprites;
    public GameObject mainImage;
    public GameObject resetButton;
    public Sprite[] gameSprites;

    public GameObject inputPanel;

    public string retrySceneName = "";

    GameObject player;

    private void Start()
    {
        UpdateItemCount();      //아이템 수 갱신
        UpdateHp();             //HP 갱신

        Invoke("InactiveImage", 1f);        //메인이미지 비활성화
        resetButton.SetActive(false);

        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void Update()
    {
        UpdateItemCount();      //아이템 수 갱신
        UpdateHp();             //HP 갱신
    }

    //HP 갱신 함수
    private void UpdateHp()
    {
        if(PlayerController.gameState != "gameend")
        {
            if(player != null)
            {
                if(PlayerController.hp != hp)
                {
                    //플레이어 사망, 게임오버 처리
                    hp = PlayerController.hp;
                    if(hp <= 0)
                    {
                        hpImage.GetComponent<Image>().sprite = lifeSprites[hp];     //하트 0개 이미지 
                        resetButton.SetActive(true);
                        mainImage.GetComponent<Image>().sprite = gameSprites[0];    //게임오버 이미지
                        mainImage.SetActive(true);
                        inputPanel.SetActive(false);
                        PlayerController.gameState = "gameend";
                    }
                    else if (hp == 1)
                    {
                        hpImage.GetComponent<Image>().sprite = lifeSprites[hp];
                    }
                    else if (hp == 2)
                    {
                        hpImage.GetComponent<Image>().sprite = lifeSprites[hp];
                    }
                    else { hpImage.GetComponent<Image>().sprite = lifeSprites[hp]; }
                }
            }
        }
    }

    private void UpdateItemCount()
    {
        //화살
        if(hasArrows != ItemKeeper.hasArrows)
        {
            arrowText.text = ItemKeeper.hasArrows.ToString();
            hasArrows = ItemKeeper.hasArrows;
        }

        //열쇠
        if(hasKeys != ItemKeeper.hasKeys)
        {
            keyText.text = ItemKeeper.hasKeys.ToString();
            hasKeys = ItemKeeper.hasKeys;
        }
    }

    void InactiveImage()
    {
        mainImage.SetActive(false);
    }

    public void Retry()
    {
        PlayerPrefs.SetInt("PlayerHp", 3);
        SceneManager.LoadScene(retrySceneName);
    }

    //게임 클리어
    public void GameClear()
    {
        mainImage.SetActive(true);
        mainImage.GetComponent<Image>().sprite = gameSprites[1];

        inputPanel.SetActive(false);
        PlayerController.gameState = "gameclear";

        Invoke("GoToTitle", 3f);
    }

    void GoToTitle()
    {
        PlayerPrefs.DeleteKey("LastScene");
        SceneManager.LoadScene("Title");
    }
}
