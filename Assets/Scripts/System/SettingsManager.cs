using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    public SceneLoader sceneLoader;
    public GameObject settingsPanel;
    public GameObject confirmReturnPanel; // Just for asking if the player wants to return to main menu when in level (will not save game)
    public Slider musicVolumeSlider;
    public Toggle musicMuteToggle;

    private void Awake()
    {
        Assert.IsNotNull(sceneLoader);
        Assert.IsNotNull(settingsPanel);
        Assert.IsNotNull(confirmReturnPanel);
        Assert.IsNotNull(musicVolumeSlider);
        Assert.IsNotNull(musicMuteToggle);
    }

    private void Start()
    {
        musicVolumeSlider.value = GlobalControl.Instance.savedValues.MusicVolume;
        musicMuteToggle.isOn = GlobalControl.Instance.savedValues.MusicMuted;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (!settingsPanel.gameObject.activeSelf)
            {
                OpenSettings();   
            }
            else
            {
                CloseSettings();
            }
        }
    }

    public void OnLoadScene(string sceneName)
    {
        sceneLoader.LoadScene(sceneName);
    }

    public void ConfirmReturn()
    {
        confirmReturnPanel.SetActive(true);
    }

    public void ButtonCancelReturn()
    {
        confirmReturnPanel.SetActive(false);
    }

    public void ToggleMusic()
    {
        GlobalControl.Instance.savedValues.MusicMuted = musicMuteToggle.isOn;
    }

    public void SliderVolume()
    {
        GlobalControl.Instance.savedValues.MusicVolume = musicVolumeSlider.value;
    }

    public void OpenSettings()
    {
        GameManager.Pause();
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
        ButtonCancelReturn();
        GameManager.Unpause();
    }
}
