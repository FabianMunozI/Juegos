using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class MenuEscape : MonoBehaviour
{

    public AudioMixer audioMixer;
    public TMPro.TMP_Dropdown resolutionDropdown;
    public TMPro.TMP_Dropdown qualityDropdown;
    public TMPro.TMP_Dropdown textureDropdown;
    public TMPro.TMP_Dropdown aaDropdown;
    public Slider volumeSlider;
    float currentVolume;
    Resolution[] resolutions;

    private GameObject graficas;
    private GameObject escapeMenu;
    private bool menuAbierto;

    // Start is called before the first frame update
    void Start()
    {
        graficas = transform.GetChild(0).gameObject;
        graficas.SetActive(false);
        escapeMenu = transform.GetChild(1).gameObject;
        escapeMenu.SetActive(false);

        menuAbierto = false;

        resolutionDropdown.ClearOptions();
        List<string> options = new List<string>();
        resolutions = Screen.resolutions;
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + 
                    resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width 
                && resolutions[i].height == Screen.currentResolution.height)
                currentResolutionIndex = i;
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.RefreshShownValue();
        LoadSettings(currentResolutionIndex);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape) && !menuAbierto){
            menuAbierto=true;
            escapeMenu.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            GameObject.Find("Player").GetComponent<FpsCamera>().enabled = false;
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && menuAbierto){
            BackToGame();
        }
    }

    public void BackToGame()
    {
        menuAbierto=false;
        graficas.SetActive(false);
        escapeMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        GameObject.Find("Player").GetComponent<FpsCamera>().enabled = true;
    }

    public void OptionMenu()
    {
        menuAbierto=true;
        escapeMenu.SetActive(false);
        graficas.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    public void BackToMenu(){
        //GameObject.Find("EventSystem").GetComponent<EventSystem>().enabled=false;;
        TransitionManager.Instance.LoadScene(TransitionManager.SCENE_NAME_GAME_MENU);
    }

    public void BackToOptions(){
        menuAbierto=true;
        graficas.SetActive(false);
        escapeMenu.SetActive(true);
    }

    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("Volume", volume);
        currentVolume = volume;
    }
    public void SetFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }
    public void SetResolution(int resolutionIndex)
    {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, 
                resolution.height, Screen.fullScreen);
    }
    public void SetTextureQuality(int textureIndex)
    {
        QualitySettings.masterTextureLimit = textureIndex;
        qualityDropdown.value = 6;
    }
    public void SetAntiAliasing(int aaIndex)
    {
        QualitySettings.antiAliasing = aaIndex;
        qualityDropdown.value = 6;
    }
    public void SetQuality(int qualityIndex)
    {
        if (qualityIndex != 6) // if the user is not using 
                                //any of the presets
            QualitySettings.SetQualityLevel(qualityIndex);
        
        switch (qualityIndex)
        {
            case 0: // quality level - very low
                textureDropdown.value = 3;
                aaDropdown.value = 0;
                break;
            case 1: // quality level - low
                textureDropdown.value = 2;
                aaDropdown.value = 0;
                break;
            case 2: // quality level - medium
                textureDropdown.value = 1;
                aaDropdown.value = 0;
                break;
            case 3: // quality level - high
                textureDropdown.value = 0;
                aaDropdown.value = 0;
                break;
            case 4: // quality level - very high
                textureDropdown.value = 0;
                aaDropdown.value = 1;
                break;
            case 5: // quality level - ultra
                textureDropdown.value = 0;
                aaDropdown.value = 2;
                break;
        }
            
        qualityDropdown.value = qualityIndex;
    }
    public void ExitGame()
    {
        Application.Quit();
    }
    public void SaveSettings()
    {
        PlayerPrefs.SetInt("QualitySettingPreference", 
                qualityDropdown.value);
        PlayerPrefs.SetInt("ResolutionPreference", 
                resolutionDropdown.value);
        PlayerPrefs.SetInt("TextureQualityPreference", 
                textureDropdown.value);
        PlayerPrefs.SetInt("AntiAliasingPreference", 
                aaDropdown.value);
        PlayerPrefs.SetInt("FullscreenPreference", 
                Convert.ToInt32(Screen.fullScreen));
        PlayerPrefs.SetFloat("VolumePreference", 
                currentVolume); 
    }
    public void LoadSettings(int currentResolutionIndex)
    {
        if (PlayerPrefs.HasKey("QualitySettingPreference"))
            qualityDropdown.value = 
                        PlayerPrefs.GetInt("QualitySettingPreference");
        else
            qualityDropdown.value = 3;
        if (PlayerPrefs.HasKey("ResolutionPreference"))
            resolutionDropdown.value = 
                        PlayerPrefs.GetInt("ResolutionPreference");
        else
            resolutionDropdown.value = currentResolutionIndex;
        if (PlayerPrefs.HasKey("TextureQualityPreference"))
            textureDropdown.value = 
                        PlayerPrefs.GetInt("TextureQualityPreference");
        else
            textureDropdown.value = 0;
        if (PlayerPrefs.HasKey("AntiAliasingPreference"))
            aaDropdown.value = 
                        PlayerPrefs.GetInt("AntiAliasingPreference");
        else
            aaDropdown.value = 1;
        if (PlayerPrefs.HasKey("FullscreenPreference"))
            Screen.fullScreen = 
            Convert.ToBoolean(PlayerPrefs.GetInt("FullscreenPreference"));
        else
            Screen.fullScreen = true;
        if (PlayerPrefs.HasKey("VolumePreference"))
            volumeSlider.value = 
                        PlayerPrefs.GetFloat("VolumePreference");
        else
            volumeSlider.value = 
                        PlayerPrefs.GetFloat("VolumePreference");
    }
}
