using UnityEngine;
using System.Collections;
using UnityEngine.UI; // ui

// script which will try to load the save.xml

public class XmlLoadOnce : MonoBehaviour
{
  public GameObject m_buttonResume = null;

	void Start ()
  {
    if (SingletonManager.XmlSave.LoadXml())
    {
      // save file could be loaded
      // enable button resume
      if (m_buttonResume != null)
      {
        m_buttonResume.GetComponent<Button>().interactable = true;
        Debug.Log("Enabled resume button");
      }
    }
    else
    {
      Debug.Log("Disabled resume button");
    }
	}
}
