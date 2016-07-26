using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
  float m_waitTimeExitGame = 0.0f;

  public void OnClick_MainMenu_NewGame()
  {
    SingletonManager.AudioManager.Play(AudioType.UI_BUTTON_CLICK);
    SceneManager.LoadScene(StringManager.Scenes.game);
    SingletonManager.MouseManager.SetMouseState(MouseState.LOCKED);
    SingletonManager.GameManager.SetGameState(GameState.INGAME);
    Time.timeScale = 1.0f;
  }

  public void OnClick_MainMenu_Exit()
  {
    SingletonManager.AudioManager.Play(AudioType.UI_BUTTON_CLICK);
    m_waitTimeExitGame = 1.0f;
  }

  public void OnClick_MainMenu_Option()
  {
    Debug.Log("Loading...");
    SingletonManager.AudioManager.Play(AudioType.UI_BUTTON_CLICK);
    SceneManager.LoadScene(StringManager.Scenes.optionScreen);
    Time.timeScale = 1.0f;
  }

  void Update()
  {
    if (m_waitTimeExitGame > 0.0f)
    {
      m_waitTimeExitGame -= Time.fixedDeltaTime;

      if (m_waitTimeExitGame < 0.0f)
      {
        m_waitTimeExitGame = 0.0f;
        Application.Quit();
      }
    }
  }
}
