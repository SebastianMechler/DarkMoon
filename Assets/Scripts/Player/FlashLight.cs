﻿using UnityEngine;
using System.Collections;

// enables or disables the flashLight
public enum FlashLightState
{
  ON,
  OFF  
}

public class FlashLight : MonoBehaviour
{
  public static bool m_isEnabled = false; // will be set to true if the item is picked up
  public FlashLightState m_flashState = FlashLightState.OFF; // controls the current state of the flashLight
  private Light m_light; // reference to light component to switch it off or on through 'Range'
  private float m_range; // backup the range, because it will be set to 0 if off, and to this range if on

  void Start()
  {
    m_light = GetComponent<Light>();
    m_range = m_light.range; // make a backup of the starting range
    m_light.range = 0.0f; // set light range on start to 0.0f
  }

	void Update ()
  {
	  if (m_isEnabled)
    {
      if (Input.GetKeyDown(SingletonManager.GameManager.m_gameControls.flashLight))
      {
        if (m_flashState == FlashLightState.OFF)
        {
          // enable flashLight
          m_light.range = m_range;
          m_flashState = FlashLightState.ON;
        }
        else
        {
          // disable flashLight
          m_light.range = 0;
          m_flashState = FlashLightState.OFF;
        }
      }
    }
	}
}
