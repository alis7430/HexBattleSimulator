using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager
{
    private AudioSource[] _audioSources = new AudioSource[(int)Define.Sound.MaxCount];
    private Dictionary<string, AudioClip> _audioClips = new Dictionary<string, AudioClip>();

    public void Init()
    {
        GameObject root = GameObject.Find("@Sound");
        if (root == null)
        {
            root = new GameObject { name = "@Sound" };
            Object.DontDestroyOnLoad(root);

            string[] soundNames = System.Enum.GetNames(typeof(Define.Sound));
            for (int i = 0; i < soundNames.Length - 1; i++)
            {
                GameObject go = new GameObject { name = soundNames[i] };
                _audioSources[i] = go.AddComponent<AudioSource>();
                go.transform.parent = root.transform;
            }

            _audioSources[(int)Define.Sound.BGM].loop = true;
        }
    }

    public void Play(string path, Define.Sound type = Define.Sound.SFX)
    {
        AudioClip audioClip = GetOrAddAudioClip(path, type);
        Play(audioClip, type);
    }

    public void Play(AudioClip audioClip, Define.Sound type = Define.Sound.SFX)
    {
        if (audioClip == null)
            return;

        if (type == Define.Sound.BGM)
        {
            AudioSource audioSource = _audioSources[(int)Define.Sound.BGM];

            if (audioSource.isPlaying)
                audioSource.Stop();

            audioSource.clip = audioClip;
            audioSource.Play();
        }
        else
        {
            AudioSource audioSource = _audioSources[(int)Define.Sound.SFX];
            audioSource.PlayOneShot(audioClip);
        }
    }

    public void Clear()
    {
        foreach (var source in _audioSources)
        {
            source.clip = null;
            source.Stop();
        }
        _audioClips.Clear();
    }

    private AudioClip GetOrAddAudioClip(string path, Define.Sound type = Define.Sound.SFX)
    {
        if (path.Contains("Sounds/") == false)
            path = $"Sounds/{path}";

        AudioClip audioClip;

        if (type == Define.Sound.BGM)
        {
            audioClip = Managers.Resource.Load<AudioClip>(path);
        }
        else
        {
            if (_audioClips.TryGetValue(path, out audioClip) == false)
            {
                audioClip = Managers.Resource.Load<AudioClip>(path);
                _audioClips.Add(path, audioClip);
            }
        }

        if (audioClip == null)
            Debug.Log($"AudioClip Missing ! {path}");

        return audioClip;
    }
}
