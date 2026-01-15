using UnityEngine;

[System.Serializable]

public class SaveData
{
    public int arrangeId = 0;       //배치ID
    public string objTag = "";      //배치된 오브젝트 태그
}

[System.Serializable]
public class SaveDataList
{
    public SaveData[] saveDatas;        //SaveData 배열
}

public class SaveDataManager : MonoBehaviour
{
    public static SaveDataList arrangeDataList;

    private void Start()
    {
        //SaveDataList
        arrangeDataList = new SaveDataList();
        arrangeDataList.saveDatas = new SaveData[] { };
        //TLsdlfma qnffjdhrl
        string stageName = PlayerPrefs.GetString("LastScene");

        string data = PlayerPrefs.GetString(stageName);

        if (data != "")
        {
            arrangeDataList = JsonUtility.FromJson<SaveDataList>(data);
            for (int i = 0; i < arrangeDataList.saveDatas.Length; i++)
            {
                SaveData savedata = arrangeDataList.saveDatas[i];
                
                string objTag = savedata.objTag;
                GameObject[] objects = GameObject.FindGameObjectsWithTag(objTag);
                for (int j = 0; j < objects.Length; j++)
                {
                    GameObject obj = objects[j];

                    if (objTag == "Door")
                    {
                        Door door = obj.GetComponent<Door>();
                        if(door.arrangeld == savedata.arrangeId)
                        {
                            Destroy(door);      //arrangeId가 같다면 제거
                        }
                    }
                    else if(objTag == "ItemBox")
                    {
                        ItemBox box = obj.GetComponent<ItemBox>();
                        if(box.arrangeld == savedata.arrangeId)
                        {
                            box.isClosed = false;
                            box.GetComponent<SpriteRenderer>().sprite = box.openImage;      //Id가 같다면 열린 상자로
                        }
                    }
                    else if (objTag == "Item")
                    {
                        ItemData item = obj.GetComponent<ItemData>();
                        if(item.arrageId == savedata.arrangeId)
                        {
                            Destroy(item);
                        }
                    }

                    else if( objTag == "Enemy")
                    {
                        EnemyController enemy = obj.GetComponent<EnemyController>();
                        if(enemy.arrangeId == savedata.arrangeId)
                        {
                            Destroy(obj);
                        }
                    }
                }
            }
        }

    }


    // 배치 ID 설정
    public static void SetArrangeld(int  arrangeld, string objTag)
    {
        if(arrangeld == 0 || objTag == "")
        {
            //기록하지 않는다
            return;
        }

        //추가하기 배열의 크기를 하나 더 크게
        SaveData[] newSaveDatas = new SaveData[arrangeDataList.saveDatas.Length + 1];

        for(int i = 0; i < arrangeDataList.saveDatas.Length; i++)
        {
            newSaveDatas[i] = arrangeDataList.saveDatas[i];
        }
        //SaveData 만들기
        SaveData savedata = new SaveData();
        savedata.arrangeId = arrangeld;     //Id 기록
        savedata.objTag = objTag;           //tag 기록

        //SaveData 추가
        newSaveDatas[arrangeDataList.saveDatas.Length + 1] = savedata;
        arrangeDataList.saveDatas = newSaveDatas;

    }

    // 기록된 데이터 저장
    public static void SaveArrangeData(string stageName)
    {
        if (arrangeDataList.saveDatas != null && stageName != "")
        {
            string saveJson= JsonUtility.ToJson(arrangeDataList);

            PlayerPrefs.SetString(stageName, saveJson);
        }
    }

    
}
