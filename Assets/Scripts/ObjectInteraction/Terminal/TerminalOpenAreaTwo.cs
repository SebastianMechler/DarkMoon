using UnityEngine;
using System.Collections;

public class TerminalOpenAreaTwo : MonoBehaviour {

  void OnTriggerEnter(Collider other)
  {
    if (other.gameObject.tag == StringManager.Tags.player)
    {
      if (SingletonManager.MainTerminalController.GetTerminalState(TerminalType.TERMINAL_ONE) == TerminalState.Unlocked)
      {
        TerminalInformation tInformation = SingletonManager.MainTerminalController.GetTerminalInformation((int)TerminalType.TERMINAL_THREE);
        if (tInformation.isActivated == true)
        {
          return;
        }

        // Open the door
        Debug.Log("Open door.");

        // INFO: terminal-one
        TerminalInformation information = new TerminalInformation();
        information.isActivated = true;
        information.isCollected = true;
        SingletonManager.MainTerminalController.SetTerminalInformation((int)TerminalType.TERMINAL_ONE, information);

        information.isActivated = true;
        information.isCollected = false;
        SingletonManager.MainTerminalController.SetTerminalInformation((int)TerminalType.TERMINAL_THREE, information);

        SingletonManager.XmlSave.Save();
      }
    }
  }
}
