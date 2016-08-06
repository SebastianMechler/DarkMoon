using UnityEngine;
using System.Collections;

public enum TerminalType
{
  MAIN_TERMINAL = 0, // do not change this
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
    Debug.Log("Interacting with Terminal: " + this.gameObject.name + " Type: " + m_terminalType.ToString());
#endif

    TerminalInformation information = new TerminalInformation();

    switch (m_terminalType)
    {
      case TerminalType.MAIN_TERMINAL:
        break;
      case TerminalType.TERMINAL_ONE:
        break;

      case TerminalType.TERMINAL_TWO:
        if (SingletonManager.MainTerminalController.GetTerminalState(TerminalType.TERMINAL_TWO) == TerminalState.Unlocked)
        {
          // deactivate light

          // INFO: main-terminal
          information.isActivated = true;
          information.isCollected = true;
          SingletonManager.MainTerminalController.SetTerminalInformation((int)TerminalType.MAIN_TERMINAL, information);

          // INFO: terminal-one
          information.isActivated = true;
          information.isCollected = false;
          SingletonManager.MainTerminalController.SetTerminalInformation((int)TerminalType.TERMINAL_ONE, information);

          // INFO: terminal-two
          information.isActivated = true;
          information.isCollected = true;
          SingletonManager.MainTerminalController.SetTerminalInformation((int)TerminalType.TERMINAL_TWO, information);

          // set update mainterminal screen and unlock it
          UpdateTerminalScreen(TerminalType.MAIN_TERMINAL);
          SingletonManager.MainTerminalController.SetTerminalState(TerminalType.MAIN_TERMINAL, TerminalState.Unlocked);
          SingletonManager.AudioManager.Play(AudioType.TERMINAL_COMPILE_SUCCESS);

          // update self
          UpdateTerminalScreen(TerminalType.TERMINAL_TWO);

          // enable terminal-one screen
          SingletonManager.MainTerminalController.SetTerminalState(TerminalType.TERMINAL_ONE, TerminalState.Unlocked);
        }
        break;
      case TerminalType.TERMINAL_THREE:
        // can only be activated when terminal_generator has been enabled!
        if (SingletonManager.MainTerminalController.GetTerminalState(TerminalType.TERMINAL_GENERATOR) == TerminalState.Unlocked)
        {
          // update self ==> player can exit now
          UpdateTerminalScreen(m_terminalType);
          SingletonManager.MainTerminalController.SetTerminalState(m_terminalType, TerminalState.Unlocked);
          SingletonManager.AudioManager.Play(AudioType.TERMINAL_COMPILE_SUCCESS);
        }
          
        break;
      case TerminalType.TERMINAL_GENERATOR:
        // unlockes access to terminal_three
        // generator can only be accessed if terminal_one is unlocked
        if (SingletonManager.MainTerminalController.GetTerminalState(TerminalType.TERMINAL_ONE) == TerminalState.Unlocked)
        {
          // update terminal3
          UpdateTerminalScreen(TerminalType.TERMINAL_THREE);
          SingletonManager.MainTerminalController.SetTerminalState(TerminalType.TERMINAL_THREE, TerminalState.Unlocked);

          // update self
          UpdateTerminalScreen(m_terminalType);
          SingletonManager.MainTerminalController.SetTerminalState(m_terminalType, TerminalState.Unlocked);

          // INFO: terminal-three
          information.isActivated = true;
          information.isCollected = true;
          SingletonManager.MainTerminalController.SetTerminalInformation((int)TerminalType.TERMINAL_THREE, information);

          // INFO: terminal-generator
          information.isActivated = true;
          information.isCollected = true;
          SingletonManager.MainTerminalController.SetTerminalInformation((int)TerminalType.TERMINAL_GENERATOR, information);

          SingletonManager.AudioManager.Play(AudioType.TERMINAL_COMPILE_SUCCESS);
        }
        
        break;
    }


    

    //SingletonManager.MainTerminalController.UpdateData(m_terminalType);
    
    //
    //Transform go = this.transform.FindChild("display_2states");
    //go.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(0.5f, 0.0f));

    // Save Game
    SingletonManager.XmlSave.Save();
  }

    void UnlockSelf()
    {
        Transform displayState = this.gameObject.transform.FindChild("display_2states");
        displayState.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(0.5f, 0.0f));
    }

  void UpdateTerminalScreen(TerminalType type)
  {
    Terminals terminal = SingletonManager.MainTerminalController.GetTerminalByType(type);
    Transform displayState = terminal.m_terminal.transform.FindChild("display_2states");
    displayState.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(0.5f, 0.0f));
  }


    
}
