using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;
    public AudioSource audioSource,bgm;
    public AudioClip jumpAudio, hurtAudio, eatAudio;
    
    public void Awake()
    {
        instance = this;
    }
    public void JumpAudio()
    {
        audioSource.clip = jumpAudio;
        audioSource.Play();        
    }
    public void HurtAudio()
    {
        audioSource.clip = hurtAudio;
        audioSource.Play();        
    }
    public void EatAudio()
    {
        audioSource.clip = eatAudio;
        audioSource.Play();        
    }
    public void Bgm()
    {
        bgm.Pause();
    }
}
