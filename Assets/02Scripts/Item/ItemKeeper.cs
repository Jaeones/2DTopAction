using UnityEngine;

public class ItemKeeper : MonoBehaviour
{

    public static int hasKeys = 0;
    public static int hasArrows = 10;


    private void Start()
    {
        hasKeys = PlayerPrefs.GetInt("Keys");
        hasArrows = PlayerPrefs.GetInt("Arrows");
    }

    public static void SaveItem()
    {
        PlayerPrefs.SetInt("Keys",hasKeys);
        PlayerPrefs.SetInt("Arrows", hasArrows);
    }
}
