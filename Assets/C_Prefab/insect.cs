using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class insect : MonoBehaviour
{
    public Transform Insect;
    public Transform waypointsParent; // waypoint ������Ʈ���� ���� �θ� ������Ʈ
    public float speed = 5f; // �̵� �ӵ�

    private Transform[] waypoints; // waypoint ������Ʈ�� �迭
    private int currentWaypointIndex = 0; // ���� waypoint �ε���



    void Start()
    {
        // waypointsParent���� ��� �ڽ� ������Ʈ���� Transform ������Ʈ�� ������ �迭�� ����
        waypoints = waypointsParent.GetComponentsInChildren<Transform>();

        // �迭�� ù ��° ��Ҵ� �θ� ������Ʈ ��ü�̹Ƿ� ����
        waypoints = waypoints.Skip(1).ToArray();
    }

    void Update()
    {


        // ���� ��ġ���� ���� waypoint�� ���ϴ� ���� ���
        Vector3 direction = (waypoints[currentWaypointIndex].position - transform.position).normalized;


        // waypoint �������� �̵�
        Insect.transform.position += direction * speed * Time.deltaTime;

        // waypoint�� �����ϸ� ���� waypoint�� �ε��� ����
        if (Vector3.Distance(Insect.transform.position, waypoints[currentWaypointIndex].position) < 0.1f)
        {
            currentWaypointIndex++;
            // ��� waypoint�� ������ ��� ù ��° waypoint�� �ε��� �ʱ�ȭ
            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;
            }
        }

        Vector3 dir = (Insect.transform.position - waypoints[currentWaypointIndex].position).normalized;
        Insect.transform.rotation = Quaternion.LookRotation(dir);
    }

}
