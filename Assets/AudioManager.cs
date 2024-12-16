using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }

    [SerializeField] private AudioScriptable audioScript;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }


        foreach (Sound s in audioScript.SoundDatabase)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume;
            s.source.pitch = s.pitch;
            s.source.loop = s.loop;
            s.source.playOnAwake = s.playOnAwake;
        }
    }

    public void Play(string name)
    {
        Sound s = Array.Find(audioScript.SoundDatabase.ToArray(), sound => sound.title == name);
        if(s == null)
        {
            Debug.Log("Sound " + name + " Not Found");
            return;
        }

        s.source.Play();
    }

    public bool CheckPlaying(string name)
    {
        Sound s = Array.Find(audioScript.SoundDatabase.ToArray(), sound => sound.title == name);
        if (s == null)
        {
            Debug.Log("Sound " + name + " Not Found");
            return false;
        }

        return s.source.isPlaying;
    }

    public void Stop(string name)
    {
        Sound s = Array.Find(audioScript.SoundDatabase.ToArray(), sound => sound.title == name);
        if (s == null)
        {
            Debug.Log("Sound " + name + " Not Found");
            return;
        }
        s.source.Stop();
    }

}
