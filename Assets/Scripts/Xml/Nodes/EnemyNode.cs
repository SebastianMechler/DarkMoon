using UnityEngine;
using System.Collections;

// XML includes
using System.Text;
using System.Xml;

public class EnemyNode : XmlNodeBase
{
  private XmlNode m_main = null;
  private XmlNode m_position = null; // child
  private XmlNode m_rotation = null; // child

  public EnemyNode(XmlNode mainNode)
  {
    Initialize(mainNode);
  }

  public void Initialize(XmlNode mainNode)
  {
    m_main = mainNode;
    m_position = m_main.ChildNodes[0]; // ADD LOOP here to be safe
    m_rotation = m_main.ChildNodes[1]; // ADD LOOP here to be safe
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
}


