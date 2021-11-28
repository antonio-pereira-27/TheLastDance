using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    // VARIABLES
    private float sensivity;
    private static float volume;
    
    // REFERENCES
    private Resolution[] _resolutions;
    public TMP_Dropdown _dropdown;
    public TMP_InputField sensivityInputField;

    public AudioManager _audioManager;
    public Slider volumeSlider;

    private int currentResolutionIndex = 0;
    private void Start()
    {
        sensivity = MouseLook.mouseSensivity;
        sensivityInputField.text = sensivity.ToString();

        _audioManager = FindObjectOfType<AudioManager>();
        //Debug.Log(_audioManager);

        _resolutions = Screen.resolutions;
        _dropdown.ClearOptions();
        List<string> options = new List<string>();

        for (int i = 0; i < _resolutions.Length; i++)
        {
            string option = _resolutions[i].width + " x " + _resolutions[i].height;
            options.Add(option);

            if (_resolutions[i].width == Screen.currentResolution.width &&
                _resolutions[i].height == Screen.currentResolution.height)
                currentResolutionIndex = i;
        }
        
        _dropdown.AddOptions(options);
        _dropdown.value = currentResolutionIndex;
        _dropdown.RefreshShownValue();
    }


    public void SetVolume()
    {
        try
        {
            //volumeSlider.value = _audioManager.volume;
            volume = volumeSlider.value;
            foreach (Sound s in _audioManager.sounds)
            {
                if (s.name == "Background")
                {
                    s.audioSource.volume = volume;
                }
            }
        }
        catch (Exception e)
        {
            Debug.Log("Error: " + e);
        }
        
    }

    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen(bool isFullScreen)
    {
        Screen.fullScreen = isFullScreen;
    }

    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = _resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void ChangeSensivity()
    {
        sensivity = Single.Parse(sensivityInputField.text);
        MouseLook.mouseSensivity = sensivity;
    }
    
    public void BackButton()
    {
        //FindObjectOfType<AudioManager>().Play("Button");
        SceneManager.LoadScene("StartMenu");
    }
}
