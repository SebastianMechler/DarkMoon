using UnityEngine;
using System.Collections;

public class TerminalTriggerLab1 : MonoBehaviour
{
	void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.tag != StringManager.Tags.player)
      return;

    if (SingletonManager.MainTerminalController.GetTerminalState(TerminalType.TERMINAL_TWO) == TerminalState.Locked)
    {
      TerminalInformation information = new TerminalInformation();
      Terminals terminal;
      Transform displayState;

      TerminalInformation tInformation = SingletonManager.MainTerminalController.GetTerminalInformation((int)TerminalType.TERMINAL_TWO);
      if (tInformation.isActivated == true)
      {
        return;
      }

      // INFO: terminal-two
      information = new TerminalInformation();
      information.isActivated = true;
      information.isCollected = false;
      SingletonManager.MainTerminalController.SetTerminalInformation((int)TerminalType.TERMINAL_TWO, information);


      terminal = SingletonManager.MainTerminalController.GetTerminalByType(TerminalType.TERMINAL_TWO);
      displayState = terminal.m_terminal.transform.FindChild("display_2states");
      displayState.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(0.0f, 0.0f));
      SingletonManager.MainTerminalController.SetTerminalState(TerminalType.TERMINAL_TWO, TerminalState.Unlocked);

      SingletonManager.XmlSave.Save();
    }
        

      if (SingletonManager.MainTerminalController.GetTerminalState(TerminalType.MAIN_TERMINAL) == TerminalState.Unlocked)
      {
        TerminalInformation information = new TerminalInformation();
        Terminals terminal;
        Transform displayState;

        // INFO: terminal-one
        information.isActivated = true;
        information.isCollected = true;
        SingletonManager.MainTerminalController.SetTerminalInformation((int)TerminalType.TERMINAL_ONE, information);

        terminal = SingletonManager.MainTerminalController.GetTerminalByType(TerminalType.TERMINAL_ONE);
        displayState = terminal.m_terminal.transform.FindChild("display_2states");
        displayState.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(0.5f, 0.0f));
        SingletonManager.MainTerminalController.SetTerminalState(TerminalType.TERMINAL_ONE, TerminalState.Unlocked);

        SingletonManager.XmlSave.Save();
      }
   }
}
