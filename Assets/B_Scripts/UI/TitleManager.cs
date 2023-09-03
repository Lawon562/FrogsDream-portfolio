using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TitleManager : MonoBehaviour
{

    //public Toggle toggle;


    void Start()
    {

    }

    void Update()
    {
        
    }

    //btn_play ´©¸£¸é Æ©Åä¸®¾ó ¾À ÀÌµ¿
    public void Btn_PlayGame()
    {
        //DontDestroyOnLoad(audioOb);
        SceneManager.LoadScene("2_TutorialScene");
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

