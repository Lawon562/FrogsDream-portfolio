using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//��ũ��Ʈ�� ��� �ֵ� �ڵ������� ����� ã���ش��
[RequireComponent(typeof(Toggle))]

public class Bgm2Controller : MonoBehaviour
{
    //�������� �����̴�
    public Slider vol_BGM;

    //���Ұ� ���
    public Toggle bgm2Toggle;

    //����� ����
    public AudioClip audioClip_bg2; //�÷��� ���

    private AudioSource audioSource;


    void Start()
    {
        //toggle ������Ʈ �����ϱ�
        bgm2Toggle = GetComponent<Toggle>();

        SoundInit();
        DontDestroyOnLoad(this);
        audioSource = GetComponent<AudioSource>();


    }
    void Update()
    {
        //�����Ӹ��� ���� ����
        audioSource.volume = GameData.bgmVolume;

        //��� üũ�ߴ��� Ȯ���ϱ�
        bgm2Toggle.onValueChanged.AddListener(delegate {
            BgmOnOFF(bgm2Toggle);
        });
        
    }

    //slider�� ������ ������ GameData�� �����ϱ�
    void SoundInit()
    {
        vol_BGM.minValue = 0f;
        vol_BGM.maxValue = 1f;
        vol_BGM.value = GameData.bgmVolume;
    }

    //GameData�� ������ �������� �ҷ�����
    public void Change_vol_BGM()
    {
        GameData.bgmVolume = vol_BGM.value;
    }

    //toggle ���� �� mute
    public void BgmOnOFF(bool isOn)
    {
        if (isOn)
        {
            audioSource.mute = true;
        }

    }

}