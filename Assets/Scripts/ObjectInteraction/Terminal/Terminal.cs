using UnityEngine;
using System.Collections;

public enum TerminalType
{
  MAIN_TERMINAL = -1, // do not change this
  TERMINAL_ONE,
  TERMINAL_TWO,
  TERMINAL_THREE,
  TERMINAL_GENERATOR,
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

    Transform displayState = null;

    switch (m_terminalType)
    {
      case TerminalType.MAIN_TERMINAL:
        //SingletonManager.UIManager.SetUIVisibility(UIType.MainTerminal, true);
        break;
      case TerminalType.TERMINAL_ONE:
        //SingletonManager.UIManager.SetUIVisibility(UIType.TerminalOne, true);
        SingletonManager.AudioManager.Play(AudioType.TERMINAL_COMPILE_SUCCESS);   
        break;

      case TerminalType.TERMINAL_TWO:
        //SingletonManager.UIManager.SetUIVisibility(UIType.TerminalTwo, true);

        // update mainterminal information
        Terminals terminal = SingletonManager.MainTerminalController.GetTerminalByType(TerminalType.MAIN_TERMINAL);
        displayState = terminal.m_terminal.transform.FindChild("display_2states");
        displayState.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(0.5f, 0.0f));
        terminal.m_state = TerminalState.Unlocked;

        // update terminal-one information
        displayState = this.gameObject.transform.FindChild("display_2states");
        displayState.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(0.5f, 0.0f));
        SingletonManager.AudioManager.Play(AudioType.TERMINAL_COMPILE_SUCCESS);
        break;
      case TerminalType.TERMINAL_THREE:
        //SingletonManager.UIManager.SetUIVisibility(UIType.TerminalThree, true);
        SingletonManager.AudioManager.Play(AudioType.TERMINAL_COMPILE_SUCCESS);
        break;
      case TerminalType.TERMINAL_GENERATOR:
        //SingletonManager.UIManager.SetUIVisibility(UIType.TerminalThree, true);
        SingletonManager.AudioManager.Play(AudioType.TERMINAL_COMPILE_SUCCESS);
        break;
    }

    SingletonManager.MainTerminalController.UpdateData(m_terminalType);
    
    //
    Transform go = this.transform.FindChild("display_2states");
    go.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(0.5f, 0.0f));

    // Save Game
    SingletonManager.XmlSave.Save();
  }
}
