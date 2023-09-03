using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField] private GameObject Enemy;  // 적 객체
    [SerializeField] private int MaxCount;      // 라운드의 적 최대 개수
    [SerializeField] private Transform Ground;       // 바닥의 가로, 세로 길이를 구하기 위한 렌더러 컴포넌트


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
        CheckMaxCount();
        MaxCount = GameData.maxEnemy;
        Invoke("EnemyGenerate", 0.2f);
        /* 0.2초 뒤 실행시키는 이유 */
        // NavMesh가 Bake되고 NavMeshObstacle이 적용됩니다. 이 사이에 BallRandomPosition이 실행되면
        // Ball이 오브젝트끼리 중첩된 영역에 워프될 가능성이 있습니다.
        // 따라서, NavMesh에 NavMeshObstacle이 완전히 적용되는 0.2초 뒤 공의 랜덤좌표 설정을 시작하도록 하였습니다.
    }

    /// <summary>
    /// MaxCount 변수의 값이 만약 0이라면(초기화되지 않았다면) default 값을 30으로 설정합니다.
    /// </summary>
    private void CheckMaxCount()
    {
        if (MaxCount == 0) MaxCount = 30;
    }

    /// <summary>
    /// Ground의 가로, 세로 값을 groundWidth, groundDepth 변수에 저장합니다.
    /// </summary>
    private void GetGroundArea()
    {
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
    private GameObject CopyObject()
    {
        GameObject copy = Instantiate(Enemy);
        return copy;
    }

    /// <summary>
    /// 바닥의 가로, 세로 길이를 참조하여 랜덤 Vector3를 반환합니다.
    /// </summary>
    /// <returns></returns>
    private Vector3 GetRandomPosition()
    {
        return new Vector3(
            Random.Range(groundStartX, groundEndX),        // x
            (Random.Range(0,2) == 0) ? 0 : -3,                                            // y
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
            obj.GetComponent<EnemyPathFinder>().enabled = true;
            obj.GetComponent<RandomJump>().enabled = true;
        }
    }

    /// <summary>
    /// 진짜/가짜 공을 복제한 후 NavMesh 영역의 랜덤 좌표로 워프시킵니다.
    /// </summary>
    private void EnemyGenerate()
    {
        GetGroundArea();

        for (int i = 0; i < MaxCount; i++)
        {
            GameObject copy = CopyObject();
            WarpInNavMeshArea(copy);
        }
    }

}
