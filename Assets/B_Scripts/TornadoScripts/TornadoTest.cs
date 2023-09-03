using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoTest : MonoBehaviour
{
    public float upSpeed = 10f; //상승속도
    public float rotSpeed = 50f; //회전속도
    public GameObject particle;

    //테스트
    

    public bool activate = false; //발동되었는가

    Rigidbody player; //플레이어의 리지드바디를 받기위한 공간
    //PlayerMoveTest playerControl; //플레이어무브테스트 스크립트 내의 변수에 접근하기 위한 변수

    void Start()
    {
        upSpeed = 13f;
        rotSpeed = 500f;
        activate = false;
        
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.Log(hit.gameObject.name);
    }


    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        TornadoLogic();
    }
    /// <summary>
    /// 토네이도가 발동되면 플레이어를 회전상승시킨다
    /// </summary>
    void TornadoLogic()
    {
        if (activate == true)
        {
            float angle = rotSpeed * Time.deltaTime;

            player.transform.RotateAround(this.transform.position, Vector3.up, angle);//객체를 기준축을 중심으로 돌린다. 구조: 물리객체.transform.RotateAround(중심위치, 중심축, 회전속도);
            player.AddForce(Vector3.up * upSpeed * Time.deltaTime, ForceMode.Impulse); //위쪽으로 힘주기
        }
        
    }

    private void OnCollisionEnter(Collision other) 
    {
        if (other.gameObject.name == "Player")
        {
            //playerControl = other.transform.GetComponent<PlayerMoveTest>();//플레이어의 제어권을 제어하기 위해 PlayerMoveTest스크립트에 접근
            player = other.transform.GetComponent<Rigidbody>(); //플레이어의 물리를 받아 player에 넣기

            TornadoAct();
        }
    }

    /// <summary>
    /// 토네이도를 활성화한다
    /// </summary>
    void TornadoAct()
    {
        //playerControl.cancontrol = false; //플레이어 제어권 뺏기
        activate = true; //발동했음
        Invoke("TornadoDeAct", 2f); //발동후 2초뒤 TornadoDeAct를 발동
        Destroy( Instantiate(particle,this.transform), 2f );
    }
    /// <summary>
    /// 토네이도 비활성화
    /// </summary>
    void TornadoDeAct()
    {
        //playerControl.cancontrol = true; //플레이어 제어권 돌려주기
        activate = false; //발동 끝
        //player.velocity.Set(0,0,0); //이동력 없애기

        GameObject player = GameObject.Find("Player");

        player.GetComponent<CharacterController>().enabled = true;
        player.GetComponent<PlayerMovement>().enabled = true;
        player.GetComponent<PlayerJump>().enabled = true;
        PlayerJump.MoveDir = Vector3.zero;
        player.GetComponent<Rigidbody>().isKinematic = true;
        player.GetComponent<Rigidbody>().useGravity = false;
    }
}
