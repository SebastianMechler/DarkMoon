using UnityEngine;
using System.Collections;

public enum UIType
{
  MainTerminal,
}

public class UIManager : MonoBehaviour
{

  public void SetUIVisibility(UIType type, bool isVisisble)
  {
    Canvas canvas = null;

    switch (type)
    {
      case UIType.MainTerminal:
        canvas = GetMainTerminal().GetComponent<Canvas>();
        break;
    }

    if (canvas != null)
    {
      canvas.enabled = isVisisble;
    }
    else
    {
      Debug.Log("Canvas is null...");
    }
  }

  public GameObject GetMainTerminal()
  {
    return this.transform.FindChild(StringManager.UI.MainTerminal).gameObject;
  }

  public static UIManager GetInstance()
  {
    return GameObject.Find(StringManager.Names.uiManager).GetComponent<UIManager>();
  }
}
