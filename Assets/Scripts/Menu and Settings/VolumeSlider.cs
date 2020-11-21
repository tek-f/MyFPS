using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace MyFPS.GameAdmin
{
    public class VolumeSlider : MonoBehaviour
    {
        /// <summary>
        /// Reference to the main audio source in the scene.
        /// </summary>
        public AudioSource audio;
        /// <summary>
        /// Refefence to the Slider UI element in the settings menu.
        /// </summary>
        public Slider slider;
        /// <summary>
        /// Sets the volume of audio to _value.
        /// </summary>
        /// <param name="_value">Value the volume is set to.</param>
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