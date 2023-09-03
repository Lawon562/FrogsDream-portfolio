/**
 * Creation Day : 2023.04.15
 * Script Name : BallGenerator
 * Author : Jaemin Lee
 * 
 * Description
 *  - 진짜 공과 가짜 공을 NavMesh 내의 랜덤 좌표로 복사하여 이동시키는 스크립트
 */

using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.AI;


/// <summary>
/// 진짜 공과 가짜 공을 NavMesh 내의 랜덤 좌표로 복사하여 이동시킴.
/// </summary>
public class BallGenerator: MonoBehaviour
{
    [SerializeField] private GameObject RealBall;   // 진짜 공 오브젝트 저장 변수
    [SerializeField] private GameObject FakeBall;   // 가짜 공 오브젝트 저장 변수
    [SerializeField] private int MaxBallCount;      // 라운드의 공 최대 개수
    [SerializeField] private Transform Ground;       // 바닥의 가로, 세로 길이를 구하기 위한 렌더러 컴포넌트

    private List<bool> balls;   // 공의 진짜, 가짜 여부를 bool타입으로 저장한 리스트
                                // 공 복제 시 이 리스트를 기초로 진짜, 가짜 공을 복제함.

    private float groundStartX;
    private float groundEndX;
    private float groundStartZ;
    private float groundEndZ;

    /**
     * Start에서 실행하는 작업
     * - MaxBallCount(공 최대 개수) 검사
     * - balls 리스트 세팅(MaxBallCount만큼 bool 값 설정)
     * - 0.2초 뒤, 볼을 랜덤 좌표로 워프시키는 BallGenerate 실행
     */
    void Start()
    {
        MaxBallCount = GameData.maxBallCount;
        CheckMaxBallCount();
        SetBallList();
        
        Invoke("BallGenerate", 0.2f);
        /* 0.2초 뒤 실행시키는 이유 */
        // NavMesh가 Bake되고 NavMeshObstacle이 적용됩니다. 이 사이에 BallRandomPosition이 실행되면
        // Ball이 오브젝트끼리 중첩된 영역에 워프될 가능성이 있습니다.
        // 따라서, NavMesh에 NavMeshObstacle이 완전히 적용되는 0.2초 뒤 공의 랜덤좌표 설정을 시작하도록 하였습니다.
    }


    /// <summary>
    /// MaxBallCount 변수의 값이 만약 0이라면(초기화되지 않았다면) default 값을 30으로 설정합니다.
    /// </summary>
    private void CheckMaxBallCount()
    {
        if (MaxBallCount == 0) MaxBallCount = 30;
    }


    /// <summary>
    /// balls 리스트를 MaxBallCount만큼 false로 초기화하고, 
    /// 0~MaxBallCount-1에서 랜덤한 값을 뽑아 해당 값의 인덱스를 true로 설정합니다.
    /// </summary>
    private void SetBallList()
    {
        balls = new List<bool>();
        for (int i = 0; i < MaxBallCount; i++) balls.Add(false);
        balls[Random.Range(0, MaxBallCount - 1)] = true;
    }

    /// <summary>
    /// Ground의 가로, 세로 값을 groundWidth, groundDepth 변수에 저장합니다.
    /// </summary>
    private void GetGroundArea()
    {
        /* 다른 코드로 뺄 것*/
        //Vector3 plainSize = Ground.bounds.size;
        //groundWidth = plainSize.x;
        //groundDepth = plainSize.z; // 깊이 길이

        Vector3 objectScale = Ground.transform.localScale;
        Vector3 objectPosition = Ground.transform.localPosition;
        groundStartX = objectPosition.x - (objectScale.x / 2f);
        groundEndX = objectPosition.x + (objectScale.x / 2f);
        groundStartZ = objectPosition.z - (objectScale.z / 2f);
        groundEndZ = objectPosition.z + (objectScale.z / 2f);

        //groundWidth = 400f ;
        //groundDepth = 60f;
    }


