using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Audio : MonoBehaviour
{
    [Header(" ------------ Audio Source ------------ ")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource SFXSource;

    [Header(" ------------ Audio Clip ------------ ")]
    public AudioClip Background;
    public AudioClip BrickTouch;
    public AudioClip Checkpoint;
    public AudioClip Death;
    public AudioClip OtherPlayerTouch;
    public AudioClip Artefacto;
    public AudioClip Quest;
    public AudioClip Inventory;


    public void Start()
    {
        musicSource.clip = Background;
        musicSource.Play();
    }

    public void PlaySFX(AudioClip clip)
    {
        SFXSource.PlayOneShot(clip);
    }
}

