using UnityEngine;

[System.Serializable]

public class SaveData
{
    public int arrangeId = 0;       //��ġID
    public string objTag = "";      //��ġ�� ������Ʈ �±�
}

[System.Serializable]
public class SaveDataList
{
    public SaveData[] saveDatas;        //SaveData �迭
}

public class SaveDataManager : MonoBehaviour
{
    public static SaveDataList arrangeDataList;

    private void Start()
    {
        //현재 씬 이름 가져오기
        string stageName = PlayerPrefs.GetString("LastScene");

        //SaveDataList 초기화 (현재 씬의 저장된 데이터로)
        arrangeDataList = new SaveDataList();
        arrangeDataList.saveDatas = new SaveData[] { };

        //저장된 데이터 불러오기
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
                        if(door != null && door.arrangeld == savedata.arrangeId)
                        {
                            Destroy(door.gameObject);      //arrangeId�� ���ٸ� ����
                        }
                    }
                    else if(objTag == "ItemBox")
                    {
                        ItemBox box = obj.GetComponent<ItemBox>();
                        if(box != null && box.arrangeld == savedata.arrangeId)
                        {
                            box.isClosed = false;
                            box.GetComponent<SpriteRenderer>().sprite = box.openImage;      //Id�� ���ٸ� ���� ���ڷ�
                        }
                    }
                    else if (objTag == "Item")
                    {
                        ItemData item = obj.GetComponent<ItemData>();
                        if(item != null && item.arrageId == savedata.arrangeId)
                        {
                            Destroy(item.gameObject);
                        }
                    }

                    else if( objTag == "Enemy")
                    {
                        EnemyController enemy = obj.GetComponent<EnemyController>();
                        Enemy1Controller enemy1 = obj.GetComponent<Enemy1Controller>();
                        if((enemy != null && enemy.arrangeId == savedata.arrangeId) || 
                           (enemy1 != null && enemy1.arrangeId == savedata.arrangeId))
                        {
                            Destroy(obj);
                        }
                    }
                }
            }
        }

    }


    // ��ġ ID ����
    public static void SetArrangeld(int  arrangeld, string objTag)
    {
        if(arrangeld == 0 || objTag == "")
        {
            //������� �ʴ´�
            return;
        }

        //�߰��ϱ� �迭�� ũ�⸦ �ϳ� �� ũ��
        // arrangeDataList가 null이면 초기화
        if (arrangeDataList == null)
        {
            arrangeDataList = new SaveDataList();
            arrangeDataList.saveDatas = new SaveData[] { };
        }

        // 중복 체크: 같은 arrangeId와 objTag가 이미 있는지 확인
        if (arrangeDataList.saveDatas != null)
        {
            for (int i = 0; i < arrangeDataList.saveDatas.Length; i++)
            {
                if (arrangeDataList.saveDatas[i].arrangeId == arrangeld && 
                    arrangeDataList.saveDatas[i].objTag == objTag)
                {
                    // 이미 존재하면 추가하지 않음
                    return;
                }
            }
        }

        SaveData[] newSaveDatas = new SaveData[arrangeDataList.saveDatas.Length + 1];

        for(int i = 0; i < arrangeDataList.saveDatas.Length; i++)
        {
            newSaveDatas[i] = arrangeDataList.saveDatas[i];
        }
        //SaveData �����
        SaveData savedata = new SaveData();
        savedata.arrangeId = arrangeld;     //Id ���
        savedata.objTag = objTag;           //tag ���

        //SaveData �߰�
        //newSaveDatas[arrangeDataList.saveDatas.Length + 1] = savedata;
        newSaveDatas[arrangeDataList.saveDatas.Length] = savedata;
        arrangeDataList.saveDatas = newSaveDatas;

    }

    // ��ϵ� ������ ����
    public static void SaveArrangeData(string stageName)
    {
        if (arrangeDataList != null && arrangeDataList.saveDatas != null && stageName != "")
        {
            string saveJson= JsonUtility.ToJson(arrangeDataList);

            PlayerPrefs.SetString(stageName, saveJson);
            PlayerPrefs.Save(); // PlayerPrefs 저장 보장
        }
    }

    
}
