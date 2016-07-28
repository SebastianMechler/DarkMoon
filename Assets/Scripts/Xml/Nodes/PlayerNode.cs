using UnityEngine;
using System.Collections;

// XML includes
using System.Text;
using System.Xml;

public class PlayerNode : XmlNodeBase
{
  private XmlNode m_main = null;
  private XmlNode m_position = null; // child
  private XmlNode m_rotation = null; // child
  private XmlNode m_oxygen = null; // child
  private XmlNode m_battery = null; // child

  public PlayerNode(XmlNode mainNode)
  {
    Initialize(mainNode);
  }

  public void Initialize(XmlNode mainNode)
  {
    m_main = mainNode;
    m_position = m_main.ChildNodes[0]; // ADD LOOP here to be safe
    m_rotation = m_main.ChildNodes[1]; // ADD LOOP here to be safe
    m_oxygen = m_main.ChildNodes[2]; // ADD LOOP here to be safe
    m_battery = m_main.ChildNodes[3]; // ADD LOOP here to be safe
  }

  public Vector3 GetPosition()
  {
    return GetVector3(m_position);
  }

  public Vector3 GetRotation()
  {
    return GetVector3(m_rotation);
  }

  public void SetPosition(Vector3 position)
  {
    SetVector3(m_position, position);
  }

  public void SetRotation(Vector3 rotation)
  {
    SetVector3(m_rotation, rotation);
  }

  public float GetOxygen()
  {
    return GetFloat(m_oxygen);
  }

  public void SetOxygen(float value)
  {
    SetFloat(m_oxygen, value);
  }

  public float GetBattery()
  {
    return GetFloat(m_battery);
  }

  public void SetBattery(float value)
  {
    SetFloat(m_battery, value);
  }
}


