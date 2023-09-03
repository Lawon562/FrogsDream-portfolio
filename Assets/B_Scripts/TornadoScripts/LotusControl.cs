using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LotusControl : MonoBehaviour
{

    Vector3 lotusfirst; //연잎의 처음위치 저장할 변수장소
    Vector3 lotusPlace; // 연잎이 이동할 위치 저장할 변수장소

    bool lotusAct = false; //연잎이 발동했는가

    void Start()
    {
        lotusfirst = this.transform.position; //처음위치 저장
        lotusPlace = new Vector3(this.transform.position.x, -2.4f, this.transform.position.z); // 이동할 위치 계산 : x와 z위치는 그대로 받고 y(높이)만 아래로 2.4만큼 내린다
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
    /// 연잎을 내렸다가 제자리로
    /// </summary>
    void LotusMove()
    {
        if (lotusAct == true) //연잎이 발동됐으면 내려간다
        {
            
            this.transform.position = Vector3.Lerp(this.transform.position, lotusPlace, Time.deltaTime * 2.2f);
        }
        else //아니라면 연잎을 제자리로 되돌린다
        {
            this.transform.position = Vector3.Lerp(this.transform.position, lotusfirst, Time.deltaTime * 1.4f);
        }
    }
    private void OnCollisionStay(Collision other) 
    {
        if (other.transform.CompareTag("Player"))
        {
            other.transform.SetParent(this.transform);//플레이어가 닿아있는동안 연잎을 부모객체로 설정
            LotusActivate();
        }
        
    }

    private void OnCollisionExit(Collision other)//플레이어가 떨어지면 부모관계 끊기
    {
        other.transform.SetParent(null);
    }
    /// <summary>
    /// 연잎 발동
    /// </summary>
    void LotusActivate()
    {
        lotusAct = true;
    }
    /// <summary>
    /// 연잎 끄기
    /// </summary>
    void LotusDeAct()
    {
        lotusAct = false;
    }
}