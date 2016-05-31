using UnityEngine;
using System.Collections;

public class SingletonManager : MonoBehaviour
{
  private static MouseManager m_mouseManager = null;
  private static GameManager m_gameManager = null;
  private static MainTerminalController m_mainTerminalController = null;
  private static UIManager m_uiManager = null;
  private static GameObject m_player = null;
  private static AudioManager m_audioManager = null;

    public static MouseManager MouseManager
    {
        get
        {
            if (m_mouseManager == null)
            {
                m_mouseManager = MouseManager.GetInstance();
            }
            return m_mouseManager;
        }
    }

    public static GameManager GameManager
    {
        get
        {
            if (m_gameManager == null)
            {
                m_gameManager = GameManager.GetInstance();
            }
            return m_gameManager;
        }
    }

  public static MainTerminalController MainTerminalController
  {
    get
    {
      if (m_mainTerminalController == null)
      {
        m_mainTerminalController = MainTerminalController.GetInstance();
      }
      return m_mainTerminalController;
    }
  }

  public static UIManager UIManager
  {
    get
    {
      if (m_uiManager == null)
      {
        m_uiManager = UIManager.GetInstance();
      }
      return m_uiManager;
    }
  }

  public static GameObject Player
  {
    get
    {
      if (m_player == null)
      {
        m_player = GameObject.Find(StringManager.Names.player);
      }

      return m_player;
    }
  }

  public static AudioManager AudioManager
  {
    get
    {
      if (m_audioManager == null)
      {
        m_audioManager = AudioManager.GetInstance();
      }
      return m_audioManager;
    }
  }
}
