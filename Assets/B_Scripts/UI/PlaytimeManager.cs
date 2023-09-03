using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PlaytimeManager : MonoBehaviour
{
    //Timecanvas를 받아올 공간 만듬
    public GameObject Timecanvas;

    // 미션창 카운트다운
    float countdown;
    public TMP_Text count_txt;

    // 플레이 시간 카운트 다운
    public TMP_Text playtime_txt;
    float playtime;
    public Slider lefttime_slider;

    // 타이머 실행, 종료 관리
    public bool recog;

    //전체 시간 재기
    //public float wholetime;

    void Start()
    {
        playtime = 120f - GameData.stage * 10;
        if (playtime < 30)
        {
            playtime = 30;
        }
        GameData.playTime = playtime;
        countdown = 5f;
        Timecanvas.gameObject.SetActive(true);

        lefttime_slider.minValue = 0;
        lefttime_slider.maxValue = GameData.playTime;
        lefttime_slider.value = GameData.playTime;

    }

    void Update()
    {
        //wholetime += Time.deltaTime;
        Recog();
    }

    public void Recog()
    {
        if (countdown >= 0) // 5초동안 카운트다운
        {
            CountDown();
        }
        else // 끝나면 플레이게임 실행
        {
            Timecanvas.gameObject.SetActive(false);
            playtime -= Time.unscaledDeltaTime;
            LeftTime();
        }
        
    }

    public void addTime()
    {
        float addingTime = 10 - ((GameData.stage - 1) * 0.3f);
        if (addingTime < 3)
            addingTime = 3;
        playtime += addingTime;
    }

    // 게임 시작 시 카운트 다운 5초
    public void CountDown()
    {
        countdown -= Time.unscaledDeltaTime;
        count_txt.text = Mathf.Round(countdown).ToString();
    }

    //float cur_time = 0;

    // 게임 플레이 시간 60초
    public void LeftTime()
    {
        lefttime_slider.value = playtime;

        playtime_txt.text = Mathf.Round(playtime).ToString();
    }
    public void Btn_ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

}
