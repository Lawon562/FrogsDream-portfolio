using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Toggle))]
public class AudioController_sfx2 : MonoBehaviour
{
    //볼륨조절 슬라이더
    public Slider vol_SFX;

    //음소거 토글
    public Toggle sfxToggle;

    //오디오 파일
    public AudioClip audioClip_sfx;
    private AudioSource audioSource;


    void Start()
    {
        //toggle 컴포넌트 연결하기
        sfxToggle = GetComponent<Toggle>();

        SoundInit();
        DontDestroyOnLoad(this);
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        //프레임마다 볼륨 조절
        audioSource.volume = GameData.sfxVolume;
        //토글 체크했는지 확인하기
        sfxToggle.onValueChanged.AddListener(delegate
        {
            SfxOnOFF(sfxToggle);
        });
        //마우스 좌클릭 시 sfx 재생
        if (Input.GetMouseButtonDown(0))
        {
            audioSource.PlayOneShot(audioClip_sfx, GameData.sfxVolume);
        }
    }

    //slider로 볼륨값 조절해 GameData에 저장하기
    void SoundInit()
    {
        vol_SFX.minValue = 0f;
        vol_SFX.maxValue = 1f;
        vol_SFX.value = GameData.sfxVolume;
    }

    //GameData에 지정된 볼륨값을 불러오기
    public void Change_vol_SFX()
    {
        GameData.sfxVolume = vol_SFX.value;
    }

    //toggle 선택 시 mute
    public void SfxOnOFF(bool isOn)
    {
        if (isOn)
        {
            audioSource.mute = true;
        }

    }
}
