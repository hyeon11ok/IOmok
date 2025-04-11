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
    [SerializeField] private GameObject apple;
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

    public bool[] IsChecked { get { return isChecked; } }

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

        GameManager.instance.ChangeTurn(); // NONE으로 초기화된 플레이어 변수를 PLAYER1로 변경
    }

    /// <summary>
    /// 사과 미리보기 출력
    /// </summary>
    /// <returns>마우스가 오목 판 안에 있는 경우 마우스 위치의 인덱스 반환, 마우스가 오목 판 밖에 있는 경우 -1 반환</returns>
    public int PrintApplePreview() {
        int idx = -1;

        // 마우스의 위치 좌표
        Vector2 mPos = Camera.main.ScreenToWorldPoint(new Vector2(Input.mousePosition.x, Input.mousePosition.y));

        // 마우스가 오목 판 안에 있는 경우
        if (-4.75f < mPos.x && mPos.x < 4.75f &&
            -4.75f < mPos.y && mPos.y < 4.75f) {
            idx = IndexToPosition(mPos.x, mPos.y); // 현재 마우스의 좌표와 인접한 인덱스 저장

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
        }
        else { // 마우스가 오목 판 밖으로 벗어나면 초기화
            lastIdx = -1;
            if (lastApple != null) {
                lastApple.SetActive(false);
                lastApple = null;
            }
        }

        return idx;
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

    /// <summary>
    /// 오목 판 위에 사과를 놓는 기능
    /// </summary>
    /// <param name="idx">사과를 놓을 교차점의 인덱스</param>
    /// <param name="player">플레이어 정보</param>
    /// <param name="appleNum">사과에 부여될 숫자</param>
    public void PlaceApple(int idx, PlayerType player, int appleNum) {
        lastIdx = -1;
        lastApple = null;
        isChecked[idx] = true;

        int ownerNum = (int)player; // 현재 플레이어 정보 저장
        Color curColor = Color.white; // 사과에 입힐 색
        if(ownerNum == 1) { // 플레이어 1은 빨간색
            curColor = red;
        } else if(ownerNum == 2){ // 플레이어 2는 초록색
            curColor = green;
        }

        apples[idx].GetComponent<Apple>().Setting(appleNum, (PlayerType)ownerNum, curColor); // 사과 초기화 함수 호출
        CheckWin(apples[idx].GetComponent<Apple>(), idx);
        GameManager.instance.ChangeTurn();
    }

    /// <summary>
    /// 승리 판정 기능
    /// 사과를 판에 놓는 순간 해당 사과를 기준으로 전 방향을 조사하여 결과 판단
    /// </summary>
    /// <param name="pivot">마지막으로 놓인 사과</param>
    /// <param name="idx">사과가 놓인 곳의 인덱스</param>
    private void CheckWin(Apple pivot, int idx) {
        // 탐색 방향 배열
        // 상, 우상, 우, 좌하
        int[,] dirs = { { 0, 1 }, { 1, 1 }, { 1, 0 }, { 1, -1 } };

        // 방향 배열을 순회하며 각 방향과 그 반대 방향을 계산
        for(int i = 0; i < dirs.GetLength(0); i++) {
            int dirR = dirs[i, 0] - (dirs[i, 1] * 19); // 정방향 타겟
            int dirL = (dirs[i, 1] * 19) - dirs[i, 0]; // 역방향 타겟

            int cnt = 1; // 이어진 사과의 개수(기준이 되는 사과도 포함하기에 기본값 1)
            int sum = CheckApple(dirR, idx, pivot, ref cnt) + CheckApple(dirL, idx, pivot, ref cnt) - pivot.Number; // 이어진 사과의 숫자 합과 개수를 계산
            if(cnt == 5 && sum == 10) {
                Debug.Log($"Player{pivot.Owner} Win!!");
            }
        }
    }

    /// <summary>
    /// 특정 방향으로 이어진 동일한 사과의 갯수와 숫자의 합을 반환
    /// 재귀함수로서 사용하여 이어진 사과의 끝단까지 진행
    /// ref를 활용하여 사과 갯수 기록
    /// </summary>
    /// <param name="dir">탐색 방향</param>
    /// <param name="pivotIdx">시작점 인덱스</param>
    /// <param name="pivot">시작점 사과 객체</param>
    /// <param name="cnt">개수를 기록할 변수</param>
    /// <returns></returns>
    private int CheckApple(int dir, int pivotIdx, Apple pivot, ref int cnt) {
        int sum = 0;
        // 지정된 방향의 다음 칸에 사과가 존재하고 그 사과가 최초의 사과와 같은 종류인 경우
        if (isChecked[pivotIdx + dir] && pivot.Owner == apples[pivotIdx + dir].GetComponent<Apple>().Owner) {
            // 해당 사과를 매개변수로 넘겨 함수 다시 호출하여 해당 사과의 숫자를 반환
            sum += CheckApple(dir, pivotIdx + dir, apples[pivotIdx + dir].GetComponent<Apple>(), ref cnt);
            cnt++; // 사과 개수 증가
        }
        sum += pivot.Number; // 자기 자신의 숫자 누적 후 반환

        return sum;
    }
}
