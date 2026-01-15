using UnityEngine;

public class Door : MonoBehaviour
{
    //식별에 사용하기 위한 값, 배치 데이터를 저장하기 위해.
    public int arrangeld = 0;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(ItemKeeper.hasKeys > 0)
            {
                ItemKeeper.hasKeys--;
                Destroy(this.gameObject);

                SaveDataManager.SetArrangeld(arrangeld, gameObject.tag);
            }
        }
    }
}
