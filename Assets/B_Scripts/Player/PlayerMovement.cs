/**
 * Creation Day : 2023.04.15
 * Script Name : PlayerMovement
 * Author : Jaemin Lee
 * 
 * Description
 *  - 방향키/WASD 입력 시의 플레이어 이동에 관련된 스크립트
 */

using UnityEngine;

/// <summary>
/// 방향키/WASD 입력 시 플레이어 회전 및 움직임을 담당함
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip walkSound;
    public float Speed;
    public float jumpPow = 6f; //플레이어 점프력
    Vector3 movePower;
    CharacterController characterController;

    Animator ani;


    void Start()
    {
        //rigid = GetComponent<Rigidbody>();
        characterController = GetComponent<CharacterController>();
        ani = this.GetComponentInChildren<Animator>();
        ani.SetTrigger("idle");
        audioSource = GetComponent<AudioSource>();
    }
    


    void Update()
    {
        //Jump();

    }

    private void FixedUpdate()
    {
        Move();
    }
    float gravity = 9.8f;
    Vector3 dir = new Vector3();

    /// <summary>
    /// 플레이어 점프
    /// </summary>
    void Jump()
    {
        if (characterController.isGrounded)
        {
            Debug.Log("isGrounded");
            dir.y = 0;

            if (Input.GetButtonDown("Jump"))
            {
                dir.y = jumpPow;
            }
        }
        dir.y -= gravity * Time.deltaTime;
        characterController.Move(dir * Time.deltaTime);
    }

    

    private void Move()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");
        if (h != 0f || v != 0f)
        {
            float y = Camera.main.transform.rotation.eulerAngles.y;
            ani.SetTrigger("walk");
            float targetAngle = y;
            if (h > 0f) // D 키를 눌렀을 때
            {
                targetAngle = y + 90f;
                if (v > 0f)
                {
                    targetAngle -= 45f;
                }
                else if (v < 0f)
                {
                    targetAngle += 45f;
                }

            }
            else if (h < 0f) // A 키를 눌렀을 때
            {
                targetAngle = y - 90f;
                if (v > 0f)
                {
                    targetAngle += 45f;
                }
                else if (v < 0f)
                {
                    targetAngle -= 45f;
                }
            }
            else if (v < 0f) // S 키를 눌렀을 때
            {
                targetAngle = y + 180f;
            }

            transform.rotation = Quaternion.Euler(0, targetAngle, 0);

            movePower = transform.forward * Time.deltaTime * Speed;
            characterController.Move(movePower);
            if (!audioSource.isPlaying && characterController.isGrounded)
                audioSource.PlayOneShot(walkSound, GameData.sfxVolume);
        }
        else
        {
            ani.SetTrigger("idle");
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.tag == "Water")
        {
            ani.SetTrigger("swim");
        }
    }
}
