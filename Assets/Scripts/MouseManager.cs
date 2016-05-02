using UnityEngine;
using System.Collections;

// Description:
// Class to show/hide the mouse cursor in game

public enum MouseState
{
    LOCKED,
    UNLOCKED,
    NONE
}

public class MouseManager : MonoBehaviour
{
    private MouseState m_currentMouseState = MouseState.NONE;

    void Start()
    {
        // enable mouse lock by default => needs to be done later when game starts
        SetMouseState(MouseState.LOCKED);
    }

    void Update()
    {
        // just for programmers to switch between mouse-states
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (m_currentMouseState == MouseState.LOCKED)
            {
                SetMouseState(MouseState.UNLOCKED);
            }
            else
            {
                SetMouseState(MouseState.LOCKED);
            }
        }
    }

    public void SetMouseState(MouseState state)
    {
        if (state == MouseState.LOCKED)
        {
#if DEBUG
            Debug.Log("Locking Mouse...");
#endif

            UnityEngine.Cursor.visible = false;
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
        }
        else if (state == MouseState.UNLOCKED)
        {
#if DEBUG
            Debug.Log("Unlocking Mouse...");
#endif
            UnityEngine.Cursor.visible = true;
            UnityEngine.Cursor.lockState = CursorLockMode.None;
        }

        // update current state
        m_currentMouseState = state;
    }

    public MouseState GetMouseState()
    {
        return this.m_currentMouseState;
    }

    public static MouseManager GetInstance()
    {
        return GameObject.Find(StringManager.Names.mouseManager).GetComponent<MouseManager>();
    }
}
