using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DeathScreen : MonoBehaviour
{
  public void OnClick_DeathScreen_NewGame()
  {
    UIManager.NewGame();
  }

  public void OnClick_DeathScreen_MainMenu()
  {
    UIManager.SwitchToMainMenu();
  }
}