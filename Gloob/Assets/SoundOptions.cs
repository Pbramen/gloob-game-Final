using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class SoundOptions : MonoBehaviour
{
    public Slider masterSlider;
    public Slider musicSlider;
    public Slider sFxSlider;
    public AudioMixer audioMixer;

    void Start() {
        if (PlayerPrefs.GetInt("set Value First") == 0) {
            PlayerPrefs.SetInt("set Value First", 1);
            masterSlider.value = 0.25f;
            musicSlider.value = 0.25f;
            sFxSlider.value = 0.25f;
        }
        else
        {
            masterSlider.value = PlayerPrefs.GetFloat("MasterVolume");
            musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
            sFxSlider.value = PlayerPrefs.GetFloat("sFXVolume");
        }
    }
    public void setMasterVolume() {
        SetVolume("MasterVolume", masterSlider.value);
    }
    public void setMusicVolume() {
        SetVolume("MusicVolume", musicSlider.value);
    }
    public void setsFXVolume() {
        SetVolume("sFXVolume", sFxSlider.value);
    }
    void SetVolume(string name, float value) {
        float volume = Mathf.Log(value) * 20;
        if (value == 0) {
            volume = -80; 
        }
        audioMixer.SetFloat(name, volume);
        PlayerPrefs.SetFloat(name, value);

    }
}
