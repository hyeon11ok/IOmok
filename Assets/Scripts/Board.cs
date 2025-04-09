using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour
{
    private const float xOffset = -4.5f; // x축 시작 좌표
    private const float yOffset = 4.5f; // y축 시작 좌표
    private const float size = 0.5f; // 칸 사이즈
    private const int lineCnt = 19;

    // 게임 실행시 모든 좌표에 사과 오브젝트를 미리 한 개 만들어둠
    // 생성된 사과는 리스트로 관리
    // 플레이어가 판을 클릭하면 해당 위치의 사과를 활성화 시키고 유저에 맞는 색으로 바꿈
    public GameObject apple;
    private List<GameObject> apples = new List<GameObject>();
    private Color red = new Color(221 / 255, 41 / 255, 58 / 255);
    private Color green = new Color(169 / 255, 200 / 255, 16 / 255);

    // Start is called before the first frame update
    void Start()
    {
        // 생성된 사과를 담을 부모 오브젝트
        Transform parent = GameObject.Find("AppleCollector").transform;

        // 오목 판은 총 19개의 선이 가로, 세로로 존재
        // 사과가 올라갈 교차점의 수 만큼 반복하여 좌표를 계산해 생성 후 비활성화
        // 교차점의 개수는 19의 제곱
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
