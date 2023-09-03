using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LotusControl : MonoBehaviour
{

    Vector3 lotusfirst; //������ ó����ġ ������ �������
    Vector3 lotusPlace; // ������ �̵��� ��ġ ������ �������

    bool lotusAct = false; //������ �ߵ��ߴ°�

    void Start()
    {
        lotusfirst = this.transform.position; //ó����ġ ����
        lotusPlace = new Vector3(this.transform.position.x, -2.4f, this.transform.position.z); // �̵��� ��ġ ��� : x�� z��ġ�� �״�� �ް� y(����)�� �Ʒ��� 2.4��ŭ ������
        lotusAct = false;
    }

    void Update()
    {

    }


    private void FixedUpdate()
    {
        LotusMove();
        LotusDeAct();
    }
    /// <summary>
    /// ������ ���ȴٰ� ���ڸ���
    /// </summary>
    void LotusMove()
    {
        if (lotusAct == true) //������ �ߵ������� ��������
        {
            
            this.transform.position = Vector3.Lerp(this.transform.position, lotusPlace, Time.deltaTime * 2.2f);
        }
        else //�ƴ϶�� ������ ���ڸ��� �ǵ�����
        {
            this.transform.position = Vector3.Lerp(this.transform.position, lotusfirst, Time.deltaTime * 1.4f);
        }
    }
    private void OnCollisionStay(Collision other) 
    {
        if (other.transform.CompareTag("Player"))
        {
            other.transform.SetParent(this.transform);//�÷��̾ ����ִµ��� ������ �θ�ü�� ����
            LotusActivate();
        }
        
    }

    private void OnCollisionExit(Collision other)//�÷��̾ �������� �θ���� ����
    {
        other.transform.SetParent(null);
    }
    /// <summary>
    /// ���� �ߵ�
    /// </summary>
    void LotusActivate()
    {
        lotusAct = true;
    }
    /// <summary>
    /// ���� ����
    /// </summary>
    void LotusDeAct()
    {
        lotusAct = false;
    }
}