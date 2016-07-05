using UnityEngine;
using System.Collections;

public class HidingZone : MonoBehaviour
{
  public static bool g_isPlayerHidden = false;
  public bool m_isCrouchingZone = false;
  public float m_disableOnNoiseTime = 15.0f; // time in seconds
  float m_disableOnNoiseTimer = 0.0f; // timer which calculates the time

  PlayerMovement m_playerMovement = null;

  float m_timer = 0.15f;
  float m_timerMax = 0.15f;

  bool m_isEnabled = true;

	void Start ()
  {
    m_playerMovement = SingletonManager.Player.GetComponent<PlayerMovement>();
  }

  void Update()
  {
    m_timer -= Time.deltaTime;

    if (m_timer <= 0.0f)
    {
      m_timer = m_timerMax;
      SingletonManager.UIManager.ToggleHiddenState(g_isPlayerHidden);
      //SingletonManager.UIManager.ToggleHiddenState(true);
    }

    // timer is running?
    if (m_disableOnNoiseTimer > 0.0f)
    {
      // reduce time
      m_disableOnNoiseTimer -= Time.deltaTime;

      // timer ends?
      if (m_disableOnNoiseTimer <= 0.0f)
      {
        // allow hiding
        m_isEnabled = true;
      }
    }
  }
  
  void OnTriggerStay(Collider collider)
  {
    if (m_isEnabled && collider.gameObject.name == StringManager.Names.player)
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

  public void OnNoiseTrigger()
  {
    m_disableOnNoiseTimer = m_disableOnNoiseTime;
    g_isPlayerHidden = false;
    m_isEnabled = false;
  }
}
