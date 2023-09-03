using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
public enum GameState
{
    idle, play, pause, clear, over
}
public class GameManager : MonoBehaviour
{
    // UI Settings
    public GameObject timerUI;
    public GameObject optionUI;
    public Slider leftTimeSlider;
    public TMP_Text stage_txt;

    // Game Play Object, Varient
    /*
     * 이 곳에 Game Play 관련 객체와 변수를 설정해주세요.
     * 대표적으로 Player, Enemy 등 게임 플레이에 관련된 데이터 입니다.
     */
    float limitTime; // 게임 플레이 제한시간 입니다. stage에 따라 다르게 배분되게 설정해주세요.
    float timer; // GameFlow 등에 사용할 시간 변수입니다. Coroutine을 사용하지 않는 로직을 timer로 적절하게 시간을 설정할 수 있도록 관리해주세요.

    // Game State enumerator
    GameState g_state;
    void Start()
    {
        GameData.stage++;
        GameData.maxBallCount = GameData.stage * 10;
        GameData.maxEnemy = GameData.stage * 10 + 5;

        g_state = GameState.idle;
        timer = 0f;
        limitTime = GameData.playTime;
        stage_txt.text = "STAGE " + GameData.stage;
    }

    void Update()
    {
        ShowGameUI();
        GameFlow();
        GameRule();

    }

    void ShowGameUI()
    {
        // UI 로드하는 로직이 들어갑니다.
        // 여기에 직접 추가하지 마시고 작업하신 UI 스크립트를 받아오면 한번에 합치도록 할게요.
    }
    void CheckPaused()
    {
        if (timerUI.gameObject.activeSelf || optionUI.gameObject.activeSelf)
        {
            g_state = GameState.pause;
        }
    }

    void ResumePaused()
    {
        if (timerUI.gameObject.activeSelf == false && optionUI.gameObject.activeSelf == false)
        {
            g_state = GameState.play;
        }
    }
    // Game State Check. -> switch of g_state
    // 게임 플레이의 흐름을 전반적으로 관리합니다.
    // 게임의 클리어나 게임 오버 여부를 결정하지 않습니다.
    void GameFlow()
    {
        switch (g_state)
        {
            case GameState.idle:
                CheckPaused();
                break;

            case GameState.play:
                Time.timeScale = 1f;
                CheckPaused();
                break;

            case GameState.pause:
                Time.timeScale = 0f;
                ResumePaused();
                break;

            case GameState.clear:

                break;
            case GameState.over:

                break;
        }
    }

    // Game Rule Check.
    // 게임이 플레이 중인 경우 게임이 클리어인지 게임오버인지를 결정합니다.
    // 적 충돌 상황 이벤트, 시간 초과 이벤트, 성공 등을 로직으로 구현하겠습니다.
    void GameRule()
    {
        switch (g_state)
        {
            case GameState.play:
                timer = leftTimeSlider.value;
                if (timer <= 0)
                {
                    SceneManager.LoadScene("5_Fail_Timeover");
                }
                if (GameObject.Find("Player") == null)
                {
                    Invoke("GoFailScene", 3f);
                }
                break;
        }
    }

    public void changeClear()
    {
        g_state = GameState.clear;
    }
    private void GoFailScene()
    {
        SceneManager.LoadScene("4_Fail_Enemy");
    }

}
