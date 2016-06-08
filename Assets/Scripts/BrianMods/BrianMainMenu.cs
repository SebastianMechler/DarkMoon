using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class BrianMainMenu : MonoBehaviour
{
  public void OnClick_MainMenu_NewGame()
  {
    SceneManager.LoadScene(BrianStringManager.Scenes.game);
    BrianSingletonManager.MouseManager.SetMouseState(MouseState.LOCKED);
    BrianSingletonManager.GameManager.SetGameState(GameState.INGAME);
  }

  public void OnClick_MainMenu_Exit()
  {
    Application.Quit();
  }
}
