using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    public AudioClip jumpSound;
    public float speed;     // 캐릭터 움직임 스피드.
    public float jumpSpeed; // 캐릭터 점프 힘.
    public float gravity;   // 캐릭터에게 작용하는 중력.

    private CharacterController controller; // 현재 캐릭터가 가지고있는 캐릭터 컨트롤러 콜라이더.
    //private Vector3 MoveDir;                // 캐릭터의 움직이는 방향.
    public static Vector3 MoveDir;                // 캐릭터의 움직이는 방향.
    AudioSource audioSource;


    Animator ani;

    void Start()
    {
        MoveDir = Vector3.zero;
        controller = GetComponent<CharacterController>();
        ani = this.GetComponentInChildren<Animator>();
        audioSource = transform.GetChild(2).GetComponent<AudioSource>();
    }

    void Update()
    {
        // 현재 캐릭터가 땅에 있는가?
        if (controller.isGrounded)
        {
            // 플레이어가 바라보는 방향으로 세팅
            MoveDir = Vector3.zero;

            // 벡터를 로컬 좌표계 기준에서 월드 좌표계 기준으로 변환한다.
            MoveDir = transform.TransformDirection(MoveDir);

            // 스피드 증가.
            MoveDir *= speed;

            // 캐릭터 점프
            if (Input.GetButton("Jump"))
            {
                MoveDir.y = jumpSpeed;
                ani.SetTrigger("jump");
                if (!audioSource.isPlaying)
                {
                    audioSource.PlayOneShot(jumpSound, GameData.sfxVolume);
                }
            }
                
        }
        
        // 캐릭터에 중력 적용.
        MoveDir.y -= gravity * Time.deltaTime;

        // 캐릭터 움직임.
        
        controller.Move(MoveDir * Time.deltaTime);
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ground")
        {
            GetComponent<Rigidbody>().useGravity = false;
            GetComponent<Rigidbody>().isKinematic = true;
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Water")
        {
            ani.SetTrigger("swim");
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Water")
        {
            ani.SetTrigger("idle");
        }
    }
}
