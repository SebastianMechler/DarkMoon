using UnityEngine;
using System.Collections;

public class HidingZone : MonoBehaviour
{
  public static bool g_isPlayerHidden = false;
  public bool m_isCrouchingZone = false;

  PlayerMovement m_playerMovement = null;

  float timer = 0.15f;
  float timer_max = 0.15f;

	void Start ()
  {
    m_playerMovement = SingletonManager.Player.GetComponent<PlayerMovement>();
  }

  void Update()
  {
    timer -= Time.deltaTime;

    if (timer <= 0.0f)
    {
      timer = timer_max;
      SingletonManager.UIManager.ToggleHiddenState(g_isPlayerHidden);
    }
  }
  
  void OnTriggerStay(Collider collider)
  {
    if (collider.gameObject.name == StringManager.Names.player)
    {
      if (m_isCrouchingZone)
      {
        if (m_playerMovement.HasMovementState(MovementState.CROUCH))
        {
          g_isPlayerHidden = true;
        }
        else
        {
          g_isPlayerHidden = false;
        }
      }
      else
      {
        g_isPlayerHidden = true;
      }
    }
  }

  void OnTriggerExit(Collider collider)
  {
    if (collider.gameObject.name == StringManager.Names.player)
    {
      g_isPlayerHidden = false;
    }
  }
}
