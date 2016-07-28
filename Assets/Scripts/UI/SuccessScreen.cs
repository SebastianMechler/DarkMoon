using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SuccessScreen : MonoBehaviour
{
  
  public void OnClick_SuccessScreen_NewGame()
  {
    UIManager.NewGame();
  }

  public void OnClick_SuccessScreen_MainMenu()
  {
    UIManager.SwitchToMainMenu();
  }
}