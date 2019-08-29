using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class CAudioManager : MonoBehaviour
{
    public static CAudioManager Inst
    {
        get
        {
            if (_inst == null)
            {
                GameObject obj = Resources.Load<GameObject>("AudioManager"); //cargo referencia del proyect
                return Instantiate<GameObject>(obj).GetComponent<CAudioManager>(); //instancio la referencia
            }

            return _inst;
        }
    }
    private static CAudioManager _inst;
    private Dictionary<string, AudioClip> _audios;

    private List<AudioSource> _sfxSources;
    private AudioSource _musicSource;

    private float _musicVolume = 1;
    private float _sfxVolume = 1;

    [SerializeField]
    private AudioMixer _masterMixer;
    [SerializeField]
    private AudioMixerGroup _musicGroup;
    [SerializeField]
    private AudioMixerGroup _sfxGroup;

    public void Awake()
    {
        if(_inst != null && _inst != this)
        {
            Destroy(this.gameObject);
            return;
        }
        _inst = this;
        DontDestroyOnLoad(this.gameObject);

        //init
        _audios = new Dictionary<string, AudioClip>();
        _sfxSources = new List<AudioSource>();

        AudioClip[] clips = Resources.LoadAll<AudioClip>("Audio/");
        //Debug.Log("Hay " + clips.Length + " audios");
        for(int i = 0; i < clips.Length;i++)
        {
            _audios.Add(clips[i].name, clips[i]);
        }
    }

    public void LateUpdate()
    {
        for(int i = _sfxSources.Count-1 ; i >= 0; i--)
        {
            AudioSource source = _sfxSources[i];
            if(source == null || !source.isPlaying)
            {
                if(source != null)
                    Destroy(source.gameObject);
                _sfxSources.RemoveAt(i);
            }
        }
    }

    public void PlayMusic(string name, bool loops = true, Transform parent = null, float spatialBlend = 0)
    {
        if (!_audios.ContainsKey(name))
            return;

        if (_musicSource == null)
            CreateMusicSource();

        _musicSource.clip = _audios[name];
        _musicSource.volume = _musicVolume;
        _musicSource.loop = loops;
        _musicSource.transform.SetParent(parent);
        _musicSource.spatialBlend = spatialBlend;

        _musicSource.Play();
    }

    public AudioSource PlaySFX(string name, bool loops = false, Transform parent = null, bool playSingle = false, float randomPitch = 0, float spatialBlend = 0)
    {
        if (!_audios.ContainsKey(name))
            return null;

        if (playSingle)
            DestroyAllSFXWith(name);

        AudioSource sfxSource = CreateSFXSource();
        sfxSource.clip = _audios[name];
        sfxSource.volume = _sfxVolume;
        sfxSource.loop = loops;
        sfxSource.transform.SetParent(parent);
        sfxSource.spatialBlend = spatialBlend;
        sfxSource.pitch += Random.Range(-1f, 1f) * randomPitch;
        sfxSource.Play();
        _sfxSources.Add(sfxSource);
        return sfxSource;
    }

    public void StopMusic()
    {
        _musicSource.Stop();
    }

    public void StopSFX()
    {
        for(int i = 0; i < _sfxSources.Count;i++)
        {
            if (_sfxSources[i] != null)
                _sfxSources[i].Stop();
        }
    }

    public void PauseMusic()
    {
        _musicSource.Pause();
    }

    public void ResumeMusic()
    {
        _musicSource.UnPause();
    }

    public void PauseSFX()
    {
        for (int i = 0; i < _sfxSources.Count; i++)
        {
            if (_sfxSources[i] != null)
                _sfxSources[i].Pause();
        }
    }

    public void ResumeSFX()
    {
        for (int i = 0; i < _sfxSources.Count; i++)
        {
            if (_sfxSources[i] != null)
                _sfxSources[i].UnPause();
        }
    }

    public void SetMusicVolume(float v)
    {
        _musicVolume = v;
        UpdateVolumes();
    }

    public void SetSFXVolume(float v)
    {
        _sfxVolume = v;
        UpdateVolumes();
    }

    private void CreateMusicSource()
    {
        GameObject obj = new GameObject("Music Source");
        _musicSource = obj.AddComponent<AudioSource>();
        DontDestroyOnLoad(obj);

        _musicSource.outputAudioMixerGroup = _musicGroup;
        _musicSource.spatialBlend = 0;
        _musicSource.playOnAwake = false;
    }

    private AudioSource CreateSFXSource()
    {
        GameObject obj = new GameObject("SFX Source");
        AudioSource source = obj.AddComponent<AudioSource>();
        source.outputAudioMixerGroup = _sfxGroup;
        source.playOnAwake = false;
        return source;
    }

    private void UpdateVolumes()
    {
        for (int i = 0; i < _sfxSources.Count; i++)
        {
            if (_sfxSources[i] != null)
                _sfxSources[i].volume = _sfxVolume;
        }
        if (_musicSource == null)
            CreateMusicSource();
        _musicSource.volume = _musicVolume;
    }

    private void DestroyAllSFXWith(string name)
    {
        for (int i = _sfxSources.Count-1; i >= 0 ; i--)
        {
            if (_sfxSources[i] != null && _sfxSources[i].clip.name == name)
            {
                Destroy(_sfxSources[i].gameObject);
                _sfxSources.RemoveAt(i);
            }
        }
    }
}
