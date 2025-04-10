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
    public GameObject apple;
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
    }

    // Update is called once per frame
    void Update()
    {
        // ���콺�� ��ġ ��ǥ
        Vector2 mPos = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));

        // ���콺�� ���� �� �ȿ� �ִ� ���
        if(-4.75f < mPos.x &&  mPos.x < 4.75f &&
            -4.75f < mPos.y && mPos.y < 4.75f) {
            int idx = IndexToPosition(mPos.x, mPos.y); // ���� ���콺�� ��ǥ�� ������ �ε��� ����

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

            // ���콺 Ŭ���ϸ� ���� �ε����� ����� �� ���� ���´�.
            if(Input.GetMouseButtonDown(0)) {
                lastIdx = -1;
                lastApple = null;
                isChecked[idx] = true;
                apples[idx].GetComponent<SpriteRenderer>().color = red;
            }
        }
        else { // ���콺�� ���� �� ������ ����� �ʱ�ȭ
            lastIdx = -1;
            if(lastApple != null) {
                lastApple.SetActive(false);
                lastApple = null;
            }
        }
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
}
