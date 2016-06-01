using UnityEngine;
using System.Collections;

public class PlayerBattery : MonoBehaviour
{
  [Tooltip("[0.0f to max] Defines the maximum amount of battery a player can hold.")]
  public float m_max = 100.0f; // how much oxygen the player can have //= min would be 0

  [Tooltip("[0.0f to [Max] - see above] Defines how much battery the player will start with, also shows the current amount.")]
  public float m_current = 0.0f; // how much oxygen the player currently has

  [Tooltip("[0.0f to max] Defines how much battery will be lost when flashLight is being used (play around with this).")]
  public float m_regenerateStep = -1f; // regenerate oxygen each frame

  FlashLight m_flashLight = null;

  void Start()
  {
    m_flashLight = FlashLight.GetInstance();
  }

  void Update()
  {
    if (m_flashLight.m_hasBeenPickedUp && m_flashLight.GetState() == FlashLightState.ON)
    {
      Increase(m_regenerateStep * Time.deltaTime); // will increase by negative value ==> decrease

      if (m_current <= 0.0f)
      {
        // Disable FlashLight
        m_flashLight.SetState(FlashLightState.OFF);
      }
    }
  }
  
  public void Increase(float value)
  {
    if (m_current + value > m_max)
      m_current = m_max;
    else if (m_current + value < 0.0f)
      m_current = 0.0f;
    else
      m_current += value;
  }

  public float GetPercentage()
  {
    return m_current / m_max;
  }

  public bool HasBattery()
  {
    return m_current > 0.0f;
  }
}
