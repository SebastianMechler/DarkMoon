using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum BrianUIType
{
  MainTerminal,
  TerminalOne,
  TerminalTwo,
  TerminalThree,
  MainMenu,
  PauseMenu,
}

public class BrianUIManager : MonoBehaviour
{

  PlayerOxygen m_playerOxygen = null;
  Scrollbar m_oxygenScrollBar = null;

  PlayerBattery m_playerBattery = null;
  Scrollbar m_batteryScrollBar = null;
  
  void Start()
  {
    // oxygen
    m_playerOxygen = BrianSingletonManager.Player.GetComponent<PlayerOxygen>();
    m_oxygenScrollBar = GetOxygen().GetComponent<Scrollbar>();

    // battery
    m_playerBattery = BrianSingletonManager.Player.GetComponent<PlayerBattery>();
    m_batteryScrollBar = GetBattery().GetComponent<Scrollbar>();
  }

  void Update()
  {
    UpdateIngameUI();

    // toggle pauseMenu
    if (Input.GetKeyDown(BrianSingletonManager.GameManager.m_gameControls.ui_togglePauseMenu))
    {
      TogglePauseMenu();
    }
  }

  public void SetUIVisibility(UIType type, bool isVisisble)
  {
    Canvas canvas = GetUIObject(type).GetComponent<Canvas>();
    
    if (canvas != null)
    {
      canvas.enabled = isVisisble;
    }
    else
    {
      Debug.Log("Canvas is null...");
    }
  }

  public bool GetUIVisibility(UIType type)
  {
    Canvas canvas = GetUIObject(type).GetComponent<Canvas>();

    if (canvas != null)
    {
      return canvas.enabled;
    }

    return false;
  }

  public GameObject GetUIObject(UIType type)
  {
    switch (type)
    {
      case UIType.MainTerminal:
        return this.transform.FindChild(BrianStringManager.UI.MainTerminal).gameObject;
      case UIType.TerminalOne:
        return this.transform.FindChild(BrianStringManager.UI.TerminalOne).gameObject;
      case UIType.TerminalTwo:
        return this.transform.FindChild(BrianStringManager.UI.TerminalTwo).gameObject;
      case UIType.TerminalThree:
        return this.transform.FindChild(BrianStringManager.UI.TerminalThree).gameObject;
      case UIType.PauseMenu:
        return this.transform.FindChild(BrianStringManager.UI.PauseMenu).gameObject;
    }

    return null;
  }

  public void UpdateIngameUI()
  {
    // Update Oxygen bar
    m_oxygenScrollBar.size = m_playerOxygen.GetPercentage();

    // update battery bar
    m_batteryScrollBar.size = m_playerBattery.GetPercentage();
  }

  public GameObject GetOxygen()
  {
    return GameObject.Find(BrianStringManager.UI.OxygenBackground).gameObject;
  }

  public GameObject GetBattery()
  {
    return GameObject.Find(BrianStringManager.UI.BatteryBackground).gameObject;
  }

  public void EnableBatteryUI()
  {
    Text text = GameObject.Find(BrianStringManager.UI.BatteryText).gameObject.GetComponent<Text>();
    text.enabled = true;

    Image imageBackground = GameObject.Find(BrianStringManager.UI.BatteryBackground).gameObject.GetComponent<Image>();
    imageBackground.enabled = true;

    Image imageForeground = GameObject.Find(BrianStringManager.UI.BatteryForgeround).gameObject.GetComponent<Image>();
    imageForeground.enabled = true;
  }
  
  public void TogglePauseMenu()
  {
    bool isVisisble = BrianSingletonManager.BrianUIManager.GetUIVisibility(UIType.PauseMenu);

    if (isVisisble == false)
    {
      Time.timeScale = 0.0f;
      BrianSingletonManager.MouseManager.SetMouseState(MouseState.UNLOCKED);
    }
    else
    {
      Time.timeScale = 1.0f;
      BrianSingletonManager.MouseManager.SetMouseState(MouseState.LOCKED);
    }

    BrianSingletonManager.BrianUIManager.SetUIVisibility(UIType.PauseMenu, !isVisisble);
  }

  //
  // PAUSE MENU
  //
  public void OnClick_PauseMenu_ButtonNewGame()
  {
    Time.timeScale = 1.0f;
    BrianSingletonManager.MouseManager.SetMouseState(MouseState.LOCKED);
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
  }

  public void OnClick_PauseMenu_Resume()
  {
    TogglePauseMenu();
  }

  public void OnClick_PauseMenu_ButtonExit()
  {
    Application.Quit();
  }

  public void OnClick_PauseMenu_MainMenu()
  {
    Time.timeScale = 1.0f;
    SceneManager.LoadScene(BrianStringManager.Scenes.mainMenu);
  }
  
  public static BrianUIManager GetInstance()
  {
    return GameObject.Find(BrianStringManager.Names.uiManager).GetComponent<BrianUIManager>();
  }
}
