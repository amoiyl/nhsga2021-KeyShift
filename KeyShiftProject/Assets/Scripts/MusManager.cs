using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusManager : MonoBehaviour
{
    public AudioSource mmMusic;

    private IEnumerator loopaudio;

    private static MusManager instance = null;
    public static MusManager Instance
    {
        get { return instance; }
    }

    private void Start()
    {
       
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
        loopaudio = LoopAudio();
        if (!mmMusic.isPlaying)
        {
            StartCoroutine(loopaudio);
        }
    }

    public void StopMusic()
    {
        StopCoroutine(loopaudio);
        mmMusic.Stop();
    }

    private IEnumerator LoopAudio()
    {
        float length = mmMusic.clip.length;

        while (true)
        {
            mmMusic.Play();
            yield return new WaitForSeconds(length - 5f);
        }
    }

}
