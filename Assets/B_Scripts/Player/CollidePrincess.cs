using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollidePrincess : MonoBehaviour
{
    public AudioClip collectSound, RealBallSound, FraudBallSound;
    public Transform BackPack;
    AudioSource collectAsrc, RealBallAsrc, FraudBallAsrc;
    bool getBall = false;

    public GameObject collectParticle, realParticle, fraudParticle;

    private void Start()
    {
        collectAsrc = transform.GetChild(1).GetComponent<AudioSource>();
        RealBallAsrc = transform.GetChild(0).GetComponent<AudioSource>();
        FraudBallAsrc = transform.GetChild(3).GetComponent<AudioSource>();
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!getBall && hit.gameObject.tag == "Mission_Obj") // 닿은 객체가 플레이어라면 머리위로
        {
            if (collectAsrc.isPlaying)
                collectAsrc.Stop();
            collectAsrc.PlayOneShot(collectSound, GameData.sfxVolume);
            Destroy(Instantiate(collectParticle, this.transform), 3f);
            getBall = !getBall;
            hit.gameObject.transform.position = BackPack.position;
            hit.gameObject.transform.SetParent(BackPack.transform);
        }

        if (getBall && hit.gameObject.tag == "Princess")
        {
            //Debug.Log("GB && Collide Princess");
            getBall = !getBall;
            GameObject ball = BackPack.transform.GetChild(0).gameObject;
            if (ball.name == "Real(Clone)")
            {
                if (RealBallAsrc.isPlaying)
                    RealBallAsrc.Stop();
                RealBallAsrc.PlayOneShot(RealBallSound, GameData.sfxVolume);
                GameObject.Find("GameManager").SendMessage("changeClear");
                Destroy(Instantiate(realParticle, this.transform), 3f);
                Invoke("GoSuccessScene", 1.5f);
            }
            else if (ball.name == "fraud(Clone)")
            {
                if (FraudBallAsrc.isPlaying)
                    FraudBallAsrc.Stop();
                FraudBallAsrc.PlayOneShot(FraudBallSound, GameData.sfxVolume);
                Destroy(Instantiate(fraudParticle, this.transform), 3f);
            }
            Destroy(ball);
        }
        else if (hit.gameObject.tag == "Princess")
        {
            //Debug.Log("공을 가져오세요.");
        }
    }

    private void GoSuccessScene()
    {
        SceneManager.LoadScene("6_Success");
    }
}
