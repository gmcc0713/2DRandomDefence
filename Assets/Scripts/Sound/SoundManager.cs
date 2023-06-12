
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[System.Serializable]
public enum Sound_Type { Sound_BGM = 0, Sound_SFX,Sound_Character,Sound_Type_Count }
public enum BGM_Num { BGM_MenuScene = 0, BGM_PlayScene }
public enum SFX_Num { Click_Button = 0, CountDown,Win,Lose}
public enum CharacterSound_Num { Unit_Attack_Sword = 0, Unit_Attack_Bow, Unit_Attack_Masic}
public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    [SerializeField] private AudioSource[] audioSources;

    [SerializeField] private AudioClip[] audioBGMClips;
    [SerializeField] private AudioClip[] audioSFXClips;
    [SerializeField] private AudioClip[] audioCharacterClips;
    private float[] volumeValue;
    public float GetVolumValue(Sound_Type type) => volumeValue[(int)type];
    void Start()
    {

        audioSources[(int)Sound_Type.Sound_BGM].clip = audioBGMClips[(int)BGM_Num.BGM_MenuScene];
        audioSources[(int)Sound_Type.Sound_BGM].Play();
        volumeValue = new float[3] { 0.5f, 0.5f, 0.5f };


    }
    public void Initialize()
    {
    }
    // Update is called once per frame
    public void PlayAudioClipOneShot(Sound_Type sound_Type,int clip_num)
    {
        switch (sound_Type)
        {
            case Sound_Type.Sound_Character:
                audioSources[(int)sound_Type].PlayOneShot(audioCharacterClips[(int)clip_num]);
                break;
            case Sound_Type.Sound_SFX:
                audioSources[(int)sound_Type].PlayOneShot(audioSFXClips[(int)clip_num]);
                break;
        }
    }
    public void ChangeAudioVolume(Sound_Type sound_Type,float value)
    {
        audioSources[(int)sound_Type].volume = value;
        volumeValue[(int)sound_Type] = value;
    }
    public void StopBGM()
    {
        audioSources[(int)Sound_Type.Sound_BGM].Stop();
    }
    void OnEnable()
    {
        // 씬 매니저의 sceneLoaded에 체인을 건다.
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    // 체인을 걸어서 이 함수는 매 씬마다 호출된다.
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Initialize();

        if (scene.name == "MenuScene")
        {
            audioSources[(int)Sound_Type.Sound_BGM].clip = audioBGMClips[(int)BGM_Num.BGM_MenuScene];
        }
        else if(scene.name == "PlayScene")
        {
            audioSources[(int)Sound_Type.Sound_BGM].clip = audioBGMClips[(int)BGM_Num.BGM_PlayScene];
        }

        audioSources[(int)Sound_Type.Sound_BGM].Play();
    }

    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
