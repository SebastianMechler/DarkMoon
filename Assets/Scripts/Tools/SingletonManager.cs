using UnityEngine;
using System.Collections;

public class SingletonManager : MonoBehaviour
{
    private static MouseManager m_mouseManager = null;
    private static GameManager m_gameManager = null;

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
}
