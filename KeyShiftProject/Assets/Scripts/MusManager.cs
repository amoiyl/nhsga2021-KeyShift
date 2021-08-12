using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusManager : MonoBehaviour
{
    public AudioSource mmMusic;

    private static MusManager instance = null;
    public static MusManager Instance
    {
        get { return instance; }
    }
    void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }
        else
        {
            instance = this;
        }
        DontDestroyOnLoad(this.gameObject);
    }

    public void PlayMusic()
    {
        if (!mmMusic.isPlaying)
        {
            StartCoroutine(LoopAudio());
        }
    }

    public void StopMusic()
    {
        StopCoroutine(LoopAudio());
        mmMusic.Stop();
    }

    IEnumerator LoopAudio()
    {
        float length = mmMusic.clip.length;

        while (true)
        {
            mmMusic.Play();
            yield return new WaitForSeconds(length);
        }
    }
}
