using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
/// <summary>
/// Enemy�� ���� ���� (IDLE: EnemyStateMachine ����, PATROL: ���� ��, TRACKING: Ÿ�� �߰� �� ����, FAILED: Ÿ�� ��ħ)
/// </summary>
public enum ENEMY_STATE { IDLE, PATROL, TRACKING, FAILED, KILLED }
public class EnemyPathFinder : MonoBehaviour
{
    private Transform TargetObject;        // Enemy�� ����ٴ� Object�� Transform�� �����մϴ�.
    [SerializeField] private Transform[] PatrolPosition;    // Enemy�� Patrol �߿� �ݵ�� �鷯�� �� ��ġ �迭�Դϴ�.
    [SerializeField] private float recognitionDistance;     // Enemy�� �ν��� �� �ִ� �ִ� �Ÿ��� ����

    //[SerializeField] private GameObject PatrolSet;       // ���� �������� �ڽ� ������Ʈ�� �ִ� ������Ʈ �޾ƿ���
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

    private NavMeshAgent agent;                 // ���� �� PathFind�� ���� NavMeshAgent ������Ʈ ����
    private ENEMY_STATE enemyState;             // Enemy�� ���� ���¸� ������ ������ ����
    private int patrolPositionNumber = 0;       // ��Ʈ���ؾ��ϴ� Position�� �� ��°���� �����ϴ� ����
    private float actionTimer = 0f;             // ���� ���¸� �ٲ� �� ����ϴ� Ÿ�̸� ����

    /**
     * Start���� �����ϴ� �۾�
     *  - enemyState�� �ʱ�ȭ(IDLE)
     *  - NavMeshAgent component ����
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
            // ���� ���� ���� ����(3~4��)
            int currentPatrolPosCount = Random.Range(3,5);

            // ���� ����� �������� ã�� ����Ʈ�� �߰�
            /**
             * ���� ������ �θ� ������Ʈ�κ��� �ڽ� ������Ʈ(���� ������)�� �޾ƿ´�.
             * ���� ��ü�κ��� ���� ���� ������Ʈ������ �Ÿ��� ���Ѵ�.
             * ����ؼ� ���� �Ÿ���, ���� �Ÿ��� ���Ͽ� ���� �Ÿ����� �� ������ �Ÿ��� �� ���� �������� ������Ʈ�� �����Ѵ�.
             *   - ����, ť�� posCount��ŭ ������Ʈ�� ���ִ� ��Ȳ�̶��, ť�� �� �� ������Ʈ�� ������ �����Ѵ�.
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
    /// ���� ��ü���� Target ��ü���� �Ÿ��� ��ȯ�ϴ� �Լ��Դϴ�.
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
    /// �� ��ü �� �Ÿ��� ���Ͽ� ENEMY�� �ִ��Ÿ����� ª���� ENEMY_STATE�� CHASE����, PATROL���� ��ȯ�ϴ� �Լ��Դϴ�.
    /// </summary>
    private ENEMY_STATE CheckDistance(Transform Target)
    {
        // TargetObject�� ���� Object�� �Ÿ��� �ν� �ִ� �Ÿ�(recognitionDistance)���� ������
        return GetDistance(Target) < recognitionDistance ?
            ENEMY_STATE.TRACKING :  // ENEMYSTATE.TRACKING ��ȯ
            ENEMY_STATE.PATROL;     // �װ� �ƴ϶��, ENEMY_STATE.PATROL ��ȯ
    }


    /// <summary>
    /// checkDistance�� �˾Ƴ� ENEMY_STATE�� ����, TRACKING�̸� TargetObject�� �����ϰ� 
    /// �׷��� ������ ��θ� �ʱ�ȭ �� FAILED�� �ٲ۴�.
    /// </summary>
    private void Tracking()
    {
        try
        {
            if (CheckDistance(TargetObject) == ENEMY_STATE.TRACKING)
            {
                // TargetObject�� ��ġ�� ������ ����
                agent.SetDestination(TargetObject.position);
            }
            else
            {
                // ������ �ʱ�ȭ �� ENEMY_STATE�� FAILED�� ����
                agent.ResetPath();
                enemyState = ENEMY_STATE.FAILED;
            }
        }
        catch
        {

        }
        
    }


    /// <summary>
    /// actionTimer�� ���� �ð����� ��ٸ� ��, ���޹��� ENEMY_STATE�� ���� �׼� ���¸� �������ִ� �Լ��Դϴ�.
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
    /// agent�� ������ �������� �����ϴ� �Լ�
    /// </summary>
    private void Patrol()
    {
        // �� ��° ��ġ�� Patrol���� �����Ѵ�.
        agent.SetDestination(PatrolPosition[patrolPositionNumber].position);

        // ���� ��ü�� Patrol�� ��ü�� ��ġ�� �Ÿ��� ���Ѵ�.
        float posDistance = Vector3.Distance(this.transform.position, PatrolPosition[patrolPositionNumber].position);

        // �Ÿ��� 1���� ������, PatrolPositionNumber�� 1�� �ø��µ�,
        // ���� PatrolPosition�迭�� ���̺��� ������� �ٽ� 0���� �����Ѵ�.
        if (posDistance < 2f) patrolPositionNumber++;
        if (patrolPositionNumber == PatrolPosition.Length) patrolPositionNumber = 0;

        // Enemy�� ���¸� enemyState�� �������Ѵ�.
        enemyState = CheckDistance(TargetObject);
    }

    public void Killed()
    {
        enemyState = ENEMY_STATE.KILLED;
    }
    /// <summary>
    /// Enemy�� ���¿� ���� �ൿ�� �ٲ��ִ� �Լ�
    /// </summary>
    private void EnemyStateMachine()
    {
        switch (enemyState)
        {
            case ENEMY_STATE.IDLE:
                // 3�� �� PATROL ����
                ChangeEnemyState(3f, ENEMY_STATE.PATROL);
                break;
            case ENEMY_STATE.PATROL:
                // PATROL ����
                Patrol();
                break;
            case ENEMY_STATE.TRACKING:
                Tracking();
                break;
            case ENEMY_STATE.FAILED:
                // �߰� ���� ���¿��� Target�� �Ÿ��� ����Ͽ� ���� Target �ν� �Ÿ� �ȿ� ������ �ٽ� �߰ݸ��� ��ȯ
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
     * Update���� �����ϴ� �۾�
     *  - ���� ���� �ӽ� ����
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

    // SceneView���� ���� ���Ϸ��� ���� ���
    /// <summary>
    /// Enemy�� �ν��ϴ� ������ Gizmos(������ ��)�� �׷���
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, recognitionDistance);
    }
}
