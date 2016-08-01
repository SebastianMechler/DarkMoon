using UnityEngine;
using System.Collections;

// XML includes
using System.Text;
using System.Xml;

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

  public void SetInt(XmlNode node, int value)
  {
    node.InnerText = value.ToString();
  }

  public int GetInt(XmlNode node)
  {
    return int.Parse(node.InnerText);
  }

  public void SetBool(XmlNode node, bool value)
  {
    node.InnerText = value.ToString();
  }

  public bool GetBool(XmlNode node)
  {
    return bool.Parse(node.InnerText);
  }

  public void SetString(XmlNode node, string value)
  {
    node.InnerText = value;
  }

  public string GetString(XmlNode node)
  {
    return node.InnerText;
  }
}
