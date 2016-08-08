using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;

public class OptionScreen : MonoBehaviour
{
  public Slider m_difficultySlider;
  public Text m_difficultyText;

  public Slider m_sliderSound;
  public Text m_soundText;

  public Slider m_sliderMusic;
  public Text m_musicText;

  public Slider m_sliderGamma;
  public Text m_gammaText;

  public Dropdown m_resolution;
  public Text m_resolutionText;

  public Toggle m_toggleTutorial;

  private bool m_isSliderInitialized = false;

  private bool isInitialized = false;

  void Start()
  {
    m_difficultySlider.value = (float)SingletonManager.GameManager.m_gameDifficulty;
    m_difficultyText.text = "Difficulty: " + SingletonManager.GameManager.m_gameDifficulty.ToString();
    m_isSliderInitialized = true;

    // update slider values   
    m_sliderMusic.value = SingletonManager.GameManager.m_settings.m_musicVolume;
    m_sliderSound.value = SingletonManager.GameManager.m_settings.m_soundVolume;
    m_difficultySlider.value = (int)SingletonManager.GameManager.m_gameDifficulty;
    
    // Add all available resolution modes to the dropdown button
    m_resolution.ClearOptions();

    List<Dropdown.OptionData> optionList = new List<Dropdown.OptionData>();
    Resolution[] resolutions = Screen.resolutions;
    for (int i = 0; i < resolutions.Length; i++)
    {
      string resolution = resolutions[i].ToString();
      string[] split = resolution.Split('@');
      optionList.Add(new Dropdown.OptionData(split[0]));
    }

    m_resolution.AddOptions(optionList);

    // set tutorial state
    m_toggleTutorial.isOn = SingletonManager.GameManager.m_isTutorial;

    // enable sound for onvaluechange
    isInitialized = true;
  }

  public void OnValueChange_Slider_Difficulty()
  {
    if (m_isSliderInitialized == false)
    {
      return;
    }

    if (isInitialized)
    {
      SingletonManager.AudioManager.Play(AudioType.UI_BUTTON_CLICK);
    }
    GameDifficulty difficulty = (GameDifficulty)m_difficultySlider.value;
    Debug.Log(difficulty.ToString());
    m_difficultyText.text = "Difficulty: " + difficulty.ToString();
    SingletonManager.GameManager.SetGameDifficulty(difficulty);
  }

  public void OnClick_SuccessScreen_MainMenu()
  {
    UIManager.SwitchToMainMenu();
  }

  public void OnValueChange_Slider_SoundVolume()
  {
    SingletonManager.GameManager.m_settings.m_soundVolume = m_sliderSound.value;
    m_soundText.text = "Sound-Volume: " + m_sliderSound.value.ToString("0.00");

    if (isInitialized)
    {
      SingletonManager.AudioManager.Play(AudioType.UI_BUTTON_CLICK);
    }
    
  }

  public void OnValueChange_Slider_MusicVolume()
  {
    SingletonManager.GameManager.m_settings.m_musicVolume = m_sliderMusic.value;
    m_musicText.text = "Music-Volume: " + m_sliderMusic.value.ToString("0.00");
    if (isInitialized)
    {
      SingletonManager.AudioManager.Play(AudioType.UI_BUTTON_CLICK);
    }
  }

  public void OnValueChange_Slider_CameraGamma()
  {
    // DISABLED for now

    //SingletonManager.GameManager.m_settings.m_cameraGamma = m_sliderGamma.value;
    //m_gammaText.text = "Gamma: " + m_sliderGamma.value.ToString("0.00");
  }


  public void OnValueChange_DropDown_Resolutions()
  {
    if (isInitialized)
    {
      SingletonManager.AudioManager.Play(AudioType.UI_BUTTON_CLICK);
    }
    Resolution[] resolutions = Screen.resolutions;
    Screen.SetResolution(resolutions[0].width, resolutions[0].height, Screen.fullScreen);    
  }

  public void OnValueChange_Toggle_Tutorial()
  {
    SingletonManager.GameManager.m_isTutorial = m_toggleTutorial.isOn;
  }

}