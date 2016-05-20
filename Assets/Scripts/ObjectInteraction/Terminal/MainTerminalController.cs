using UnityEngine;
using System.Collections;


[System.Serializable]
public struct TerminalInformation
{
  public bool isCollected; // will be set to true if interaction with a terminal (e. g. TERMINAL_ONE) has been done
  public bool isActivated; // will be set to true if isCollected is true and interaction with main terminal has been done
}

// script which holds information about collected data
public class MainTerminalController : MonoBehaviour
{
  TerminalInformation[] m_terminals = new TerminalInformation[(int)TerminalType.COUNT]; // create an array with the number of terminals (-1 cause MAIN_TERMINAL is not needed)

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
    }
  }

  bool IsAllDataActivated()
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

  public static MainTerminalController GetInstance()
  {
    return GameObject.Find(StringManager.Names.mainTerminalController).GetComponent<MainTerminalController>();
  }
}
