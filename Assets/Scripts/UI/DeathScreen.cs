using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
  public void OnClick_DeathScreen_NewGame()
  {
    SceneManager.LoadScene(StringManager.Scenes.game);
    SingletonManager.MouseManager.SetMouseState(MouseState.LOCKED);
    SingletonManager.GameManager.SetGameState(GameState.INGAME);
  }

  public void OnClick_DeathScreen_MainMenu()
  {
    SceneManager.LoadScene(StringManager.Scenes.mainMenu);
    SingletonManager.MouseManager.SetMouseState(MouseState.UNLOCKED);
    SingletonManager.GameManager.SetGameState(GameState.MENU);
  }
}