using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// Enemy의 순찰 상태 (IDLE: EnemyStateMachine 생성, PATROL: 순찰 중, TRACKING: 타겟 발견 및 추적, FAILED: 타겟 놓침)
/// </summary>
public enum ENEMY_STATE { IDLE, PATROL, TRACKING, FAILED, KILLED }
public class EnemyPathFinder : MonoBehaviour
{
    private Transform TargetObject;        // Enemy가 따라다닐 Object의 Transform을 저장합니다.
    [SerializeField] private Transform[] PatrolPosition;    // Enemy가 Patrol 중에 반드시 들러야 할 위치 배열입니다.
    [SerializeField] private float recognitionDistance;     // Enemy가 인식할 수 있는 최대 거리를 저장

    //[SerializeField] private GameObject PatrolSet;       // 순찰 구역들이 자식 오브젝트로 있는 오브젝트 받아오기
    private GameObject PatrolSet;
    Animator ani;

    struct DistanceOfPatrolPos
    {
        public Transform transform;
        public float distance;

        public DistanceOfPatrolPos(Transform _transform, float _distance)
        {
            this.transform= _transform;
            this.distance= _distance;
        }
    }

    private NavMeshAgent agent;                 // 순찰 시 PathFind에 사용될 NavMeshAgent 컴포넌트 변수
    private ENEMY_STATE enemyState;             // Enemy의 순찰 상태를 저장할 열거형 변수
    private int patrolPositionNumber = 0;       // 패트롤해야하는 Position이 몇 번째인지 저장하는 변수
    private float actionTimer = 0f;             // 유한 상태를 바꿀 때 사용하는 타이머 변수

    /**
     * Start에서 실행하는 작업
     *  - enemyState를 초기화(IDLE)
     *  - NavMeshAgent component 연결
     */
    void Start()
    {
        try
        {
            TargetObject = GameObject.Find("Player").transform;
            PatrolSet = GameObject.Find("PatrolSet");
        }
        catch
        {
        }
        
        enemyState = ENEMY_STATE.IDLE;
        agent = GetComponent<NavMeshAgent>();
        if(PatrolPosition.Length == 0)
        {
            // 순찰 구역 개수 지정(3~4개)
            int currentPatrolPosCount = Random.Range(3,5);

            // 가장 가까운 순찰구역 찾아 리스트에 추가
            /**
             * 순찰 구역의 부모 오브젝트로부터 자식 오브젝트(순찰 구역들)을 받아온다.
             * 현재 객체로부터 순찰 구역 오브젝트까지의 거리를 구한다.
             * 계산해서 나온 거리와, 이전 거리를 비교하여 이전 거리보다 더 작으면 거리가 더 적은 순찰구역 오브젝트를 저장한다.
             *   - 만약, 큐에 posCount만큼 오브젝트가 들어가있는 상황이라면, 큐의 맨 앞 오브젝트를 꺼내고 저장한다.
             */
            List<DistanceOfPatrolPos> patrolListP = new List<DistanceOfPatrolPos>();
            List<DistanceOfPatrolPos> patrolListN = new List<DistanceOfPatrolPos>();
            List<DistanceOfPatrolPos> patrolList = new List<DistanceOfPatrolPos>();

            int count = PatrolSet.transform.childCount;


            for (int idx = 0; idx < count; idx++)
            {
                Transform childTransform = PatrolSet.transform.GetChild(idx);
                float distance = Vector3.Distance(this.transform.position, childTransform.position);
                if (childTransform.position.y < 0)
                    patrolListN.Add(new DistanceOfPatrolPos(childTransform, distance));
                else
                    patrolListP.Add(new DistanceOfPatrolPos(childTransform, distance));
            }
            if (this.transform.position.y < 0)
                patrolList = patrolListN;
            else
                patrolList = patrolListP;


            PatrolPosition = new Transform[currentPatrolPosCount];
            patrolList.Sort((x, y) => x.distance.CompareTo(y.distance));
            for (int idx = 0; idx < currentPatrolPosCount; idx++)
            {
                PatrolPosition[idx] = patrolList[idx].transform;
            }
        }
    }


    /// <summary>
    /// 현재 객체에서 Target 객체까지 거리를 반환하는 함수입니다.
    /// </summary>
    private float GetDistance(Transform Target)
    {
        try
        {
            return Vector3.Distance(this.transform.position, TargetObject.position);
        }
        catch
        {
            return 0f;
        }
    }


