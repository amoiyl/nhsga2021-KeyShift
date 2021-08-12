using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public GameObject beatmap;
    //public AudioClip level;
    public float delayBySeconds = 0f;
    public float delayByBeats = 0f;

    public AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        //audioSource = GetComponent<AudioSource>();
        audioSource.clip = Data.instance.music;
        float bpm = beatmap.GetComponent<BeatMapController>().bpm;
        float sumDelay = delayBySeconds + (60/bpm) * delayByBeats;
        audioSource.Play();

        //audioSource.PlayDelayed(sumDelay);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetSong(AudioClip setSong)
    {
        audioSource.clip = setSong;
    }

    public void Play()
    {
        //Debug.Log("play function");
        audioSource.Play();
    }
}
