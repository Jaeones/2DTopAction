using UnityEngine;

public class VirtualPad : MonoBehaviour
{
    public float MaxLength = 70; //탭이 움직이는 최대 거리
    public bool is4DPad = false; //상하좌우 움직임 여부
    GameObject player;
    Vector2 defPos;         //탭의 초기좌표
    Vector2 downPos;        //터치 위치

    private void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        defPos = GetComponent<RectTransform>().localPosition;
    }

    // 다운 이벤트
    public void PadDown()
    {
        downPos = Input.mousePosition;
    }

    public void PadDrag()
    {
        Vector2 mousePos = Input.mousePosition;     
        //마우스 다운 위치로부터 이동거리
        Vector2 newTapPos = mousePos - downPos;

        if(is4DPad == false)
        {
            newTapPos.y = 0;
        }

        //이동 벡터 계산
        Vector2 axis = newTapPos.normalized;
        //두 점의 거리
        float len = Vector2.Distance(defPos, newTapPos);

        if(len > MaxLength)
        {
            //최대 거리를 넘겼을 경우 최대 좌표로 설정
            newTapPos.x = axis.x * MaxLength;
            newTapPos.y = axis.y * MaxLength;
        }

        //탭 이동 처리
        GetComponent<RectTransform>().localPosition = newTapPos;
        PlayerController plct = player.GetComponent<PlayerController>();

        plct.SetAxis(axis.x, axis.y);
    }

    public void PadUp()
    {
        //탭 위치 초기화
        GetComponent<RectTransform>().localPosition = defPos;
        //플레이어 정지
        PlayerController plct = player.GetComponent<PlayerController>();
        plct.SetAxis(0, 0);
    }

    //공격
    public void Attack()
    {
        ArrowShot shot = player.GetComponent<ArrowShot>();

        shot.Attack();
    }
}
