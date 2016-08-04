using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerOxygen : MonoBehaviour
{
    [Tooltip("[0.0f to max] Defines the maximum amount of oxygen a player can hold.")]
    public float m_max = 100.0f; // how much oxygen the player can have //= min would be 0

    [Tooltip("[0.0f to [Max] - see above] Defines how much oxygen the player will start with, also shows the current amount.")]
    public float m_current = 0.0f; // how much oxygen the player currently has

    [Tooltip("[0.0f to max] Defines how much oxygen will be lost each frame (play around with this).")]
    public float m_regenerateStep = -1.0f; // regenerate oxygen each frame

    [Tooltip("[0.0f to 1.0f] At this percentage the GrayScale-Effect will take start to fade in.")]
    public float m_grayScaleEffectStart = 0.2f; // percentage when the GrayScale will start to fáde in (range from 0.0f to 1.0f)
                                                //public float m_grayScaleIntensity = 0.3f; // value which indicates how strong the grayscale effect should appear based on oxygen range from 0.0f to 0.5f the higher the value the more grayScale will be effected from beginning
    
    // GrayScale m_grayScale = null;
    GrayScaleManager m_grayScaleManager = null;
    void Start()
    {
      m_grayScaleManager = SingletonManager.GrayScaleManager;
      UpdateCosts();
    }

    void Update()
    {
        if (GetPercentage() < m_grayScaleEffectStart)
        {
            m_grayScaleManager.Enable();
        }
        else
        {
            m_grayScaleManager.Disable();
        }

        //m_grayScale.m_intensity = GetPercentageInversed();
        Regenerate();
    }

    void Regenerate()
    {
        Increase(m_regenerateStep * Time.deltaTime);
    }

    public void Increase(float value)
    {
        if (m_current <= 0.0f)
        {
            if (value > 0.0f)
            {
                SingletonManager.AudioManager.Play(AudioType.OXYGEN_RECHARGE);
            }
        }

        bool isZeroPreviously = m_current <= 0.0f ? true : false;

        if (m_current + value > m_max)
            m_current = m_max;
        else if (m_current + value < 0.0f)
            m_current = 0.0f;
        else
            m_current += value;

        if (m_current == 0.0f && isZeroPreviously == false && SingletonManager.GameManager.CurrentGameDifficultySettings.m_deathOnNoOxygen)
        {
            SingletonManager.AudioManager.Play(AudioType.OXYGEN_OUT);

            SingletonManager.MouseManager.SetMouseState(MouseState.UNLOCKED);
            SceneManager.LoadScene(StringManager.Scenes.deathScreen);
        }
    }

    /*
    float GetPercentageInversed()
    {
      if (m_current != m_max)
      {
        return 1.0f + m_grayScaleIntensity - m_current / m_max;
      }
      else
      {
        return 1.0f - m_current / m_max;
      }
    }
    */

    public float GetPercentage()
    {
        return m_current / m_max;
    }

  public void UpdateCosts()
  {
    m_regenerateStep = SingletonManager.GameManager.CurrentGameDifficultySettings.m_regnerationOxygenNormal;
  }
}
