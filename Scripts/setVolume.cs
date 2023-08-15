using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class setVolume : MonoBehaviour
{
    public AudioMixer mixer;

    public void setLevel(float sliderValue)
    {
        mixer.SetFloat("mainVolume", Mathf.Log10(sliderValue) * 20);
    }

}
