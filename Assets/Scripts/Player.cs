using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Board board;

    // 플레이어 넘버링
    [SerializeField] PlayerType playerType;

    // 플레이어의 사과에 부여될 다음 숫자
    private int nextNum;
    public TextMeshProUGUI numberTxt;

    // 턴에 따라 플레이어 프로필 활성화해줄 필터 이미지
    [SerializeField] private GameObject filter;

    // Start is called before the first frame update
    void Start()
    {
        board = GameObject.FindGameObjectWithTag("Board").GetComponent<Board>();
        SettingNextNumber();
    }

    // Update is called once per frame
    void Update()
    {
        // 현재 자신의 차례인 경우
        if(GameManager.instance.CurPlayer == playerType) {
            filter.SetActive(false);
            int idx = board.PrintApplePreview();
            if (Input.GetMouseButtonDown(0) && !board.IsChecked[idx]) {
                board.PlaceApple(idx, playerType, nextNum);
                SettingNextNumber();
            }
        }
        else {
            filter.SetActive(true);
        }
    }

    private void SettingNextNumber() {
        nextNum = GameManager.instance.RandomNumber();
        numberTxt.text = nextNum.ToString();
    }
}
