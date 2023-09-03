using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConversionView : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        firstCameraView.tag = "Untagged";
        firstCameraView.SetActive(false);
        ThirdCameraView.SetActive(true);
        ThirdCameraView.tag = "MainCamera";
    }

    public static bool view = false;
    [SerializeField]
    GameObject firstCameraView;
    [SerializeField]
    GameObject ThirdCameraView;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.V))
        {
            view = !view;
            if (view)
            {
                ThirdCameraView.tag = "Untagged";
                firstCameraView.SetActive(true);
                firstCameraView.tag = "MainCamera";
                ThirdCameraView.SetActive(false);
            }
            else
            {
                firstCameraView.tag = "Untagged";
                firstCameraView.SetActive(false);
                ThirdCameraView.SetActive(true);
                ThirdCameraView.tag = "MainCamera";
            }
        }
    }
}
