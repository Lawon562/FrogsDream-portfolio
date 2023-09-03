using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RandomJump : MonoBehaviour
{
    private NavMeshAgent agent;
    float randomValue; //랜덤값 받을 변수
    bool isGround; //땅인가?
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
    /// 랜덤 점프
    /// </summary>
    void GetRandom()
    {
        randomValue = Random.Range(0, 5000);
        if (randomValue < 3) // 프레임당 0.003%확률로 점프
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
        if (other.transform.CompareTag("Ground") && agent != null) //닿아있는게 땅이면
        {
            this.transform.GetComponent<Rigidbody>().useGravity = false;
            this.GetComponent<EnemyPathFinder>().enabled = true;
            agent.enabled = true;
            isGround = true;
        }
    }

    private void OnCollisionExit(Collision other)
    {
        if (other.transform.CompareTag("Ground"))//닿아있던게 땅이면
        {
            isGround = false;
        }
    }
    //float randomValue; //랜덤값 받을 변수
    //bool isGround; //땅인가?
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
    ///// 랜덤 점프
    ///// </summary>
    //void GetRandom()
    //{
    //    randomValue = Random.Range(0, 10000);
    //    if (randomValue < 1000) // 프레임당 0.003%확률로 점프
    //    {
    //        this.transform.GetComponent<Rigidbody>().AddForce(Vector3.up * 10f, ForceMode.Impulse);
    //        print("점프");
    //    }
    //}
    //private void OnCollisionStay(Collision other)
    //{
    //    if (other.transform.CompareTag("Ground")) //닿아있는게 땅이면
    //    {
    //        isGround = true;
    //        print("땅이다");
    //    }
    //}

    //private void OnCollisionExit(Collision other)
    //{
    //    if (other.transform.CompareTag("Ground"))//닿아있던게 땅이면
    //    {
    //        isGround = false;
    //        print("공중이다");
    //    }
    //}
}
