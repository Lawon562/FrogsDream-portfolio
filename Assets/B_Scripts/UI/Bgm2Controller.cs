using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//스크립트가 어디 있든 자동적으로 토글을 찾아준대용
[RequireComponent(typeof(Toggle))]

public class Bgm2Controller : MonoBehaviour
{
    //볼륨조절 슬라이더
    public Slider vol_BGM;

    //음소거 토글
    public Toggle bgm2Toggle;

    //오디오 파일
    public AudioClip audioClip_bg2; //플레이 브금

    private AudioSource audioSource;


    void Start()
    {
        //toggle 컴포넌트 연결하기
        bgm2Toggle = GetComponent<Toggle>();

        SoundInit();
        DontDestroyOnLoad(this);
        audioSource = GetComponent<AudioSource>();


    }
    void Update()
    {
        //프레임마다 볼륨 조절
        audioSource.volume = GameData.bgmVolume;

        //토글 체크했는지 확인하기
        bgm2Toggle.onValueChanged.AddListener(delegate {
            BgmOnOFF(bgm2Toggle);
        });
        
    }

    //slider로 볼륨값 조절해 GameData에 저장하기
    void SoundInit()
    {
        vol_BGM.minValue = 0f;
        vol_BGM.maxValue = 1f;
        vol_BGM.value = GameData.bgmVolume;
    }

    //GameData에 지정된 볼륨값을 불러오기
    public void Change_vol_BGM()
    {
        GameData.bgmVolume = vol_BGM.value;
    }

    //toggle 선택 시 mute
    public void BgmOnOFF(bool isOn)
    {
        if (isOn)
        {
            audioSource.mute = true;
        }

    }

}