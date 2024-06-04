using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [Header (" ------------ Audio Source ------------ ")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header(" ------------ Audio Clip ------------ ")]
    public AudioClip Background;
    public AudioClip BrickTouch;
    public AudioClip Checkpoint;
    public AudioClip Death;
    public AudioClip OtherPlayerTouch;
    public AudioClip Artifact;
    public AudioClip Quest;

    private void Start()
    {
        musicSource.clip = Background;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}
