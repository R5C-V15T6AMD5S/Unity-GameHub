using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class OptionMenuSTG : MonoBehaviour
{
    Resolution[] resolutions;
    public Dropdown resolutionDropdown;
    int currentResolutionIndex = 0;

    // Because you cant just apply resolutions,
    // first we need to format them and apply them
    void Start() {
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        for(int i = 0; i < resolutions.Length; i++) {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if(resolutions[i].width == Screen.currentResolution.width && 
                resolutions[i].height == Screen.currentResolution.height) {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue(); // Just applying the resolution doesnt nothing, has tio be refreshed
    }

    // Gets the resolutions index from list
    public void SetResolution(int resolutionIndex) {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    // Simple bool for the checkbox
    public void SetFullscreen(bool isFullscreen) {
        Screen.fullScreen = isFullscreen;
    }

    // Similar for the fullscreen
    public void EnableSounds(bool soundsEnabled) {
        if(soundsEnabled) {
            AudioListener.volume = 1;
        } else {
            AudioListener.volume = 0;
        }
    }
}
