using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CollisionDetection : MonoBehaviour
{
  void OnTriggerEnter(Collider other)
  {
    if (StringManager.Tags.player.Equals(other.gameObject.tag))
    {
      //GameManager.ClearDebugConsole();
      //Debug.Log(" †††† The Player just died a bit ††††");

      // Player died, load deathscreen
      Time.timeScale = 1.0f;
      SingletonManager.MouseManager.SetMouseState(MouseState.UNLOCKED);
      SceneManager.LoadScene(StringManager.Scenes.deathScreen);
      
    }
  }
}
