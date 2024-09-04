using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class VolumeSlider : MonoBehaviour
{
    [SerializeField] protected AudioMixer audioMixer;
    [SerializeField] protected string audioChannelName;

    private void Start()
    {
        Slider selfSlider = GetComponent<Slider>();

        selfSlider.value = PlayerPrefs.GetFloat(audioChannelName);
        audioMixer.SetFloat(audioChannelName, Mathf.Log10(selfSlider.value) * 20.0f);
    }

    public void OnValueChanged(float value)
    {
        audioMixer.SetFloat(audioChannelName, Mathf.Log10(value) * 20.0f);
        PlayerPrefs.SetFloat(audioChannelName, value);
    }
}