    /// <summary>
    /// 두 객체 간 거리를 비교하여 ENEMY의 최대사거리보다 짧으면 ENEMY_STATE가 CHASE인지, PATROL인지 반환하는 함수입니다.
    /// </summary>
    private ENEMY_STATE CheckDistance(Transform Target)
    {
        // TargetObject와 현재 Object의 거리가 인식 최대 거리(recognitionDistance)보다 작으면
        return GetDistance(Target) < recognitionDistance ?
            ENEMY_STATE.TRACKING :  // ENEMYSTATE.TRACKING 반환
            ENEMY_STATE.PATROL;     // 그게 아니라면, ENEMY_STATE.PATROL 반환
    }


    /// <summary>
    /// checkDistance로 알아낸 ENEMY_STATE에 따라, TRACKING이면 TargetObject를 추적하고 
    /// 그렇지 않으면 경로를 초기화 후 FAILED로 바꾼다.
    /// </summary>
    private void Tracking()
    {
        try
        {
            if (CheckDistance(TargetObject) == ENEMY_STATE.TRACKING)
            {
                // TargetObject의 위치로 목적지 설정
                agent.SetDestination(TargetObject.position);
            }
            else
            {
                // 목적지 초기화 후 ENEMY_STATE를 FAILED로 설정
                agent.ResetPath();
                enemyState = ENEMY_STATE.FAILED;
            }
        }
        catch
        {

        }
        
    }


    /// <summary>
    /// actionTimer로 제한 시간까지 기다린 후, 전달받은 ENEMY_STATE로 현재 액션 상태를 변경해주는 함수입니다.
    /// </summary>
    private void ChangeEnemyState(float limitTime, ENEMY_STATE state)
    {
        actionTimer += Time.deltaTime;
        if (actionTimer > limitTime)
        {
            actionTimer = 0f;
            enemyState = state;
        }
    }


    /// <summary>
    /// agent가 정해진 목적지를 순찰하는 함수
    /// </summary>
    private void Patrol()
    {
        // 몇 번째 위치로 Patrol할지 설정한다.
        agent.SetDestination(PatrolPosition[patrolPositionNumber].position);

        // 현재 객체와 Patrol할 객체의 위치간 거리를 구한다.
        float posDistance = Vector3.Distance(this.transform.position, PatrolPosition[patrolPositionNumber].position);

        // 거리가 1보다 작으면, PatrolPositionNumber를 1씩 올리는데,
        // 만약 PatrolPosition배열의 길이보다 길어지면 다시 0으로 설정한다.
        if (posDistance < 2f) patrolPositionNumber++;
        if (patrolPositionNumber == PatrolPosition.Length) patrolPositionNumber = 0;

        // Enemy의 상태를 enemyState에 재저장한다.
        enemyState = CheckDistance(TargetObject);
    }

    public void Killed()
    {
        enemyState = ENEMY_STATE.KILLED;
    }
    /// <summary>
    /// Enemy의 상태에 따라 행동을 바꿔주는 함수
    /// </summary>
    private void EnemyStateMachine()
    {
        switch (enemyState)
        {
            case ENEMY_STATE.IDLE:
                // 3초 뒤 PATROL 실행
                ChangeEnemyState(3f, ENEMY_STATE.PATROL);
                break;
            case ENEMY_STATE.PATROL:
                // PATROL 실행
                Patrol();
                break;
            case ENEMY_STATE.TRACKING:
                Tracking();
                break;
            case ENEMY_STATE.FAILED:
                // 추격 실패 상태에서 Target과 거리를 계산하여 만약 Target 인식 거리 안에 들어오면 다시 추격모드로 전환
                enemyState = GetDistance(TargetObject) < recognitionDistance ? ENEMY_STATE.TRACKING : ENEMY_STATE.FAILED;
                ChangeEnemyState(3f, ENEMY_STATE.PATROL);
                break;
            case ENEMY_STATE.KILLED:
                Debug.Log("Kill Player");
                break;
            default:
                Debug.Log("Error!!");
                break;
        }
    }

    /**
     * Update에서 실행하는 작업
     *  - 유한 상태 머신 가동
     */
    void Update()
    {
        try
        {
            EnemyStateMachine();
        }
        catch
        {

        }
        
    }

    // SceneView에서 보기 편하려고 만든 기능
    /// <summary>
    /// Enemy가 인식하는 공간을 Gizmos(빨간색 원)로 그려줌
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, recognitionDistance);
    }
}
