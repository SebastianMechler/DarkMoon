using UnityEngine;
using System.Collections;

public enum TerminalType
{
  MAIN_TERMINAL = -1, // do not change this
  TERMINAL_ONE,
  TERMINAL_TWO,
  TERMINAL_THREE,
  // IMPORTANT!!!!
  // ADD NEW TERMINALS HERE
  // IMPORTANT!!!!
    
  COUNT // number of elements
}

// script to interact with all terminals
public class Terminal : ObjectInteractionBase
{
  public TerminalType m_terminalType = TerminalType.MAIN_TERMINAL;

  void Start()
  {
    InitializeBase(ObjectInteractionType.TERMINAL);
  }

  void Update()
  {
    UpdateBase();
  }

  public override void Interact()
  {
    base.Interact();
#if DEBUG
    Debug.Log("Interacting with Terminal: " + this.gameObject.name);
#endif

    switch (m_terminalType)
    {
      case TerminalType.MAIN_TERMINAL:
        //SingletonManager.UIManager.SetUIVisibility(UIType.MainTerminal, true);
        break;
      case TerminalType.TERMINAL_ONE:
        SingletonManager.UIManager.SetUIVisibility(UIType.TerminalOne, true);
        SingletonManager.AudioManager.Play(AudioType.TERMINAL_COMPILE_SUCCESS);
        break;
      case TerminalType.TERMINAL_TWO:
        SingletonManager.UIManager.SetUIVisibility(UIType.TerminalTwo, true);
        SingletonManager.AudioManager.Play(AudioType.TERMINAL_COMPILE_SUCCESS);
        break;
      case TerminalType.TERMINAL_THREE:
        SingletonManager.UIManager.SetUIVisibility(UIType.TerminalThree, true);
        SingletonManager.AudioManager.Play(AudioType.TERMINAL_COMPILE_SUCCESS);
        break;
    }

    SingletonManager.MainTerminalController.UpdateData(m_terminalType);
  }
}
