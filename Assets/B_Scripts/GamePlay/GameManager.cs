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
     * �� ���� Game Play ���� ��ü�� ������ �������ּ���.
     * ��ǥ������ Player, Enemy �� ���� �÷��̿� ���õ� ������ �Դϴ�.
     */
    float limitTime; // ���� �÷��� ���ѽð� �Դϴ�. stage�� ���� �ٸ��� ��еǰ� �������ּ���.
    float timer; // GameFlow � ����� �ð� �����Դϴ�. Coroutine�� ������� �ʴ� ������ timer�� �����ϰ� �ð��� ������ �� �ֵ��� �������ּ���.

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
        // UI �ε��ϴ� ������ ���ϴ�.
        // ���⿡ ���� �߰����� ���ð� �۾��Ͻ� UI ��ũ��Ʈ�� �޾ƿ��� �ѹ��� ��ġ���� �ҰԿ�.
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
    // ���� �÷����� �帧�� ���������� �����մϴ�.
    // ������ Ŭ��� ���� ���� ���θ� �������� �ʽ��ϴ�.
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
    // ������ �÷��� ���� ��� ������ Ŭ�������� ���ӿ��������� �����մϴ�.
    // �� �浹 ��Ȳ �̺�Ʈ, �ð� �ʰ� �̺�Ʈ, ���� ���� �������� �����ϰڽ��ϴ�.
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
