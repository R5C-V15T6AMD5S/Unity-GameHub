using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PlaySound : MonoBehaviour
{
    public AudioSource audioSource;
    public void PlayThisSoundEffect() 
    {
        audioSource.Play();
    }
}
