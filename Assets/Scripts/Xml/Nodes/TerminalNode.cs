using UnityEngine;
using System.Collections;

// XML includes
using System.Text;
using System.Xml;

public class TerminalNode : XmlNodeBase
{
  private XmlNode m_main = null;
  private XmlNode m_isActivated = null;
  private XmlNode m_isCollected = null;

  public TerminalNode(XmlNode mainNode)
  {
    Initialize(mainNode);
  }

  public void Initialize(XmlNode mainNode)
  {
    m_main = mainNode;
    m_isActivated = m_main.ChildNodes[0];
    m_isCollected = m_main.ChildNodes[1];
  }

  public void SetActivated(bool value)
  {
    SetBool(m_isActivated, value);
  }

  public bool GetActivated()
  {
    return GetBool(m_isActivated);
  }

  public void SetCollected(bool value)
  {
    SetBool(m_isCollected, value);
  }

  public bool GetCollected()
  {
    return GetBool(m_isCollected);
  }
}


