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

        prevSoundBgmValue = 1f;
        prevSoundMasterValue = 1f;
        prevSoundMasterValue = 1f;
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

            // 사운드가 끝나면 반환
            Managers.Coroutine.StartCoroutine(name, ReturnSourceWhenFinished(source, clip.length));
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



    public void SetSFXVolume()
    {
        if (masterMixer == null) return;
        masterMixer.SetFloat("SFX", LinearToDecibel(prevSoundSfxValue));
    }


    public void SetSFXVolume(float volume)
    {
        if (masterMixer == null) return;
        masterMixer.SetFloat("SFX", LinearToDecibel(volume));
    }


    public void SetBGMVolume()
    {
        if (masterMixer == null) return;
        masterMixer.SetFloat("BGM", LinearToDecibel(prevSoundBgmValue));
    }


    public void SetBGMVolume(float volume)
    {
        if (masterMixer == null) return;
        masterMixer.SetFloat("BGM", LinearToDecibel(volume));
    }



    public void SetMasterVolume(float volume)
    {
        if (masterMixer == null) return;
        masterMixer.SetFloat("Master", LinearToDecibel(volume));
    }

    public void SetMasterVolume()
    {
        if (masterMixer == null) return;

        Debug.Log(prevSoundMasterValue);
        masterMixer.SetFloat("Master", LinearToDecibel(prevSoundMasterValue));
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

    public void TransitionToSnapshot(string snapshotName, float transitionTime)
    {
        AudioMixerSnapshot snapshot = masterMixer.FindSnapshot(snapshotName);
        if (snapshot != null)
        {
            snapshot.TransitionTo(transitionTime);
        }
        else
        {
            Debug.LogWarning($"Snapshot '{snapshotName}' not found!");
        }
    }
}