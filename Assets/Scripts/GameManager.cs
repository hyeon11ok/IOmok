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
    private PlayerType curPlayer = PlayerType.NONE;

    private float limitTime = 15;

    public PlayerType CurPlayer { get { return curPlayer; } }
    public float LimitTime { get {  return limitTime; } }

    private void Awake() {
        if(instance == null) {
            instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// �� ���� ���
    /// �÷��̾� 1 -> �÷��̾� 2 / �÷��̾� 2 -> �÷��̾� 1
    /// ���� ���� ������ NONE ����, ���� ���Խ� ȣ���ϸ� ���� �÷��̾ �÷��̾� 1�� ������
    /// </summary>
    public void ChangeTurn() {
        switch (curPlayer) {
            case PlayerType.NONE: {
                    curPlayer = PlayerType.PLAYER1;
                    break;
                }
            case PlayerType.PLAYER1: {
                    curPlayer = PlayerType.PLAYER2;
                    break;
                }
            case PlayerType.PLAYER2: {
                    curPlayer = PlayerType.PLAYER1;
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
