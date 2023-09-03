using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class PlaytimeManager : MonoBehaviour
{
    //Timecanvas�� �޾ƿ� ���� ����
    public GameObject Timecanvas;

    // �̼�â ī��Ʈ�ٿ�
    float countdown;
    public TMP_Text count_txt;

    // �÷��� �ð� ī��Ʈ �ٿ�
    public TMP_Text playtime_txt;
    float playtime;
    public Slider lefttime_slider;

    // Ÿ�̸� ����, ���� ����
    public bool recog;

    //��ü �ð� ���
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
        if (countdown >= 0) // 5�ʵ��� ī��Ʈ�ٿ�
        {
            CountDown();
        }
        else // ������ �÷��̰��� ����
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

    // ���� ���� �� ī��Ʈ �ٿ� 5��
    public void CountDown()
    {
        countdown -= Time.unscaledDeltaTime;
        count_txt.text = Mathf.Round(countdown).ToString();
    }

    //float cur_time = 0;

    // ���� �÷��� �ð� 60��
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
