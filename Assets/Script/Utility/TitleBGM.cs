using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 使っていない
/// </summary>
public class TitleBGM : SingletonMonoBehaviour<TitleBGM>
{
    private AudioSource _audioSource;
    new void Awake()
    {
        base.Awake();
        _audioSource = GetComponent<AudioSource>();
    }

    public void PlayMusic()
    {
        if (_audioSource.isPlaying) return;
        _audioSource.Play();
    }

    public void StopMusic()
    {
        _audioSource.Stop();
    }
}
