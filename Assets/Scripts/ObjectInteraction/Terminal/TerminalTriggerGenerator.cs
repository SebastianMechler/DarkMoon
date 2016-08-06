using UnityEngine;
using System.Collections;

public class TerminalTriggerGenerator : MonoBehaviour
{
  public GameObject m_Door;

  void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.tag == StringManager.Tags.player)
    {
      if (SingletonManager.MainTerminalController.GetTerminalState(TerminalType.MAIN_TERMINAL) == TerminalState.Unlocked)
      {
        TerminalInformation tInformation = SingletonManager.MainTerminalController.GetTerminalInformation((int)TerminalType.TERMINAL_GENERATOR);
        if (tInformation.isActivated == true)
        {
          return;
        }

        // todo open door to generator room
        m_Door.GetComponent<DoorBehaviour>().ChangeDoorState(DoorBehaviour.DoorState.OPENING_STUCKED);

        // INFO: terminal-three
        TerminalInformation information = new TerminalInformation();
        information.isActivated = true;
        information.isCollected = false;
        SingletonManager.MainTerminalController.SetTerminalInformation((int)TerminalType.TERMINAL_THREE, information);

        // INFO: terminal-generator
        information.isActivated = true;
        information.isCollected = false;
        SingletonManager.MainTerminalController.SetTerminalInformation((int)TerminalType.TERMINAL_GENERATOR, information);

        // update display for terminal three
        Terminals terminal = SingletonManager.MainTerminalController.GetTerminalByType(TerminalType.TERMINAL_GENERATOR);
        Transform displayState = terminal.m_terminal.transform.FindChild("display_2states");
        displayState.GetComponent<Renderer>().material.SetTextureOffset("_MainTex", new Vector2(0.0f, 0.0f));

        SingletonManager.XmlSave.Save();
      }
    }
  }
}
