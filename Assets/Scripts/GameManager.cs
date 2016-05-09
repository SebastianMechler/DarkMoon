using UnityEngine;
using System.Collections;

// This class contains Game-Settings
[System.Serializable]
public struct GameControls
{
    public KeyCode forward;
    public KeyCode backward;
    public KeyCode left;
    public KeyCode right;
    public KeyCode run;
    public KeyCode crouch;
    public KeyCode interactWithObject;
    public KeyCode flashLight;
}


public class GameManager : MonoBehaviour
{
    public GameControls m_gameControls;

    public static GameManager GetInstance()
    {
        return GameObject.Find(StringManager.Names.gameManager).GetComponent<GameManager>();
    }
}
