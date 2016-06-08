using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
  public void OnClick_MainMenu_NewGame()
  {
    SceneManager.LoadScene(StringManager.Scenes.game);
    SingletonManager.MouseManager.SetMouseState(MouseState.LOCKED);
    SingletonManager.GameManager.SetGameState(GameState.INGAME);
  }

  public void OnClick_MainMenu_Exit()
  {
    Application.Quit();
  }
}
