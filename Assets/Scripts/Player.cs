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

    private float limitTime; // ���� �ð�
    private float time; // ���� ���� �ð�
    [SerializeField] private TextMeshProUGUI timeTxt;

    // Start is called before the first frame update
    void Start()
    {
        board = GameObject.FindGameObjectWithTag("Board").GetComponent<Board>();
        limitTime = GameManager.instance.LimitTime;
        time = limitTime;
        SettingNextNumber();
    }

    // Update is called once per frame
    void Update()
    {
        // ���� �ڽ��� ������ ���
        if(GameManager.instance.CurPlayer == playerType) {
            time -= Time.deltaTime;
            if(time <= 0) {
                time = 0;
                GameManager.instance.ChangeTurn();
                time = limitTime;
            }

            filter.SetActive(false);
            int idx = board.PrintApplePreview();
            if (Input.GetMouseButtonDown(0) && !board.IsChecked[idx]) {
                board.PlaceApple(idx, playerType, nextNum);
                SettingNextNumber();
                time = limitTime;
            }

            timeTxt.text = time.ToString("N2");
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
