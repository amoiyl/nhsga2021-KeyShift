using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsController : MonoBehaviour
{
    public AudioMixer audioMixer;
    public Slider musicSlider;
    public Slider sfxSlider;

    public AudioSource escapeClip;

    private void Start()
    {
        musicSlider.value = Data.instance.musicValue;
        sfxSlider.value = Data.instance.sfxValue;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            escapeClip.Play();
            SceneManager.LoadScene("MainMenu");
        }
    }

    public void SetVolume(float volume)
    {
        Data.instance.musicValue = musicSlider.value;
        musicSlider.value = volume;
        audioMixer.SetFloat("Music", musicSlider.value);
    }

    public void SetSFX(float volume)
    {
        Data.instance.sfxValue = sfxSlider.value;
        sfxSlider.value = volume;
        audioMixer.SetFloat("SFX", sfxSlider.value);
    }
}
