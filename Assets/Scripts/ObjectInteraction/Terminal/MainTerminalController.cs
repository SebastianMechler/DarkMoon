using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[System.Serializable]
public struct TerminalInformation
{
  public bool isCollected; // will be set to true if interaction with a terminal (e. g. TERMINAL_ONE) has been done
  public bool isActivated; // will be set to true if isCollected is true and interaction with main terminal has been done
}

// script which holds information about collected data
public class MainTerminalController : MonoBehaviour
{
  public TerminalInformation[] m_terminals = new TerminalInformation[(int)TerminalType.COUNT]; // create an array with the number of terminals (-1 cause MAIN_TERMINAL is not needed)

  public void UpdateData(TerminalType type)
  {
    if (type == TerminalType.MAIN_TERMINAL)
    {
      // activate data if it has been collected
      for (int i = 0; i < m_terminals.Length; i++)
      {
        if (m_terminals[i].isCollected == true)
        {
          // activate data
            m_terminals[i].isActivated = true;
            Debug.Log("Player has activated: " + (TerminalType)i);

          // update ingame-ui
          switch ((TerminalType)i)
          {
            case TerminalType.TERMINAL_ONE:
              SingletonManager.UIManager.GetTerminalToggle(UIType.TerminalOne).isOn = true;
              break;
            case TerminalType.TERMINAL_TWO:
              SingletonManager.UIManager.GetTerminalToggle(UIType.TerminalTwo).isOn = true;
              break;
            case TerminalType.TERMINAL_THREE:
              SingletonManager.UIManager.GetTerminalToggle(UIType.TerminalThree).isOn = true;
              break;
          }
        }
      }
    }
    else
    {
      // note that the data has been collected now by the player
      // he still needs to run to the main terminal and interact with it to activate it
      // data has been collected now
        m_terminals[(int)type].isCollected = true;
        Debug.Log("Player has collected Data: " + type.ToString());    
    }


    if (IsAllDataActivated())
    {
      Debug.Log("Player has collected and activated all Data, he can escape now.");
      SingletonManager.UIManager.SetUIVisibility(UIType.MainTerminal, true);
      SingletonManager.AudioManager.Play(AudioType.TERMINAL_COMPILE_SUCCESS);
    }
  }

  public bool IsAllDataActivated()
  {
    int activatedCount = 0;

    for (int i = 0; i < m_terminals.Length; i++)
    {
      if (m_terminals[i].isActivated == true)
      {
        activatedCount++;
      }
    }

    if (activatedCount == m_terminals.Length)
    {
      return true;
    }
    else
    {
      return false;
    }
  }

  public void SetTerminalInformation(int index, TerminalInformation information)
  {
    // TODO: ADD VISUALIZE EFFECT (interact with the objects...)
    m_terminals[index] = information;
  }

  public TerminalInformation GetTerminalInformation(int index)
  {
    return m_terminals[index];
  }

  public static MainTerminalController GetInstance()
  {
    return GameObject.Find(StringManager.Names.mainTerminalController).GetComponent<MainTerminalController>();
  }
}
