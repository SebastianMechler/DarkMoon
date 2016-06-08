using UnityEngine;
using System.Collections;

// This class contains Game-Settings
[System.Serializable]
public struct BrianGameControls
{
  // ingame
  public KeyCode forward;
  public KeyCode backward;
  public KeyCode left;
  public KeyCode right;
  public KeyCode run;
  public KeyCode crouch;
  public KeyCode interactWithObject;
  public KeyCode flashLight;

  // ui
  public KeyCode ui_togglePauseMenu;
}

public enum BrianGameState
{
  INGAME,
  MENU,
}

public class BrianGameManager : MonoBehaviour
{
  public GameControls m_gameControls;
  public GameState m_gameState = GameState.MENU;

  void Awake()
  {
    DontDestroyOnLoad(this.transform.gameObject);
  }

  public void SetGameState(GameState state)
  {
    m_gameState = state;
  }

  public static BrianGameManager GetInstance()
    {
        return GameObject.Find(BrianStringManager.Names.gameManager).GetComponent<BrianGameManager>();
    }

  public static void ClearDebugConsole()
  {
	  var logEntries = System.Type.GetType("UnityEditorInternal.LogEntries,UnityEditor.dll");
	  var clearMethod = logEntries.GetMethod("Clear", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
	  clearMethod.Invoke(null, null);
  }
}
