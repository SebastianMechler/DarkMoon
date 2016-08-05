using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ImageFlasher : MonoBehaviour
{
  public GameObject[] m_gameObjects;

  public float m_flashDuration = 5.0f; // normal duration of the effect
  public float m_flashSpeedFactor = 1.0f; // the higher the value the faster the blinking, 0.5 = slow, 2.0f = fast

  private float m_blinkDuration = 0.0f; // timer for fast/slow blinking
  private float m_flashTime = 0.0f; // effect timer (defines how long the effect will last)

  private float m_initialAlpha = 255.0f;
  
	void Update()
  {
    if (m_flashTime == 0.0f)
    {
      return;
    }

    if (m_flashTime > 0.0f)
    {
      m_flashTime -= Time.deltaTime;

      if (m_flashTime < 0.0f)
      {
        m_flashTime = 0.0f;
        ResetAlpha();
        return;
      }
    }

    if (m_blinkDuration > 0.0f)
    {
      FadeAlpha();      

      m_blinkDuration -= Time.deltaTime * m_flashSpeedFactor;

      if (m_blinkDuration < 0.0f)
      {
        m_blinkDuration = 0.0f;
      }
    }
	}

  void FadeAlpha()
  {
    float lerpValue = Mathf.Lerp(0.0f, 255.0f, m_blinkDuration % 1.0f) / 255.0f; // flash alpha value from 0.0f to 255.0f

    for (int i = 0; i < m_gameObjects.Length; i++)
    {
      Image image = m_gameObjects[i].GetComponent<Image>();

      if (image != null)
      {
        Color color = image.color;
        color.a = lerpValue; // color value ranges from 0.0f to 1.0f so devision by 255.0f is required
        image.color = color;
      }
    }
  }

  void ResetAlpha()
  {
    for (int i = 0; i < m_gameObjects.Length; i++)
    {
      Image image = m_gameObjects[i].GetComponent<Image>();

      if (image != null)
      {
        Color color = image.color;
        color.a = m_initialAlpha; // color value ranges from 0.0f to 1.0f so devision by 255.0f is required
        image.color = color;
      }
    }
  }

  public void Run(float time = -1.0f)
  {
    if (time > 0.0f)
    {
      m_blinkDuration = time;
      m_flashTime = time;
    }
    else
    {
      m_blinkDuration = m_flashDuration;
      m_flashTime = m_flashDuration;
    }
  }
}
