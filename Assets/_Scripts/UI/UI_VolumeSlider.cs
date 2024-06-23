using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class UI_VolumeSlider : MonoBehaviour
{

    public Slider slider;
    public string parameter;

    [SerializeField] AudioMixer audioMixer;
    [SerializeField] float multiplier;

    public void SliderValue(float _value)
    {
        audioMixer.SetFloat(parameter, Mathf.Log10(_value) * multiplier);//_value[0.001, 1] -> Mathf.Log10(_value)[-3, 0]
    }
}
