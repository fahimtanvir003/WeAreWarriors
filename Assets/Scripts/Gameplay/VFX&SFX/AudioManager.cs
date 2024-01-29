using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private Sound[] _sounds;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(instance);
        }

        InitializeAudioSource();
    }

    
    private void InitializeAudioSource()
    {
        foreach (var sound in _sounds)
        {
            sound.audioSource = gameObject.AddComponent<AudioSource>();
            sound.audioSource.clip = sound.clip;
            sound.audioSource.loop = sound.loop;
            sound.audioSource.volume = sound.volume;
        }
    }

    public void PlaySound(string _name)
    {
        Sound s = Array.Find(_sounds, sound => sound.soundName == _name);
        if (s == null)
        {
            Debug.LogWarning("Couldn't Find Audio");
            return;
        }

        s.audioSource.Play();
    }

    [Serializable]
    private class Sound 
    { 
        public string soundName;
        public AudioClip clip;
        public float volume;
        public bool loop;
        [HideInInspector] public AudioSource audioSource;
    }

}
