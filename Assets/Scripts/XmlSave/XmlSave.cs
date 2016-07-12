using UnityEngine;
using System.Collections;

// XML includes
using System.Text;
using System.Xml;

public class XmlNodes
{
  // Player
  public const string Player = "Player";
  public const string Player_rotation = "Rotation";
  public const string Player_position = "Position";
  public const string Player_battery = "Battery";
  public const string Player_oxygen = "Oxygen";

}

public class XmlNodeBase
{
  public Vector3 GetVector3(XmlNode node)
  {
    return new Vector3(float.Parse(node.Attributes["X"].Value), float.Parse(node.Attributes["Y"].Value), float.Parse(node.Attributes["Z"].Value));
  }

  public void SetVector3(XmlNode node, Vector3 vector)
  {
    node.Attributes["X"].InnerText = vector.x.ToString();
    node.Attributes["Y"].InnerText = vector.y.ToString();
    node.Attributes["Z"].InnerText = vector.z.ToString();
  }

  public float GetFloat(XmlNode node)
  {
    return float.Parse(node.InnerText);
  }

  public void SetFloat(XmlNode node, float value)
  {
    node.InnerText = value.ToString();
  }
}



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

public class XmlSave : MonoBehaviour
{
  XmlDocument m_document = new XmlDocument();

  void Update()
  {
    if (Input.GetKeyDown(KeyCode.F3))
    {
      // Loads the xml file, if it does not exist it will create an xml tree with default values
      Load("save.xml");
    }

    if (Input.GetKeyDown(KeyCode.F4))
    {
      Save("save.xml");
    }
  }

  public XmlNode GetXmlNode(string name, XmlElement element = null)
  {
    XmlNodeList list;

    if (element == null)
    {
      list = m_document.DocumentElement.GetElementsByTagName(name);
    }
    else
    {
      list = element.GetElementsByTagName(name);
    }

    if (list.Count == 0)
      return null;
    else
    {
      return list[0];
    }
  }

  public void Save(string file)
  {
    if (m_document.DocumentElement == null)
    {
      XmlElement element = m_document.CreateElement("xml");
      m_document.AppendChild(element);
    }

    CreatePlayerNode();

    PlayerNode player = new PlayerNode(GetXmlNode(XmlNodes.Player));
    player.SetPosition(SingletonManager.Player.GetComponent<Transform>().position);

    Quaternion rotation = SingletonManager.Player.GetComponent<Transform>().rotation;
    player.SetRotation(new Vector3(rotation.x, rotation.y, rotation.z));

    float oxygen = SingletonManager.Player.GetComponent<PlayerOxygen>().m_current;
    player.SetOxygen(oxygen);

    float battery = SingletonManager.Player.GetComponent<PlayerBattery>().m_current;
    player.SetBattery(battery);

    m_document.Save(file);
    Debug.Log("XmlSave saved.");
  }

  public void Load(string file)
  {
    try
    {
      m_document.Load(file);


      if (m_document.DocumentElement == null)
      {
        XmlElement element = m_document.CreateElement("xml");
        m_document.AppendChild(element);
      }

      CreatePlayerNode();

      PlayerNode player = new PlayerNode(GetXmlNode(XmlNodes.Player));
      SingletonManager.Player.GetComponent<Transform>().position = player.GetPosition();
      SingletonManager.Player.GetComponent<Transform>().rotation = new Quaternion(player.GetRotation().x, player.GetRotation().y, player.GetRotation().z, 1.0f);
      SingletonManager.Player.GetComponent<PlayerOxygen>().m_current = player.GetOxygen();
      SingletonManager.Player.GetComponent<PlayerBattery>().m_current = player.GetBattery();
    }
    catch
    {
      // file not found, ...
    }
    Debug.Log("XmlSave loaded.");
  }

  public void CreatePlayerNode()
  {
    // creates all nodes if they dont exist already

    XmlElement ele = (XmlElement)GetXmlNode(XmlNodes.Player);
    if (ele == null)
    {
      // element does not exist, create it
      ele = m_document.CreateElement(XmlNodes.Player);
      m_document.DocumentElement.AppendChild(ele);
    }

    XmlElement position = (XmlElement)GetXmlNode(XmlNodes.Player_position, ele);
    if (position == null)
    {
      // element does not exist, create it
      position = m_document.CreateElement(XmlNodes.Player_position);
      position.SetAttribute("X", "0.0");
      position.SetAttribute("Y", "0.0");
      position.SetAttribute("Z", "0.0");
      ele.AppendChild(position);
    }

    XmlElement rotation = (XmlElement)GetXmlNode(XmlNodes.Player_rotation, ele);
    if (rotation == null)
    {
      // element does not exist, create it
      rotation = m_document.CreateElement(XmlNodes.Player_rotation);
      rotation.SetAttribute("X", "0.0");
      rotation.SetAttribute("Y", "0.0");
      rotation.SetAttribute("Z", "0.0");
      ele.AppendChild(rotation);
    }

    XmlElement oxygen = (XmlElement)GetXmlNode(XmlNodes.Player_oxygen, ele);
    if (oxygen == null)
    {
      // element does not exist, create it
      oxygen = m_document.CreateElement(XmlNodes.Player_oxygen);
      oxygen.InnerText = "0.0";
      ele.AppendChild(oxygen);
    }

    XmlElement battery = (XmlElement)GetXmlNode(XmlNodes.Player_battery, ele);
    if (battery == null)
    {
      // element does not exist, create it
      battery = m_document.CreateElement(XmlNodes.Player_battery);
      battery.InnerText = "0.0";
      ele.AppendChild(battery);
    }
  }
  /*
  foreach (XmlNode node in xmlDoc.DocumentElement.ChildNodes)
  {
    Debug.Log("Reading current node: " + node.Name);

    if (node.Name.Equals("Player"))
    {
      foreach (XmlNode lowerNode in node.ChildNodes)
      {
        if (lowerNode.Name.Equals("Position"))
        { 

          Debug.Log("Reading childNode: " + lowerNode.Name);

          Vector3 position = new Vector3(float.Parse(lowerNode.Attributes["X"].Value), float.Parse(lowerNode.Attributes["Y"].Value), float.Parse(lowerNode.Attributes["Z"].Value));
          Debug.Log("Player position: " + position);

          // update saveGameData
          m_saveGameData.playerPosition = position;
        }

      }
    }

    Debug.Log("Note completely read.");
    Debug.Log("----------------------------------");
    Debug.Log("----------------------------------");
  }



  //foreach (XmlNode xmlNode in xmlDoc.DocumentElement.ChildNodes[2].ChildNodes[0].ChildNodes)
//      Debug.Log(xmlNode.Attributes["currency"].Value + ": " + xmlNode.Attributes["rate"].Value);

}
*/

}
