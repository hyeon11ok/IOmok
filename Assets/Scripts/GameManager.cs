using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    // 랜덤으로 생성될 값의 최소, 최댓값
    // 추후 난이도 설정으로 값을 조정 가능하게 할 예정
    private int numMin = 1;
    private int numMax = 3;

    // 현재 플레이어 저장
    // 게임 시작 전 기본 값은 NONE
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
    /// 턴 변경 기능
    /// 플레이어 1 -> 플레이어 2 / 플레이어 2 -> 플레이어 1
    /// 게임 시작 전에는 NONE 상태, 게임 진입시 호출하면 현재 플레이어가 플레이어 1로 설정됨
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
    /// 랜덤 숫자 생성
    /// 기본 설정값은 1~3
    /// 추후 난이도 설정 기능 추가 후 최댓값 조정 기능 개발 예정
    /// </summary>
    /// <returns>설정된 최솟값부터 설정된 최댓값까지의 랜덤한 숫자</returns>
    public int RandomNumber() {
        return Random.Range(numMin, numMax + 1);
    }
}
