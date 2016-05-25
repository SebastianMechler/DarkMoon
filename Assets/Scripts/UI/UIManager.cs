using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum UIType
{
  MainTerminal,
}

public class UIManager : MonoBehaviour
{

  PlayerOxigen m_playerOxigen = null;
  Scrollbar m_oxigenScrollBar = null;

  void Start()
  {
    m_playerOxigen = SingletonManager.Player.GetComponent<PlayerOxigen>();
    m_oxigenScrollBar = GetOxygen().GetComponent<Scrollbar>();
  }

  void Update()
  {
    // Update Oxygen bar
    m_oxigenScrollBar.size = m_playerOxigen.GetPercentage();
  }

  public void SetUIVisibility(UIType type, bool isVisisble)
  {
    Canvas canvas = null;

    switch (type)
    {
      case UIType.MainTerminal:
        canvas = GetMainTerminal().GetComponent<Canvas>();
        break;
    }

    if (canvas != null)
    {
      canvas.enabled = isVisisble;
    }
    else
    {
      Debug.Log("Canvas is null...");
    }
  }

  public GameObject GetMainTerminal()
  {
    return this.transform.FindChild(StringManager.UI.MainTerminal).gameObject;
  }

  public GameObject GetOxygen()
  {
    return GameObject.Find(StringManager.UI.OxygenBackground).gameObject;
  }

  public static UIManager GetInstance()
  {
    return GameObject.Find(StringManager.Names.uiManager).GetComponent<UIManager>();
  }
}
