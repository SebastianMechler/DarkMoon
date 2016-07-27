using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Exit : MonoBehaviour
{
  void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.tag == StringManager.Names.player)
    {
      if (SingletonManager.MainTerminalController.IsAllDataActivated())
      {
        SingletonManager.MouseManager.SetMouseState(MouseState.UNLOCKED);
        SceneManager.LoadScene(StringManager.Scenes.successScreen);
      }
    }
  }
}
