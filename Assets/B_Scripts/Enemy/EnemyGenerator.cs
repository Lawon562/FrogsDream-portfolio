using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField] private GameObject Enemy;  // �� ��ü
    [SerializeField] private int MaxCount;      // ������ �� �ִ� ����
    [SerializeField] private Transform Ground;       // �ٴ��� ����, ���� ���̸� ���ϱ� ���� ������ ������Ʈ


    private float groundStartX;
    private float groundEndX;
    private float groundStartZ;
    private float groundEndZ;
    /**
     * Start���� �����ϴ� �۾�
     * - MaxBallCount(�� �ִ� ����) �˻�
     * - balls ����Ʈ ����(MaxBallCount��ŭ bool �� ����)
     * - 0.2�� ��, ���� ���� ��ǥ�� ������Ű�� BallGenerate ����
     */
    void Start()
    {
        CheckMaxCount();
        MaxCount = GameData.maxEnemy;
        Invoke("EnemyGenerate", 0.2f);
        /* 0.2�� �� �����Ű�� ���� */
        // NavMesh�� Bake�ǰ� NavMeshObstacle�� ����˴ϴ�. �� ���̿� BallRandomPosition�� ����Ǹ�
        // Ball�� ������Ʈ���� ��ø�� ������ ������ ���ɼ��� �ֽ��ϴ�.
        // ����, NavMesh�� NavMeshObstacle�� ������ ����Ǵ� 0.2�� �� ���� ������ǥ ������ �����ϵ��� �Ͽ����ϴ�.
    }

    /// <summary>
    /// MaxCount ������ ���� ���� 0�̶��(�ʱ�ȭ���� �ʾҴٸ�) default ���� 30���� �����մϴ�.
    /// </summary>
    private void CheckMaxCount()
    {
        if (MaxCount == 0) MaxCount = 30;
    }

    /// <summary>
    /// Ground�� ����, ���� ���� groundWidth, groundDepth ������ �����մϴ�.
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
    /// ���޹��� ball ���� ������� ��¥ / ��¥ ���� �����Ͽ� ��ȯ�մϴ�.
    /// </summary>
    /// <param name="ball">������ ��ü�� ��¥(true)/��¥(false) ����</param>
    /// <returns>����� �� ��ü</returns>
    private GameObject CopyObject()
    {
        GameObject copy = Instantiate(Enemy);
        return copy;
    }

    /// <summary>
    /// �ٴ��� ����, ���� ���̸� �����Ͽ� ���� Vector3�� ��ȯ�մϴ�.
    /// </summary>
    /// <returns></returns>
    private Vector3 GetRandomPosition()
    {
        return new Vector3(
            Random.Range(groundStartX, groundEndX),        // x
            (Random.Range(0,2) == 0) ? 0 : -3,                                            // y
            Random.Range(groundStartZ, groundEndZ)         // z
        );

        // x : �ٴ��� ���� �߽����� �������� -��ǥ���� +��ǥ������ ���� ���� ��ȯ�մϴ�.
        // y : magic number�Դϴ�. ��ǥ�� 0���� �����ص� �����Ͽ�����, ��� ���� ���� �ٴڿ� ����� �� �ֵ��� ���������� �����Ͽ����ϴ�.
        // z : �ٴ��� ���� �߽����� �������� -��ǥ���� +��ǥ������ ���� ���� ��ȯ�մϴ�.

        // ���� (50, 60)¥�� �ٴ��� ���� ��,
        // x : -25 ~ +25 ������ ���� ��
        // y :   0 ~ +10 ������ ���� ��
        // z : -30 ~ +30 ������ ���� ��
    }

    /// <summary>
    /// ��ü�� �޾� bake�� NavMesh ���� ������ ������ŵ�ϴ�.
    /// </summary>
    /// <param name="obj">������ų ��ü</param>
    private void WarpInNavMeshArea(GameObject obj)
    {
        // �ٴ� ���μ��� ���̸� �̿��� ���� ��ǥ�� warpPosition�� �����մϴ�.
        Vector3 warpPosition = GetRandomPosition();

        // NavMesh.SamplePosition : NavMesh ���� ������ Ư�� ��ġ�� ���ø�(����)�ϴ� ����Դϴ�.
        // �־��� warpPosition�� NevMesh ���� ���� �ִٸ� true��, �ƴϸ� false�� ��ȯ�մϴ�.
        if (NavMesh.SamplePosition(warpPosition, out NavMeshHit hit, 10.0f, NavMesh.AllAreas))
        {
            obj.GetComponent<NavMeshAgent>().Warp(hit.position); // ��ȿ�� NavMesh ��ġ�� ������Ʈ �̵�
            obj.GetComponent<EnemyPathFinder>().enabled = true;
            obj.GetComponent<RandomJump>().enabled = true;
        }
    }

    /// <summary>
    /// ��¥/��¥ ���� ������ �� NavMesh ������ ���� ��ǥ�� ������ŵ�ϴ�.
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