    /// <summary>
    /// 전달받은 ball 값을 기반으로 진짜 / 가짜 공을 복제하여 반환합니다.
    /// </summary>
    /// <param name="ball">복사할 객체의 진짜(true)/가짜(false) 여부</param>
    /// <returns>복사된 공 객체</returns>
    private GameObject CopyObject(bool ball)
    {
        GameObject copy;
        if (ball)
        {
            copy = Instantiate(RealBall);
        }
        else
        {
            copy = Instantiate(FakeBall);
        }
        return copy;
    }


    /// <summary>
    /// 바닥의 가로, 세로 길이를 참조하여 랜덤 Vector3를 반환합니다.
    /// </summary>
    /// <returns></returns>
    private Vector3 GetRandomPosition()
    {
        //float x = groundWidth;
        //float z = groundDepth / 2;
        //Debug.Log($"{x} {z}");
        //return new Vector3(
        //    Random.Range(20f, x),        // x
        //    Random.Range(0f, 2f),                                           // y
        //    Random.Range((z * -1), z)         // z
        //);
        return new Vector3(
            Random.Range(groundStartX, groundEndX),        // x
            Random.Range(0f, 2f),                                           // y
            Random.Range(groundStartZ, groundEndZ)         // z
        );

        // x : 바닥의 가로 중심점을 기준으로 -좌표부터 +좌표까지의 랜덤 값을 반환합니다.
        // y : magic number입니다. 좌표를 0으로 설정해도 무관하였으나, 계단 같은 높은 바닥에 적용될 수 있도록 랜덤값으로 설정하였습니다.
        // z : 바닥의 세로 중심점을 기준으로 -좌표부터 +좌표까지의 랜덤 값을 반환합니다.

        // 예시 (50, 60)짜리 바닥이 있을 때,
        // x : -25 ~ +25 까지의 랜덤 값
        // y :   0 ~ +10 까지의 랜덤 값
        // z : -30 ~ +30 까지의 랜덤 값
    }


    /// <summary>
    /// 객체를 받아 bake된 NavMesh 영역 내에서 워프시킵니다.
    /// </summary>
    /// <param name="obj">워프시킬 객체</param>
    private void WarpInNavMeshArea(GameObject obj)
    {
        // 바닥 가로세로 길이를 이용한 랜덤 좌표를 warpPosition에 저장합니다.
        Vector3 warpPosition = GetRandomPosition();

        // NavMesh.SamplePosition : NavMesh 영역 위에서 특정 위치를 샘플링(추출)하는 기능입니다.
        // 주어진 warpPosition이 NevMesh 영역 위에 있다면 true를, 아니면 false를 반환합니다.
        if (NavMesh.SamplePosition(warpPosition, out NavMeshHit hit, 10.0f, NavMesh.AllAreas))
        {
            obj.GetComponent<NavMeshAgent>().Warp(hit.position); // 유효한 NavMesh 위치로 에이전트 이동
            obj.GetComponent<NavMeshAgent>().enabled = false;    // NavMeshAgent 끄기
            
            /* NavMeshAgent 끄는 이유*/
            // NavMesh가 켜져있으면 공이 플레이어에게 붙어도 정상적인 위치로 올라가지 않고 바닥 표면에 붙어있게 됩니다.
            // 따라서, 공이 복제 및 워프된 이후 NavMeshAgent를 꺼서 공의 이동을 자유롭게 해야 합니다.
        }
    }

    

    /// <summary>
    /// 진짜/가짜 공을 복제한 후 NavMesh 영역의 랜덤 좌표로 워프시킵니다.
    /// </summary>
    private void BallGenerate()
    {
        GetGroundArea();

        for (int i = 0; i < MaxBallCount; i++)
        {
            GameObject copy = CopyObject(balls[i]);
            WarpInNavMeshArea(copy);
        }
    }

}
