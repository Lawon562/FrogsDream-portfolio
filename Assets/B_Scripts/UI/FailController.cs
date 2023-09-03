using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class FailController : MonoBehaviour
{
    
    public void Btn_ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void Go_TiTle()
    {
        destroyAudio();
        SceneManager.LoadScene("1_TiitleScene");
        GameData.stage--;
    }

    public void NextStage()
    {
        destroyAudio();

        
        SceneManager.LoadScene("3_GameScene");
    }

    void destroyAudio()
    {
        GameObject bgm = GameObject.Find("Audio_bgm2");
        GameObject sfx = GameObject.Find("Audio_sfx2");

        DestroyImmediate(bgm);
        DestroyImmediate(sfx);
    }
}
