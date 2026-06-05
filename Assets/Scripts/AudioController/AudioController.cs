using UnityEngine;
using System.Collections.Generic;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance;

    [Header("Data")]
    public SoundFXData soundFXData;
    public MusicBGData musicBGData;

    [Header("Audio Sources")]
    public AudioSource musicSource;
    public AudioSource sfxSource;

    private Dictionary<SoundType, AudioClip> sfxDict;
    private Dictionary<SoundType, AudioClip> musicDict;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            Init();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Init()
    {
        // ===== Sound FX =====
        sfxDict = new Dictionary<SoundType, AudioClip>();
        foreach (var sfx in soundFXData.soundFXList)
        {
            if (!sfxDict.ContainsKey(sfx.type))
                sfxDict.Add(sfx.type, sfx.clip);
        }

        // ===== Music =====
        musicDict = new Dictionary<SoundType, AudioClip>();
        foreach (var music in musicBGData.musicList)
        {
            if (!musicDict.ContainsKey(music.type))
                musicDict.Add(music.type, music.clip);
        }
    }

    // ========= MUSIC =========
    public void PlayMusic(SoundType type, bool loop = true)
    {
        if (!musicDict.ContainsKey(type))
        {
            Debug.LogWarning("Music not found: " + type);
            return;
        }

        musicSource.clip = musicDict[type];
        musicSource.loop = loop;
        musicSource.Play();
    }

    public void StopMusic()
    {
        musicSource.Stop();
    }

    // ========= SOUND FX =========
    public void PlaySFX(SoundType type)
    {
        if (!sfxDict.ContainsKey(type))
        {
            Debug.LogWarning("SFX not found: " + type);
            return;
        }

        sfxSource.PlayOneShot(sfxDict[type]);
    }
}