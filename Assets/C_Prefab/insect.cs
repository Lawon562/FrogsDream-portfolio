using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class insect : MonoBehaviour
{
    public Transform Insect;
    public Transform waypointsParent; // waypoint 오브젝트들을 가진 부모 오브젝트
    public float speed = 5f; // 이동 속도

    private Transform[] waypoints; // waypoint 오브젝트들 배열
    private int currentWaypointIndex = 0; // 현재 waypoint 인덱스



    void Start()
    {
        // waypointsParent에서 모든 자식 오브젝트들의 Transform 컴포넌트를 가져와 배열에 저장
        waypoints = waypointsParent.GetComponentsInChildren<Transform>();

        // 배열의 첫 번째 요소는 부모 오브젝트 자체이므로 제외
        waypoints = waypoints.Skip(1).ToArray();
    }

    void Update()
    {


        // 현재 위치에서 다음 waypoint를 향하는 벡터 계산
        Vector3 direction = (waypoints[currentWaypointIndex].position - transform.position).normalized;


        // waypoint 방향으로 이동
        Insect.transform.position += direction * speed * Time.deltaTime;

        // waypoint에 도달하면 다음 waypoint로 인덱스 증가
        if (Vector3.Distance(Insect.transform.position, waypoints[currentWaypointIndex].position) < 0.1f)
        {
            currentWaypointIndex++;
            // 모든 waypoint를 도달한 경우 첫 번째 waypoint로 인덱스 초기화
            if (currentWaypointIndex >= waypoints.Length)
            {
                currentWaypointIndex = 0;
            }
        }

        Vector3 dir = (Insect.transform.position - waypoints[currentWaypointIndex].position).normalized;
        Insect.transform.rotation = Quaternion.LookRotation(dir);
    }

}
