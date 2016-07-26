using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class OptionScreen : MonoBehaviour
{
  public Slider m_difficultySlider;
  public Text m_difficultyText;

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
    SingletonManager.AudioManager.Play(AudioType.UI_BUTTON_CLICK);
    SingletonManager.MouseManager.SetMouseState(MouseState.UNLOCKED);
    SingletonManager.GameManager.SetGameState(GameState.MENU);
    SceneManager.LoadScene(StringManager.Scenes.mainMenu);
  }
}