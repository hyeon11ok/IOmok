using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Board board;

    // �÷��̾� �ѹ���
    [SerializeField] PlayerType playerType;

    // �÷��̾��� ����� �ο��� ���� ����
    private int nextNum;
    public TextMeshProUGUI numberTxt;

    // �Ͽ� ���� �÷��̾� ������ Ȱ��ȭ���� ���� �̹���
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
        // ���� �ڽ��� ������ ���
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
