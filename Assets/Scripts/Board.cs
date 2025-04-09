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
    private Color red = new Color(221 / 255, 41 / 255, 58 / 255);
    private Color green = new Color(169 / 255, 200 / 255, 16 / 255);

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
            apples.Add(tmp);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
