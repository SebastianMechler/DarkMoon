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
  private XmlNode m_lastWaypointName = null;
  private XmlNode m_nextWaypointName = null;
  private XmlNode m_movementPattern = null;
  private XmlNode m_isHunting = null;
  private XmlNode m_huntingWaypointSourceName = null;
  private XmlNode m_huntingWaypointName;

  public EnemyNode(XmlNode mainNode)
  {
    Initialize(mainNode);
  }

  public void Initialize(XmlNode mainNode)
  {
    m_main = mainNode;
    m_position = m_main.ChildNodes[0]; // ADD LOOP here to be safe
    m_rotation = m_main.ChildNodes[1]; // ADD LOOP here to be safe
    m_lastWaypointName = m_main.ChildNodes[2]; // ADD LOOP here to be safe
    m_nextWaypointName = m_main.ChildNodes[3]; // ADD LOOP here to be safe
    m_movementPattern = m_main.ChildNodes[4]; // ADD LOOP here to be safe
    m_isHunting = m_main.ChildNodes[5]; // ADD LOOP here to be safe
    m_huntingWaypointSourceName = m_main.ChildNodes[6]; // ADD LOOP here to be safe
    m_huntingWaypointName = m_main.ChildNodes[7]; // ADD LOOP here to be safe
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

  public void SetEnemyData(EnemyAiScript.XmlConstruct enemyData)
  {
    SetVector3(m_position, enemyData.CurrentPosition);
    SetVector3(m_rotation, enemyData.CurrentRotation);
    SetString(m_lastWaypointName, enemyData.LastWaypointName);
    SetString(m_nextWaypointName, enemyData.NextWaypointName);
    SetInt(m_movementPattern, (int)enemyData.Pattern);
    SetBool(m_isHunting, enemyData.IsHunting);
    SetString(m_huntingWaypointSourceName, enemyData.HuntingWaypointSourceName);
    SetString(m_huntingWaypointName, enemyData.HuntingWaypointName);
  }

  public EnemyAiScript.XmlConstruct GetEnemyData()
  {
    EnemyAiScript.XmlConstruct enemyData = new EnemyAiScript.XmlConstruct();
    enemyData.CurrentPosition = GetVector3(m_position);
    enemyData.CurrentRotation = GetVector3(m_rotation);
    enemyData.LastWaypointName = GetString(m_lastWaypointName);
    enemyData.NextWaypointName = GetString(m_nextWaypointName);
    enemyData.Pattern = (EnemyAiScript.MovementPattern)GetInt(m_movementPattern);
    enemyData.IsHunting = GetBool(m_isHunting);
    enemyData.HuntingWaypointSourceName = GetString(m_huntingWaypointSourceName);
    enemyData.HuntingWaypointName = GetString(m_huntingWaypointName);

    return enemyData;
  }
}


