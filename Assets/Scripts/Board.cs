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
    private Color red = new Color(221 / 255f, 41 / 255f, 58 / 255f, 1f);
    private Color green = new Color(169 / 255f, 200 / 255f, 16 / 255f, 1f);

    // 사과 미리 보기를 위한 변수
    // 마지막 사과의 인덱스와 사과 오브젝트를 저장
    // 현재 마우스 위치의 사과와 차이가 있을 경우 이전 사과 꺼주고 현재 사과 켜줌
    private int lastIdx = -1;
    private GameObject lastApple;

    // 사과 리스트의 개수와 동일한 크기로 생성하여 사과가 활성화 됐는지 확인
    private bool[] isChecked = new bool[(int)Mathf.Pow(lineCnt, 2)];

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
            isChecked[i] = false;
            apples.Add(tmp);
        }
    }

    // Update is called once per frame
    void Update()
    {
        // 마우스의 위치 좌표
        Vector2 mPos = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));

        // 마우스가 오목 판 안에 있는 경우
        if(-4.75f < mPos.x &&  mPos.x < 4.75f &&
            -4.75f < mPos.y && mPos.y < 4.75f) {
            int idx = IndexToPosition(mPos.x, mPos.y); // 현재 마우스의 좌표와 인접한 인덱스 저장

            // 새로운 인덱스라면 기존에 켜둔 사과를 꺼줘야함
            if (idx != lastIdx) {
                if (lastApple != null) {
                    lastApple.SetActive(false);
                }
                // 사과가 놓이지 않은 칸인 경우
                if (!isChecked[idx]) {
                    apples[idx].SetActive(true); // 현재 인덱스의 사과 켜줌
                                                 // 현재 플레이어에 따라 색 변경
                    lastIdx = idx; // 마지막 인덱스 저장
                    lastApple = apples[idx]; // 마지막으로 켜준 사과 저장
                }
            }

            // 마우스 클릭하면 현재 인덱스의 사과를 판 위에 놓는다.
            if(Input.GetMouseButtonDown(0)) {
                lastIdx = -1;
                lastApple = null;
                isChecked[idx] = true;
                apples[idx].GetComponent<SpriteRenderer>().color = red;
            }
        }
        else { // 마우스가 오목 판 밖으로 벗어나면 초기화
            lastIdx = -1;
            if(lastApple != null) {
                lastApple.SetActive(false);
                lastApple = null;
            }
        }
    }

    /// <summary>
    /// 좌표값을 오목 판의 인덱스 값으로 반환
    /// 받아올 좌표는 
    /// -4.75 < x && x < 4.75
    /// -4.75 < y && y < 4.75
    /// </summary>
    /// <param name="x">x좌표</param>
    /// <param name="y">y좌표</param>
    private int IndexToPosition(float x, float y) {
        // 시작점부터 지정 좌표까지의 거리
        float xDist = x - (xOffset); 
        float yDist = (yOffset) - y;

        // x, y 각각 몇 번째 칸에 있는지 계산
        int xCnt = (int)(xDist / size);
        int yCnt = (int)(yDist / size);

        // 사과는 칸이 아닌 교차점에 존재하기 때문에
        // 나머지값을 0.25기준으로 비교하여 칸수를 1더할지 판단
        if(xDist % size > 0.25f) {
            xCnt++;
        }
        if(yDist % size > 0.25f) {
            yCnt++;
        }

        return yCnt * 19 + xCnt;
    }
}
