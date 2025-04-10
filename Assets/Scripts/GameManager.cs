using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // �������� ������ ���� �ּ�, �ִ�
    // ���� ���̵� �������� ���� ���� �����ϰ� �� ����
    private int numMin = 1;
    private int numMax = 3;

    // ���� �÷��̾� ����
    // ���� ���� �� �⺻ ���� NONE
    private Player curPlayer = Player.NONE;

    public Player CurPlayer { get { return curPlayer; } }

    private void Awake() {
        if(instance == null) {
            instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// �� ���� ���
    /// �÷��̾� 1 -> �÷��̾� 2 / �÷��̾� 2 -> �÷��̾� 1
    /// ���� ���� ������ NONE ����, ���� ���Խ� ȣ���ϸ� ���� �÷��̾ �÷��̾� 1�� ������
    /// </summary>
    public void ChangeTurn() {
        switch (curPlayer) {
            case Player.NONE: {
                    curPlayer = Player.PLAYER1;
                    break;
                }
            case Player.PLAYER1: {
                    curPlayer = Player.PLAYER2;
                    break;
                }
            case Player.PLAYER2: {
                    curPlayer = Player.PLAYER1;
                    break;
                }
        }
    }

    /// <summary>
    /// ���� ���� ����
    /// �⺻ �������� 1~3
    /// ���� ���̵� ���� ��� �߰� �� �ִ� ���� ��� ���� ����
    /// </summary>
    /// <returns>������ �ּڰ����� ������ �ִ񰪱����� ������ ����</returns>
    public int RandomNumber() {
        return Random.Range(numMin, numMax + 1);
    }
}
