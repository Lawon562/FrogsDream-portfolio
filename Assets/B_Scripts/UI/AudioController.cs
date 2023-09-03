using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//��ũ��Ʈ�� ��� �ֵ� �ڵ������� ����� ã���ش��
[RequireComponent(typeof(Toggle))]

public class AudioController : MonoBehaviour
{
    //�������� �����̴�
    public Slider vol_BGM;

    //���Ұ� ���
    public Toggle bgmToggle;

    //����� ����
    public AudioClip audioClip_bg1; // �׸����
    public AudioClip audioClip_bg2; // �÷��� ���
    private AudioSource audioSource;

    
    void Start()
    {
        //toggle ������Ʈ �����ϱ�
        bgmToggle = GetComponent<Toggle>();

        SoundInit();
        DontDestroyOnLoad(this);
        audioSource = GetComponent<AudioSource>();


    }
    void Update()
    {
        //�����Ӹ��� ���� ����
        audioSource.volume = GameData.bgmVolume;

        //��� üũ�ߴ��� Ȯ���ϱ�
        bgmToggle.onValueChanged.AddListener(delegate {
            BgmOnOFF(bgmToggle);
        });

        //�� �Ѿ�� bgm �ٲٱ�
        ChangeScene();
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

    //���� �� �Ѿ�� �뷡 �ٲٱ�
    public void ChangeScene()
    {
        if (SceneManager.GetActiveScene().name == "3_GameScene")
        {
            DestroyImmediate(gameObject);
        }
    }

}