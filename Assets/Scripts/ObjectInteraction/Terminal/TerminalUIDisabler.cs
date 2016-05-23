using UnityEngine;
using System.Collections;

public class TerminalUIDisabler : MonoBehaviour
{

  void OnTriggerExit(Collider other)
  {
    if (other.gameObject.tag == StringManager.Tags.player)
    {
      TerminalType type = this.transform.parent.GetComponent<Terminal>().m_terminalType;
      switch (type)
      {
        case TerminalType.MAIN_TERMINAL:
          SingletonManager.UIManager.SetUIVisibility(UIType.MainTerminal, false);
          break;
      }
    }
  }
}
