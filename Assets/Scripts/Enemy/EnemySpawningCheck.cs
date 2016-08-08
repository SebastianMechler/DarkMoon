using UnityEngine;
using System.Collections;

public class EnemySpawningCheck : MonoBehaviour
{

  private float m_WaitAtStart = 2.0f;
  public GameObject m_AdditionalCollider;

  // Update is called once per frame
  void Update ()
	{
	  m_WaitAtStart -= Time.deltaTime;
	  if (m_WaitAtStart >= 0.0f)
	  {
      return;
	  }

	  SingletonManager.Enemy.SetActive(false);
    m_AdditionalCollider.SetActive(false);

	}
}
