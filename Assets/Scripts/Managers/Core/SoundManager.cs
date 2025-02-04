using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : IManager
{
    private Dictionary<string, AudioClip> sfxClips = new Dictionary<string, AudioClip>();
    private Dictionary<string, AudioClip> bgmClips = new Dictionary<string, AudioClip>();

    private AudioSource _bgmSource;
    public AudioSource BgmSource
    {
        get
        {
            if (_bgmSource == null)
            {
                GameObject bgmSourceObject = new GameObject { name = "@BGM" };
                _bgmSource = bgmSourceObject.AddComponent<AudioSource>();
                _bgmSource.outputAudioMixerGroup = masterMixer?.FindMatchingGroups("BGM")[0];
                _bgmSource.loop = true;
                Object.DontDestroyOnLoad(bgmSourceObject);
            }
            return _bgmSource;
        }
    }
    private AudioSourcePool audioSourcePool;
    
    private AudioMixer masterMixer;

   

    private int defaultCapacity = 10;
    private int maxSize = 20;
    
    private bool initialized;

    protected bool isSoundOn;
    protected float prevSoundSfxValue;
    protected float prevSoundBgmValue;
    protected float prevSoundMasterValue;

    public bool IsSoundOn
    {
        get { return isSoundOn; }
        set { isSoundOn = value; }
    }

    public float PrevSoundSfxValue
    {
        get { return prevSoundSfxValue; }
        set { prevSoundSfxValue = value; }
    }

    public float PrevSoundBgmValue
    {
        get { return prevSoundBgmValue; }
        set { prevSoundBgmValue = value; }
    }

    public float PrevSoundMasterValue
    {
        get { return prevSoundMasterValue; }
        set { prevSoundMasterValue = value; }
    }


    public void Init()
    {
        if (initialized) return;
        
        initialized = true;
        masterMixer = Managers.Resource.Load<AudioMixer>("Sounds/MasterMixer");
        AudioClip[] bgms = Managers.Resource.LoadAll<AudioClip>("Sounds/BGM");

        foreach (var clip in bgms)
            bgmClips.Add(clip.name, clip);

        Debug.Log($"BGM Loaded Count : {bgmClips.Count}");

        AudioClip[] sfxs = Managers.Resource.LoadAll<AudioClip>("Sounds/SFX");

        foreach (var clip in sfxs)
            sfxClips.Add(clip.name, clip);

        Debug.Log($"SFX Loaded Count : {sfxClips.Count}");


        var sfxGroup = masterMixer?.FindMatchingGroups("SFX")[0];
        // AudioSource 풀 생성
        audioSourcePool = new AudioSourcePool(
            masterMixerGroup: sfxGroup,
            defaultCapacity: defaultCapacity,
            maxSize: maxSize
        );

        isSoundOn = PlayerPrefs.GetInt("IsSoundOn", 1) == 1;
        prevSoundBgmValue = PlayerPrefs.GetFloat("BGMVolume", 1f);
        prevSoundMasterValue = PlayerPrefs.GetFloat("MasterVolume", 1f);
        prevSoundMasterValue = PlayerPrefs.GetFloat("SFXVolume", 1f);
    }
    public void Clear()
    {
        audioSourcePool?.Clear();
    }

    // 위치 기반 SFX 재생
    public void PlaySFX(string name, Vector3 position)
    {
        if (sfxClips.TryGetValue(name, out AudioClip clip))
        {
            AudioSource source = audioSourcePool.Pop();
            source.transform.position = position;
            source.clip = clip;
            source.Play();
            
            string clipName = $"{clip.name}_{Managers.GetNextSequence()}";
            Debug.Log($"Play SFX : {clipName}");

            // 사운드가 끝나면 반환
            Managers.Coroutine.StartCoroutine(clipName, ReturnSourceWhenFinished(source, clip.length));
        }
        else
        {
            Debug.LogWarning($"SFX '{name}' not found!");
        }
    }

    private System.Collections.IEnumerator ReturnSourceWhenFinished(AudioSource source, float delay)
    {
        yield return new WaitForSeconds(delay);
        audioSourcePool.Push(source);
    }

    public void ToggleSound(bool isOn)
    {
        IsSoundOn = isOn;
        PlayerPrefs.SetInt("IsSoundOn", isOn ? 1 : 0);
    }

    // BGM 재생 (전역 사운드)
    public void PlayBGM(string name)
    {
        if (bgmClips.TryGetValue(name, out AudioClip clip))
        {
            BgmSource.clip = clip;
            BgmSource.Play();
        }
        else
        {
            Debug.LogWarning($"BGM '{name}' not found!");
        }
    }

    public void StopBGM()
    {
        BgmSource.Stop();
    }

    public void SetSFXVolume(float volume, bool isSave = true)
    {
        if (masterMixer == null) return;
        masterMixer.SetFloat("SFX", LinearToDecibel(volume));
        if (isSave) PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    public void SetBGMVolume(float volume, bool isSave = true)
    {
        if (masterMixer == null) return;
        masterMixer.SetFloat("BGM", LinearToDecibel(volume));
        if (isSave) PlayerPrefs.SetFloat("BGMVolume", volume);
    }

    public void SetMasterVolume(float volume, bool isSave = true)
    {
        if (masterMixer == null) return;
        masterMixer.SetFloat("Master", LinearToDecibel(volume));
        if (isSave) PlayerPrefs.SetFloat("MasterVolume", volume);
    }


    private float LinearToDecibel(float linear)
    {
        float dB;
        if (linear != 0)
            dB = 20f * Mathf.Log10(linear);
        else
            dB = -80f; // 음소거 수준
        return dB;
    }
}