using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SuccessScreen : MonoBehaviour
{
  
  public void OnClick_SuccessScreen_NewGame()
  {
    SingletonManager.AudioManager.Play(AudioType.UI_BUTTON_CLICK);
    SingletonManager.MouseManager.SetMouseState(MouseState.LOCKED);
    SingletonManager.GameManager.SetGameState(GameState.INGAME);
    SceneManager.LoadScene(StringManager.Scenes.game);
  }

  public void OnClick_SuccessScreen_MainMenu()
  {
    SingletonManager.AudioManager.Play(AudioType.UI_BUTTON_CLICK);
    SingletonManager.MouseManager.SetMouseState(MouseState.UNLOCKED);
    SingletonManager.GameManager.SetGameState(GameState.MENU);
    SceneManager.LoadScene(StringManager.Scenes.mainMenu);
  }
}