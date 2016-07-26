using UnityEngine;
using System.Collections;

// This class contains Game-Settings
[System.Serializable]
public struct GameControls
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
  public KeyCode throwItem;

  // ui
  public KeyCode ui_togglePauseMenu;
  public KeyCode ui_toggleMinimap;
}

public enum GameState
{
  INGAME,
  MENU,
}

public enum GameDifficulty
{
  Easy,
  Medium,
  Hard
}

public class GameManager : MonoBehaviour
{
  public GameControls m_gameControls;
  public GameState m_gameState = GameState.MENU;
  public GameDifficulty m_gameDifficulty = GameDifficulty.Easy;

  static bool m_isCreated = false;

  void Awake()
  {
    if (m_isCreated)
    {
      Destroy(this.gameObject);
    }
    m_isCreated = true;

    DontDestroyOnLoad(this.transform.gameObject);
  }

  public void SetGameState(GameState state)
  {
    m_gameState = state;
  }

  public void SetGameDifficulty(GameDifficulty difficulty)
  {
    m_gameDifficulty = difficulty;
  }

  public static GameManager GetInstance()
  {
      return GameObject.Find(StringManager.Names.gameManager).GetComponent<GameManager>();
  }

  public static void ClearDebugConsole()
  {
	  var logEntries = System.Type.GetType("UnityEditorInternal.LogEntries,UnityEditor.dll");
	  var clearMethod = logEntries.GetMethod("Clear", System.Reflection.BindingFlags.Static | System.Reflection.BindingFlags.Public);
	  clearMethod.Invoke(null, null);
  }
}
