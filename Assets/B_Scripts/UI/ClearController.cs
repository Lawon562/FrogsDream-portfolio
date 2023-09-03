using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ClearController : MonoBehaviour
{
    public TMP_Text stage_txt;
    // Start is called before the first frame update
    void Start()
    {
        stage_txt.text = "STAGE " + GameData.stage;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
