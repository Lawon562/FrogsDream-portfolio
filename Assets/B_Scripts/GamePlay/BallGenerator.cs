/**
 * Creation Day : 2023.04.15
 * Script Name : BallGenerator
 * Author : Jaemin Lee
 * 
 * Description
 *  - ��¥ ���� ��¥ ���� NavMesh ���� ���� ��ǥ�� �����Ͽ� �̵���Ű�� ��ũ��Ʈ
 */

using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;
using UnityEngine.AI;


/// <summary>
/// ��¥ ���� ��¥ ���� NavMesh ���� ���� ��ǥ�� �����Ͽ� �̵���Ŵ.
/// </summary>
public class BallGenerator: MonoBehaviour
{
    [SerializeField] private GameObject RealBall;   // ��¥ �� ������Ʈ ���� ����
    [SerializeField] private GameObject FakeBall;   // ��¥ �� ������Ʈ ���� ����
    [SerializeField] private int MaxBallCount;      // ������ �� �ִ� ����
    [SerializeField] private Transform Ground;       // �ٴ��� ����, ���� ���̸� ���ϱ� ���� ������ ������Ʈ

    private List<bool> balls;   // ���� ��¥, ��¥ ���θ� boolŸ������ ������ ����Ʈ
                                // �� ���� �� �� ����Ʈ�� ���ʷ� ��¥, ��¥ ���� ������.

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
        MaxBallCount = GameData.maxBallCount;
        CheckMaxBallCount();
        SetBallList();
        
        Invoke("BallGenerate", 0.2f);
        /* 0.2�� �� �����Ű�� ���� */
        // NavMesh�� Bake�ǰ� NavMeshObstacle�� ����˴ϴ�. �� ���̿� BallRandomPosition�� ����Ǹ�
        // Ball�� ������Ʈ���� ��ø�� ������ ������ ���ɼ��� �ֽ��ϴ�.
        // ����, NavMesh�� NavMeshObstacle�� ������ ����Ǵ� 0.2�� �� ���� ������ǥ ������ �����ϵ��� �Ͽ����ϴ�.
    }


    /// <summary>
    /// MaxBallCount ������ ���� ���� 0�̶��(�ʱ�ȭ���� �ʾҴٸ�) default ���� 30���� �����մϴ�.
    /// </summary>
    private void CheckMaxBallCount()
    {
        if (MaxBallCount == 0) MaxBallCount = 30;
    }


    /// <summary>
    /// balls ����Ʈ�� MaxBallCount��ŭ false�� �ʱ�ȭ�ϰ�, 
    /// 0~MaxBallCount-1���� ������ ���� �̾� �ش� ���� �ε����� true�� �����մϴ�.
    /// </summary>
    private void SetBallList()
    {
        balls = new List<bool>();
        for (int i = 0; i < MaxBallCount; i++) balls.Add(false);
        balls[Random.Range(0, MaxBallCount - 1)] = true;
    }

    /// <summary>
    /// Ground�� ����, ���� ���� groundWidth, groundDepth ������ �����մϴ�.
    /// </summary>
    private void GetGroundArea()
    {
        /* �ٸ� �ڵ�� �� ��*/
        //Vector3 plainSize = Ground.bounds.size;
        //groundWidth = plainSize.x;
        //groundDepth = plainSize.z; // ���� ����

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
    /// �ٴ��� ����, ���� ���̸� �����Ͽ� ���� Vector3�� ��ȯ�մϴ�.
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
            obj.GetComponent<NavMeshAgent>().enabled = false;    // NavMeshAgent ����
            
            /* NavMeshAgent ���� ����*/
            // NavMesh�� ���������� ���� �÷��̾�� �پ �������� ��ġ�� �ö��� �ʰ� �ٴ� ǥ�鿡 �پ��ְ� �˴ϴ�.
            // ����, ���� ���� �� ������ ���� NavMeshAgent�� ���� ���� �̵��� �����Ӱ� �ؾ� �մϴ�.
        }
    }

    

    /// <summary>
    /// ��¥/��¥ ���� ������ �� NavMesh ������ ���� ��ǥ�� ������ŵ�ϴ�.
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
