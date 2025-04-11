using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    private const float xOffset = -4.5f; // x�� ���� ��ǥ
    private const float yOffset = 4.5f; // y�� ���� ��ǥ
    private const float size = 0.5f; // ĭ ������
    private const int lineCnt = 19;

    // ���� ����� ��� ��ǥ�� ��� ������Ʈ�� �̸� �� �� ������
    // ������ ����� ����Ʈ�� ����
    // �÷��̾ ���� Ŭ���ϸ� �ش� ��ġ�� ����� Ȱ��ȭ ��Ű�� ������ �´� ������ �ٲ�
    [SerializeField] private GameObject apple;
    private List<GameObject> apples = new List<GameObject>();
    private Color red = new Color(221 / 255f, 41 / 255f, 58 / 255f, 1f);
    private Color green = new Color(169 / 255f, 200 / 255f, 16 / 255f, 1f);

    // ��� �̸� ���⸦ ���� ����
    // ������ ����� �ε����� ��� ������Ʈ�� ����
    // ���� ���콺 ��ġ�� ����� ���̰� ���� ��� ���� ��� ���ְ� ���� ��� ����
    private int lastIdx = -1;
    private GameObject lastApple;

    // ��� ����Ʈ�� ������ ������ ũ��� �����Ͽ� ����� Ȱ��ȭ �ƴ��� Ȯ��
    private bool[] isChecked = new bool[(int)Mathf.Pow(lineCnt, 2)];

    public bool[] IsChecked { get { return isChecked; } }

    // Start is called before the first frame update
    void Start()
    {
        // ������ ����� ���� �θ� ������Ʈ
        Transform parent = GameObject.Find("AppleCollector").transform;

        // ���� ���� �� 19���� ���� ����, ���η� ����
        // ����� �ö� �������� �� ��ŭ �ݺ��Ͽ� ��ǥ�� ����� ���� �� ��Ȱ��ȭ
        // �������� ������ 19�� ����
        for(int i = 0; i < Mathf.Pow(lineCnt, 2); i++) {
            GameObject tmp = Instantiate(apple, parent);
            float x = xOffset + ((i % lineCnt) * size);
            float y = yOffset - ((i / lineCnt) * size);
            tmp.transform.position = new Vector2(x, y);
            tmp.SetActive(false);
            isChecked[i] = false;
            apples.Add(tmp);
        }

        GameManager.instance.ChangeTurn(); // NONE���� �ʱ�ȭ�� �÷��̾� ������ PLAYER1�� ����
    }

    /// <summary>
    /// ��� �̸����� ���
    /// </summary>
    /// <returns>���콺�� ���� �� �ȿ� �ִ� ��� ���콺 ��ġ�� �ε��� ��ȯ, ���콺�� ���� �� �ۿ� �ִ� ��� -1 ��ȯ</returns>
    public int PrintApplePreview() {
        int idx = -1;

        // ���콺�� ��ġ ��ǥ
        Vector2 mPos = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));

        // ���콺�� ���� �� �ȿ� �ִ� ���
        if (-4.75f < mPos.x && mPos.x < 4.75f &&
            -4.75f < mPos.y && mPos.y < 4.75f) {
            idx = IndexToPosition(mPos.x, mPos.y); // ���� ���콺�� ��ǥ�� ������ �ε��� ����

            // ���ο� �ε������ ������ �ѵ� ����� �������
            if (idx != lastIdx) {
                if (lastApple != null) {
                    lastApple.SetActive(false);
                }
                // ����� ������ ���� ĭ�� ���
                if (!isChecked[idx]) {
                    apples[idx].SetActive(true); // ���� �ε����� ��� ����
                                                 // ���� �÷��̾ ���� �� ����
                    lastIdx = idx; // ������ �ε��� ����
                    lastApple = apples[idx]; // ���������� ���� ��� ����
                }
            }
        }
        else { // ���콺�� ���� �� ������ ����� �ʱ�ȭ
            lastIdx = -1;
            if (lastApple != null) {
                lastApple.SetActive(false);
                lastApple = null;
            }
        }

        return idx;
    }

    /// <summary>
    /// ��ǥ���� ���� ���� �ε��� ������ ��ȯ
    /// �޾ƿ� ��ǥ�� 
    /// -4.75 < x && x < 4.75
    /// -4.75 < y && y < 4.75
    /// </summary>
    /// <param name="x">x��ǥ</param>
    /// <param name="y">y��ǥ</param>
    private int IndexToPosition(float x, float y) {
        // ���������� ���� ��ǥ������ �Ÿ�
        float xDist = x - (xOffset); 
        float yDist = (yOffset) - y;

        // x, y ���� �� ��° ĭ�� �ִ��� ���
        int xCnt = (int)(xDist / size);
        int yCnt = (int)(yDist / size);

        // ����� ĭ�� �ƴ� �������� �����ϱ� ������
        // ���������� 0.25�������� ���Ͽ� ĭ���� 1������ �Ǵ�
        if(xDist % size > 0.25f) {
            xCnt++;
        }
        if(yDist % size > 0.25f) {
            yCnt++;
        }

        return yCnt * 19 + xCnt;
    }

    /// <summary>
    /// ���� �� ���� ����� ���� ���
    /// </summary>
    /// <param name="idx">����� ���� �������� �ε���</param>
    /// <param name="player">�÷��̾� ����</param>
    /// <param name="appleNum">����� �ο��� ����</param>
    public void PlaceApple(int idx, PlayerType player, int appleNum) {
        lastIdx = -1;
        lastApple = null;
        isChecked[idx] = true;

        int ownerNum = (int)player; // ���� �÷��̾� ���� ����
        Color curColor = Color.white; // ����� ���� ��
        if(ownerNum == 1) { // �÷��̾� 1�� ������
            curColor = red;
        } else if(ownerNum == 2){ // �÷��̾� 2�� �ʷϻ�
            curColor = green;
        }

        apples[idx].GetComponent<Apple>().Setting(appleNum, (PlayerType)ownerNum, curColor); // ��� �ʱ�ȭ �Լ� ȣ��
        CheckWin(apples[idx].GetComponent<Apple>(), idx);
        GameManager.instance.ChangeTurn();
    }

    /// <summary>
    /// �¸� ���� ���
    /// ����� �ǿ� ���� ���� �ش� ����� �������� �� ������ �����Ͽ� ��� �Ǵ�
    /// </summary>
    /// <param name="pivot">���������� ���� ���</param>
    /// <param name="idx">����� ���� ���� �ε���</param>
    private void CheckWin(Apple pivot, int idx) {
        // Ž�� ���� �迭
        // ��, ���, ��, ����
        int[,] dirs = { { 0, 1 }, { 1, 1 }, { 1, 0 }, { 1, -1 } };

        // ���� �迭�� ��ȸ�ϸ� �� ����� �� �ݴ� ������ ���
        for(int i = 0; i < dirs.GetLength(0); i++) {
            int dirR = dirs[i, 0] - (dirs[i, 1] * 19); // ������ Ÿ��
            int dirL = (dirs[i, 1] * 19) - dirs[i, 0]; // ������ Ÿ��

            int cnt = 1; // �̾��� ����� ����(������ �Ǵ� ����� �����ϱ⿡ �⺻�� 1)
            int sum = CheckApple(dirR, idx, pivot, ref cnt) + CheckApple(dirL, idx, pivot, ref cnt) - pivot.Number; // �̾��� ����� ���� �հ� ������ ���
            if(cnt == 5 && sum == 10) {
                Debug.Log($"Player{pivot.Owner} Win!!");
            }
        }
    }

    /// <summary>
    /// Ư�� �������� �̾��� ������ ����� ������ ������ ���� ��ȯ
    /// ����Լ��μ� ����Ͽ� �̾��� ����� ���ܱ��� ����
    /// ref�� Ȱ���Ͽ� ��� ���� ���
    /// </summary>
    /// <param name="dir">Ž�� ����</param>
    /// <param name="pivotIdx">������ �ε���</param>
    /// <param name="pivot">������ ��� ��ü</param>
    /// <param name="cnt">������ ����� ����</param>
    /// <returns></returns>
    private int CheckApple(int dir, int pivotIdx, Apple pivot, ref int cnt) {
        int sum = 0;
        // ������ ������ ���� ĭ�� ����� �����ϰ� �� ����� ������ ����� ���� ������ ���
        if (isChecked[pivotIdx + dir] && pivot.Owner == apples[pivotIdx + dir].GetComponent<Apple>().Owner) {
            // �ش� ����� �Ű������� �Ѱ� �Լ� �ٽ� ȣ���Ͽ� �ش� ����� ���ڸ� ��ȯ
            sum += CheckApple(dir, pivotIdx + dir, apples[pivotIdx + dir].GetComponent<Apple>(), ref cnt);
            cnt++; // ��� ���� ����
        }
        sum += pivot.Number; // �ڱ� �ڽ��� ���� ���� �� ��ȯ

        return sum;
    }
}
