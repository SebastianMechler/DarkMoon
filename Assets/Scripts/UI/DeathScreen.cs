using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
  public void OnClick_DeathScreen_NewGame()
  {
    SingletonManager.AudioManager.Play(AudioType.UI_BUTTON_CLICK);
    SceneManager.LoadScene(StringManager.Scenes.game);
    SingletonManager.MouseManager.SetMouseState(MouseState.LOCKED);
  }

  public void OnClick_DeathScreen_MainMenu()
  {
    SingletonManager.AudioManager.Play(AudioType.UI_BUTTON_CLICK);
    SceneManager.LoadScene(StringManager.Scenes.mainMenu);
    SingletonManager.MouseManager.SetMouseState(MouseState.UNLOCKED);
  }
}