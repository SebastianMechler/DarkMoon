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
#if DEBUG
    Debug.Log("Interacting with Terminal: " + this.gameObject.name);
#endif

    SingletonManager.MainTerminalController.UpdateData(m_terminalType);
  }
}
