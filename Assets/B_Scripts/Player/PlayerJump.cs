using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJump : MonoBehaviour
{
    public AudioClip jumpSound;
    public float speed;     // ĳ���� ������ ���ǵ�.
    public float jumpSpeed; // ĳ���� ���� ��.
    public float gravity;   // ĳ���Ϳ��� �ۿ��ϴ� �߷�.

    private CharacterController controller; // ���� ĳ���Ͱ� �������ִ� ĳ���� ��Ʈ�ѷ� �ݶ��̴�.
    //private Vector3 MoveDir;                // ĳ������ �����̴� ����.
    public static Vector3 MoveDir;                // ĳ������ �����̴� ����.
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
        // ���� ĳ���Ͱ� ���� �ִ°�?
        if (controller.isGrounded)
        {
            // �÷��̾ �ٶ󺸴� �������� ����
            MoveDir = Vector3.zero;

            // ���͸� ���� ��ǥ�� ���ؿ��� ���� ��ǥ�� �������� ��ȯ�Ѵ�.
            MoveDir = transform.TransformDirection(MoveDir);

            // ���ǵ� ����.
            MoveDir *= speed;

            // ĳ���� ����
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
        
        // ĳ���Ϳ� �߷� ����.
        MoveDir.y -= gravity * Time.deltaTime;

        // ĳ���� ������.
        
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
