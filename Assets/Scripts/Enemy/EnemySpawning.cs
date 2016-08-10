using UnityEngine;
using System.Collections;

public class EnemySpawning : MonoBehaviour
{
  public GameObject m_Start;
  public GameObject m_FirstWaypoint;
  public GameObject m_AdditionalCollider;
  
  void OnTriggerEnter(Collider other)
  {
    // Ignore When on Savegame
    if (SingletonManager.GameManager.m_isSaveGame) { return; }

    // Move Enemy to Start Position
    SingletonManager.Enemy.transform.position = m_Start.transform.position;

    // Set LastWayoint and NextWaypoint
    // m_Enemy.GetComponent<EnemyAiScript>().m_StartDynamicWaypoint = m_Start;
    // m_Enemy.GetComponent<EnemyAiScript>().m_FirstDynamicWaypoint = m_FirstWaypoint;
    SingletonManager.Enemy.GetComponent<EnemyAiScript>().SetDynamicWaypoints(m_FirstWaypoint, m_FirstWaypoint);

    // Enable Enemy Once again
    SingletonManager.Enemy.SetActive(true);
    m_AdditionalCollider.SetActive(true);

    // Deactivate Parent > Should deactivate all child
    transform.parent.gameObject.SetActive(false);

    // Enable Rigidbody Colliders
    transform.parent.gameObject.GetComponent<EnemySpawningCheck>().ToggleRigidbodyColliders(true);
  }
	
}
