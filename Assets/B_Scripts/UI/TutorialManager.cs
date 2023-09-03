using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TutorialManager : MonoBehaviour
{


    // Start is called before the first frame update
    void Start()
    {
        Tutorial_mission();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Tutorial_mission()
    {
        this.gameObject.SetActive(true);
    }
    public void Btn_Skip()
    {
        SceneManager.LoadScene("3_GameScene");
    }
}
