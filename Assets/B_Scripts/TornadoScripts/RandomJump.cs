using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomJump : MonoBehaviour
{
    private NavMeshAgent agent;
    float randomValue; //������ ���� ����
    bool isGround; //���ΰ�?
    void Start()
    {
        StartCoroutine("Delay");
        agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        
        if (isGround == true)
        {
            GetRandom();
        }
        else
        {
            return;
        }
    }

    IEnumerator Delay()
    {
        yield return new WaitForSeconds(2f);

    }

    /// <summary>
    /// ���� ����
    /// </summary>
    void GetRandom()
    {
        randomValue = Random.Range(0, 5000);
        if (randomValue < 3) // �����Ӵ� 0.003%Ȯ���� ����
        {
            agent.enabled = false;
            this.GetComponent<EnemyPathFinder>().enabled = false;
            //this.transform.GetComponent<Rigidbody>().velocity = new Vector3(0,0,0);
            this.transform.GetComponent<Rigidbody>().useGravity = true;
            this.transform.GetComponent<Rigidbody>().AddForce(Vector3.up * Time.deltaTime * 1000f, ForceMode.Impulse);
            isGround = false;
        }
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.transform.CompareTag("Ground") && agent != null) //����ִ°� ���̸�
        {
            this.transform.GetComponent<Rigidbody>().useGravity = false;
            this.GetComponent<EnemyPathFinder>().enabled = true;
            agent.enabled = true;
            isGround = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.transform.CompareTag("Ground"))//����ִ��� ���̸�
        {
            isGround = false;
        }
    }
    //float randomValue; //������ ���� ����
    //bool isGround; //���ΰ�?
    //void Start()
    //{

    //}

    //void Update()
    //{
    //    if (isGround == true)
    //    {
    //        GetRandom(); 
    //    }
    //    else
    //    {
    //        return;
    //    }
    //}

    ///// <summary>
    ///// ���� ����
    ///// </summary>
    //void GetRandom()
    //{
    //    randomValue = Random.Range(0, 10000);
    //    if (randomValue < 1000) // �����Ӵ� 0.003%Ȯ���� ����
    //    {
    //        this.transform.GetComponent<Rigidbody>().AddForce(Vector3.up * 10f, ForceMode.Impulse);
    //        print("����");
    //    }
    //}
    //private void OnCollisionStay(Collision other)
    //{
    //    if (other.transform.CompareTag("Ground")) //����ִ°� ���̸�
    //    {
    //        isGround = true;
    //        print("���̴�");
    //    }
    //}

    //private void OnCollisionExit(Collision other)
    //{
    //    if (other.transform.CompareTag("Ground"))//����ִ��� ���̸�
    //    {
    //        isGround = false;
    //        print("�����̴�");
    //    }
    //}
}
