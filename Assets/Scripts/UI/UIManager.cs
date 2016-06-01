using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public enum UIType
{
  MainTerminal,
}

public class UIManager : MonoBehaviour
{

  PlayerOxygen m_playerOxygen = null;
  Scrollbar m_oxygenScrollBar = null;

  PlayerBattery m_playerBattery = null;
  Scrollbar m_batteryScrollBar = null;

  void Start()
  {
    // oxigen
    m_playerOxygen = SingletonManager.Player.GetComponent<PlayerOxygen>();
    m_oxygenScrollBar = GetOxygen().GetComponent<Scrollbar>();

    m_playerBattery = SingletonManager.Player.GetComponent<PlayerBattery>();
    m_batteryScrollBar = GetBattery().GetComponent<Scrollbar>();
  }

  void Update()
  {
    // Update Oxygen bar
    m_oxygenScrollBar.size = m_playerOxygen.GetPercentage();

    // update battery bar
    m_batteryScrollBar.size = m_playerBattery.GetPercentage();
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

  public GameObject GetBattery()
  {
    return GameObject.Find(StringManager.UI.BatteryBackground).gameObject;
  }

  public void EnableBatteryUI()
  {
    Text text = GameObject.Find(StringManager.UI.BatteryText).gameObject.GetComponent<Text>();
    text.enabled = true;

    Image imageBackground = GameObject.Find(StringManager.UI.BatteryBackground).gameObject.GetComponent<Image>();
    imageBackground.enabled = true;

    Image imageForeground = GameObject.Find(StringManager.UI.BatteryForgeround).gameObject.GetComponent<Image>();
    imageForeground.enabled = true;
  }

  public static UIManager GetInstance()
  {
    return GameObject.Find(StringManager.Names.uiManager).GetComponent<UIManager>();
  }
}
