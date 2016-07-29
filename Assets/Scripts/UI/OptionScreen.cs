using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

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

  private bool m_isSliderInitialized = false;

  void Start()
  {
    m_difficultySlider.value = (float)SingletonManager.GameManager.m_gameDifficulty;
    m_difficultyText.text = "Difficulty: " + SingletonManager.GameManager.m_gameDifficulty.ToString();
    m_isSliderInitialized = true;
  }

  public void OnValueChange_Slider_Difficulty()
  {
    if (m_isSliderInitialized == false)
    {
      return;
    }

    SingletonManager.AudioManager.Play(AudioType.UI_BUTTON_CLICK);
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
  }

  public void OnValueChange_Slider_MusicVolume()
  {
    SingletonManager.GameManager.m_settings.m_musicVolume = m_sliderMusic.value;
    m_musicText.text = "Music-Volume: " + m_sliderMusic.value.ToString("0.00");
  }

  public void OnValueChange_Slider_CameraGamma()
  {
    SingletonManager.GameManager.m_settings.m_cameraGamma = m_sliderGamma.value;
    m_gammaText.text = "Gamma: " + m_sliderGamma.value.ToString("0.00");
  }
}