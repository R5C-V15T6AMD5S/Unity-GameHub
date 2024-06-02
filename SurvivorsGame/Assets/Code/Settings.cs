using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

namespace Code
{
    public class Settings : MonoBehaviour
    {
        [SerializeField]
        private AudioMixer audioMixer;

        public TMP_Dropdown resolutionDropdown;

        private Resolution[] _resolutions;

        public void Start() //used to populate the resolution dropdown with the available resolutions
        {
            _resolutions = Screen.resolutions;
        
            resolutionDropdown.ClearOptions();
            var options = new List<string>();
            var currentResolutionIndex = 0;
            //var uniqueResolutions = new HashSet<string>();

            foreach (var resolution in _resolutions)
            {
                var resolutionOption = resolution.width + "x" + resolution.height;
                options.Add(resolutionOption);
                if (resolution.width == Screen.currentResolution.width && resolution.height == Screen.currentResolution.height)
                {
                    currentResolutionIndex = options.Count - 1;
                }
            }
            resolutionDropdown.AddOptions(options);
            resolutionDropdown.value = currentResolutionIndex;
            resolutionDropdown.RefreshShownValue();
        }

        public void GoToMainMenu() 
        {
            SceneManager.LoadScene("MainMenu");
        }

        public void SetVolume(float volume) 
        {
            Debug.Log(volume);
            audioMixer.SetFloat("Volume", volume);
        }
    
        public void SetQuality(int qualityIndex)
        {
            QualitySettings.SetQualityLevel(qualityIndex);
        }
    
        public void SetFullscreen(bool isFullscreen)
        {
            Screen.fullScreen = isFullscreen;
        }
    
        public void SetResolution(int resolutionIndex)
        {
            var resolution = _resolutions[resolutionIndex];
            Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }
    }
}
