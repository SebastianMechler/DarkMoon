using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum UIType
{
  MainTerminal,
  TerminalOne,
  TerminalTwo,
  TerminalThree,
  MainMenu,
  PauseMenu,
}

public class UIManager : MonoBehaviour
{

  PlayerOxygen m_playerOxygen = null;
  Scrollbar m_oxygenScrollBar = null;

  PlayerBattery m_playerBattery = null;
  Scrollbar m_batteryScrollBar = null;
  
  void Start()
  {
    // oxygen
    m_playerOxygen = SingletonManager.Player.GetComponent<PlayerOxygen>();
    m_oxygenScrollBar = GetOxygen().GetComponent<Scrollbar>();

    // battery
    m_playerBattery = SingletonManager.Player.GetComponent<PlayerBattery>();
    m_batteryScrollBar = GetBattery().GetComponent<Scrollbar>();
  }

  void Update()
  {
    UpdateIngameUI();

    // toggle pauseMenu
    if (Input.GetKeyDown(SingletonManager.GameManager.m_gameControls.ui_togglePauseMenu))
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
        return this.transform.FindChild(StringManager.UI.MainTerminal).gameObject;
      case UIType.TerminalOne:
        return this.transform.FindChild(StringManager.UI.TerminalOne).gameObject;
      case UIType.TerminalTwo:
        return this.transform.FindChild(StringManager.UI.TerminalTwo).gameObject;
      case UIType.TerminalThree:
        return this.transform.FindChild(StringManager.UI.TerminalThree).gameObject;
      case UIType.PauseMenu:
        return this.transform.FindChild(StringManager.UI.PauseMenu).gameObject;
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
  
  public void TogglePauseMenu()
  {
    bool isVisisble = SingletonManager.UIManager.GetUIVisibility(UIType.PauseMenu);

    if (isVisisble == false)
    {
      Time.timeScale = 0.0f;
      SingletonManager.MouseManager.SetMouseState(MouseState.UNLOCKED);
    }
    else
    {
      Time.timeScale = 1.0f;
      SingletonManager.MouseManager.SetMouseState(MouseState.LOCKED);
    }

    SingletonManager.UIManager.SetUIVisibility(UIType.PauseMenu, !isVisisble);
  }

  public void ToggleMinimap(bool state)
  {
    // Circle
    Image imgCircle = GameObject.Find(StringManager.UI.MinimapCirclePanel).gameObject.GetComponent<Image>();
    imgCircle.enabled = state;

    RawImage rawImage = GameObject.Find(StringManager.UI.MinimapCircle).gameObject.GetComponent<RawImage>();
    rawImage.enabled = state;

    Image circleOutline = GameObject.Find(StringManager.UI.MinimapCircleOutline).gameObject.GetComponent<Image>();
    circleOutline.enabled = state;

    // Rect
    //RawImage imageRect = GameObject.Find(StringManager.UI.MinimapRect).gameObject.GetComponent<RawImage>();
    //imageRect.enabled = state;
  }

  public void SetMinimapTransparency(float transparency)
  {
    RawImage rawImage = GameObject.Find(StringManager.UI.MinimapCircle).gameObject.GetComponent<RawImage>();
    Color color = rawImage.color;
    color.a = transparency;
    rawImage.color = color;

    Image circleOutline = GameObject.Find(StringManager.UI.MinimapCircleOutline).gameObject.GetComponent<Image>();
    color = circleOutline.color;
    color.a = transparency;
    circleOutline.color = color;
  }

  public void ToggleHiddenState(bool state)
  {
    Image imgCircle = GameObject.Find(StringManager.UI.HiddenState).gameObject.GetComponent<Image>();
    //if (imgCircle.enabled == state)
      //return;

    imgCircle.enabled = state;
  }

  //
  // PAUSE MENU
  //
  public void OnClick_PauseMenu_ButtonNewGame()
  {
    SingletonManager.AudioManager.Play(AudioType.UI_BUTTON_CLICK);
    Time.timeScale = 1.0f;
    SingletonManager.MouseManager.SetMouseState(MouseState.LOCKED);
    SceneManager.LoadScene(SceneManager.GetActiveScene().name);
  }

  public void OnClick_PauseMenu_Resume()
  {
    SingletonManager.AudioManager.Play(AudioType.UI_BUTTON_CLICK);
    TogglePauseMenu();
  }

  public void OnClick_PauseMenu_ButtonExit()
  {
    SingletonManager.AudioManager.Play(AudioType.UI_BUTTON_CLICK);
    Application.Quit();
  }

  public void OnClick_PauseMenu_MainMenu()
  {
    SingletonManager.AudioManager.Play(AudioType.UI_BUTTON_CLICK);
    Time.timeScale = 1.0f;
    SceneManager.LoadScene(StringManager.Scenes.mainMenu);
  }
  
  public static UIManager GetInstance()
  {
    return GameObject.Find(StringManager.Names.uiManager).GetComponent<UIManager>();
  }
}
