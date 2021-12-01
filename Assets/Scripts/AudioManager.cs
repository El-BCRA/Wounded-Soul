using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    public AudioSource[] sounds;
    public AudioSource[] footSteps;
    public AudioSource[] acidBurns;
    public AudioSource[] bubblePops;
    public AudioSource[] enemyHits;
    public AudioSource attack;

    public Vector3 spawnPoint = Vector3.zero;
    public CameraStateMachine.Zone warpZone = CameraStateMachine.Zone.Heart;
    public bool inCutscene = false;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        Play("MainMenuBGM");
    }

    public void Play(string audio)
    {
        if (!inCutscene || audio == "HeartBeat")
        {
            AudioSource s = Array.Find<AudioSource>(sounds, item => item.name == audio);
            if (s == null)
            {
                Debug.LogWarning("Sound: " + name + " not found!");
                return;
            }
            s.Play();
        }
    }
    
    public void PlayFootstep()
    {
        if (!inCutscene)
        {
            foreach (AudioSource t in footSteps)
            {
                if (t.isPlaying)
                {
                    return;
                }
            }
            int i = UnityEngine.Random.Range(0, footSteps.Length);
            float p = UnityEngine.Random.Range(0.9f, 1.25f);
            AudioSource s = footSteps[i];
            s.pitch = p;
            s.Play();
        }
    }

    public void PlayAcid()
    {
        if (!inCutscene)
        {
            int i = UnityEngine.Random.Range(0, acidBurns.Length);
            AudioSource s = acidBurns[i];
            s.Play();
        }
    }

    public void Pop(Vector3 position)
    {
        if (!inCutscene)
        {
            int i = UnityEngine.Random.Range(0, bubblePops.Length);
            AudioSource s = bubblePops[i];
            AudioSource.PlayClipAtPoint(s.clip, position);
        }
    }

    public void EnemyHit()
    {
        if (!inCutscene)
        {
            int i = UnityEngine.Random.Range(0, enemyHits.Length);
            AudioSource s = enemyHits[i];
            s.Play();
        }
    }

    public void Attack()
    {
        if (!inCutscene)
        {
            attack.Play();
        }
    }

    public void Stop()
    {
        foreach (AudioSource s in sounds)
        {
            if (s.isPlaying)
            {
                s.Stop();
            }
        }
    }

    public void LowerVolume()
    {
        foreach (AudioSource s in sounds)
        {
            if (s.isPlaying && s.name != "HeartBeat")
            {
                s.volume -= 0.1f;
            }
        }
    }

    public void RaiseVolume()
    {
        foreach (AudioSource s in sounds)
        {
            if (s.isPlaying && s.volume < 1.0f && s.name != "HeartBeat")
            {
                s.volume += 0.1f;
            }
        }
    }
}
