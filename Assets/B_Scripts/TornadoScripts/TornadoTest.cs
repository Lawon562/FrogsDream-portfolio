using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TornadoTest : MonoBehaviour
{
    public float upSpeed = 10f; //��¼ӵ�
    public float rotSpeed = 50f; //ȸ���ӵ�
    public GameObject particle;

    //�׽�Ʈ
    

    public bool activate = false; //�ߵ��Ǿ��°�

    Rigidbody player; //�÷��̾��� ������ٵ� �ޱ����� ����
    //PlayerMoveTest playerControl; //�÷��̾���׽�Ʈ ��ũ��Ʈ ���� ������ �����ϱ� ���� ����

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
    /// ����̵��� �ߵ��Ǹ� �÷��̾ ȸ����½�Ų��
    /// </summary>
    void TornadoLogic()
    {
        if (activate == true)
        {
            float angle = rotSpeed * Time.deltaTime;

            player.transform.RotateAround(this.transform.position, Vector3.up, angle);//��ü�� �������� �߽����� ������. ����: ������ü.transform.RotateAround(�߽���ġ, �߽���, ȸ���ӵ�);
            player.AddForce(Vector3.up * upSpeed * Time.deltaTime, ForceMode.Impulse); //�������� ���ֱ�
        }
        
    }

    private void OnCollisionEnter(Collision other) 
    {
        if (other.gameObject.name == "Player")
        {
            //playerControl = other.transform.GetComponent<PlayerMoveTest>();//�÷��̾��� ������� �����ϱ� ���� PlayerMoveTest��ũ��Ʈ�� ����
            player = other.transform.GetComponent<Rigidbody>(); //�÷��̾��� ������ �޾� player�� �ֱ�

            TornadoAct();
        }
    }

    /// <summary>
    /// ����̵��� Ȱ��ȭ�Ѵ�
    /// </summary>
    void TornadoAct()
    {
        //playerControl.cancontrol = false; //�÷��̾� ����� ����
        activate = true; //�ߵ�����
        Invoke("TornadoDeAct", 2f); //�ߵ��� 2�ʵ� TornadoDeAct�� �ߵ�
        Destroy( Instantiate(particle,this.transform), 2f );
    }
    /// <summary>
    /// ����̵� ��Ȱ��ȭ
    /// </summary>
    void TornadoDeAct()
    {
        //playerControl.cancontrol = true; //�÷��̾� ����� �����ֱ�
        activate = false; //�ߵ� ��
        //player.velocity.Set(0,0,0); //�̵��� ���ֱ�

        GameObject player = GameObject.Find("Player");

        player.GetComponent<CharacterController>().enabled = true;
        player.GetComponent<PlayerMovement>().enabled = true;
        player.GetComponent<PlayerJump>().enabled = true;
        PlayerJump.MoveDir = Vector3.zero;
        player.GetComponent<Rigidbody>().isKinematic = true;
        player.GetComponent<Rigidbody>().useGravity = false;
    }
}
