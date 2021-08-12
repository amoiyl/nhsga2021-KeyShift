using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public float bpm;
    public float secondsPerBeat;
    public float position;
    public float positionInBeats;
    public float dspTime;

    public AudioSource tutorial;

    void Start()
    {
        secondsPerBeat = 60.0f / bpm;
        dspTime = (float) AudioSettings.dspTime;
        tutorial.Play();
    }

    void Update()
    {
        position = (float) AudioSettings.dspTime - dspTime;
        positionInBeats = position / secondsPerBeat;
    }
}
