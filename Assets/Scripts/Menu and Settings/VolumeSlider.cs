using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace MyFPS.GameAdmin
{
    public class VolumeSlider : MonoBehaviour
    {
        public AudioSource audio;
        public Slider slider;

        public void SetVolume(float _value)
        {
            audio.volume = _value;
            PlayerPrefs.SetFloat("Volume", _value);
        }
        private void Awake()
        {            
            audio.volume = PlayerPrefs.GetFloat("Volume", slider.maxValue);
            slider.value = audio.volume;
        }
    }
}