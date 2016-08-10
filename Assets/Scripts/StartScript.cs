using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class StartScript : MonoBehaviour
{
  public Image m_backgroundStart;
  public Text m_textStart;

  private float m_delayStartInternal = 0.15f;

  public float m_startWait = 5.0f;
  private float m_eyeTimer = 0.0f;

  private float m_secondPassed = 0.0f;

  

	void Start ()
  {
    if (SingletonManager.GameManager.m_isSaveGame)
    {
      m_startWait = 0.15f;
    }
  }
	
	void Update ()
  {
    if (m_delayStartInternal > 0.0f)
    {
      m_delayStartInternal -= Time.deltaTime;

      if (m_delayStartInternal < 0.0f)
      {
        m_delayStartInternal = 0.0f;
        // disable ui
        SingletonManager.UIManager.ToggleMinimap(false);
        SingletonManager.UIManager.SetBatteryUIState(false);
        SingletonManager.UIManager.SetCrosshairVisibility(false);
      }
    }

	  if (m_startWait > 0.0f)
    {
      m_startWait -= Time.deltaTime;

      m_secondPassed += Time.deltaTime;

      if (m_secondPassed >= 1.25f)
      {
        m_secondPassed = 0.0f;
        m_textStart.text = m_textStart.text + ".";
      }

      if (m_startWait < 0.0f)
      {
        // run eye effect
        m_startWait = 0.0f;

        if (SingletonManager.GameManager.m_isSaveGame)
        {
          m_eyeTimer = 0.15f;
        }
        else
        {
          m_eyeTimer = 3.0f;
        }
        m_textStart.enabled = false;
      }
    }

    if (m_eyeTimer > 0.0f)
    {
      m_eyeTimer -= Time.deltaTime;

      /*
      // fade
      Vector3 scale = m_backgroundStart.transform.localScale;
      scale *= 0.07f * Time.deltaTime;
      m_backgroundStart.transform.localScale = scale;

      Color color = m_backgroundStart.color;
      color.r -= 0.7f * Time.deltaTime;
      m_backgroundStart.color = color;
      */

      if (m_eyeTimer < 0.0f)
      {
        m_eyeTimer = 0.0f;
    
        // disable background
        m_backgroundStart.enabled = false;
        // run tutorial
        SingletonManager.Tutorial.RunTutorial();

        SingletonManager.Player.GetComponent<CameraRotation>().m_enabled = true;
      }

    }
  }
}
