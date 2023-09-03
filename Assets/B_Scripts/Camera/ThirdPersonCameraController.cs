using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCameraController : MonoBehaviour
{
    //[SerializeField]
    //private Transform Player;
    public Transform Player;
    float xAxis;
    // Start is called before the first frame update
    void Start()
    {
        xAxis = Camera.main.transform.rotation.eulerAngles.x;

    }
    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");
        float mouseWheel = Input.GetAxis("Mouse ScrollWheel");

        if (Player != null)
        {
            this.transform.position = Player.position;
        }
        else
        {
            this.transform.position = Vector3.zero;
        }
        

        RotateCamera(mouseX, mouseY);
        ZoomCamera(mouseWheel);
    }

    
    void RotateCamera(float mouseX, float mouseY)
    {
        //// 회전 속도 조절 변수
        float rotationSpeed = 2.0f;
        xAxis -= mouseY * rotationSpeed; // Y축 회전 값 갱신
        xAxis = Mathf.Clamp(xAxis, -10f, 50f); // 각도 제한 (0도~40도)
        transform.rotation = Quaternion.Euler(xAxis, transform.rotation.eulerAngles.y + mouseX * rotationSpeed, 0f); // 회전 값 적용
    }

    void ZoomCamera(float mouseWheel)
    {
        // 카메라의 로컬 Z축 방향으로 줌 인/아웃
        Vector3 zoom = new Vector3(0, 0.5f, transform.localScale.z - (mouseWheel));
        zoom.z = Mathf.Clamp(zoom.z, 0.5f, 2f);
        transform.localScale = zoom;
    }
}
