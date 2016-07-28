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

public enum GameDifficulty
{
  Easy,
  Medium,
  Hard
}

[System.Serializable]
public struct GameDifficultySettings
{
  public GameDifficulty m_gameDifficulty;
  public float m_regnerationOxygenNormal;
  public float m_regenerationOxygenRunning;
  public bool m_deathOnNoOxygen;
  public float m_chestOxygenMultiplicatorMin;
  public float m_chestOxygenMultiplicatorMax;
  public float m_regenerationFlashLight;
  public float m_regenerationRadar;
  public float m_chestBatteryMultiplicatorMin;
  public float m_chestBatteryMultiplicatorMax;
  public bool m_itemsRecollectable;
}

public class GameManager : MonoBehaviour
{
  public GameControls m_gameControls;
  public GameDifficulty m_gameDifficulty = GameDifficulty.Easy;
  public bool m_isSaveGame = false;
  public GameDifficultySettings[] m_gameDifficultySettings = new GameDifficultySettings[3];
  
  static bool m_isCreated = false;

  void Awake()
  {
    if (m_isCreated)
    {
      Destroy(this.gameObject);
    }
    m_isCreated = true;

    DontDestroyOnLoad(this.gameObject);
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

  // getter for current difficulty settings
  public GameDifficultySettings CurrentGameDifficultySettings
  {
    get
    {
      return m_gameDifficultySettings[(int)m_gameDifficulty];
    }
  }
}
