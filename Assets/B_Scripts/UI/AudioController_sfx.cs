using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

//��ũ��Ʈ�� ��� �ֵ� �ڵ������� ����� ã���ش��
[RequireComponent(typeof(Toggle))]

public class AudioController_sfx : MonoBehaviour
{
    //�������� �����̴�
    public Slider vol_SFX;

    //���Ұ� ���
    public Toggle sfxToggle;

    //����� ����
    public AudioClip audioClip_sfx;
    private AudioSource audioSource;


    void Start()
    {
        //toggle ������Ʈ �����ϱ�
        sfxToggle = GetComponent<Toggle>();

        SoundInit();
        DontDestroyOnLoad(this);
        audioSource = GetComponent<AudioSource>();
    }

    void Update()
    {
        //�����Ӹ��� ���� ����
        audioSource.volume = GameData.sfxVolume;
        //��� üũ�ߴ��� Ȯ���ϱ�
        sfxToggle.onValueChanged.AddListener(delegate
        {
            SfxOnOFF(sfxToggle);
        });
        //���콺 ��Ŭ�� �� sfx ���
        if (Input.GetMouseButtonDown(0))
        {
            audioSource.PlayOneShot(audioClip_sfx, GameData.sfxVolume);
        }
        ChangeScene();
    }

    //slider�� ������ ������ GameData�� �����ϱ�
    void SoundInit()
    {
        vol_SFX.minValue = 0f;
        vol_SFX.maxValue = 1f;
        vol_SFX.value = GameData.sfxVolume;
    }
    
    //GameData�� ������ �������� �ҷ�����
    public void Change_vol_SFX()
    {
        GameData.sfxVolume = vol_SFX.value;
    }

    //toggle ���� �� mute
    public void SfxOnOFF(bool isOn)
    {
        if (isOn)
        {
            audioSource.mute = true;
        }
        
    }
    public void ChangeScene()
    {
        if (SceneManager.GetActiveScene().name == "3_GameScene")
        {
            DestroyImmediate(gameObject);
        }
    }
}
