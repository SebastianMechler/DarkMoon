using UnityEngine;
using System.Collections;

public class EnemySpawningCheck : MonoBehaviour
{

  private float m_WaitAtStart = 2.0f;
  public GameObject m_AdditionalCollider;
  private GameObject[] m_RigidbodyColliders;

  public void ToggleRigidbodyColliders(bool state)
  {
    for (int i = 0; i < m_RigidbodyColliders.Length; i++)
    {
      m_RigidbodyColliders[i].GetComponent<RigidbodyNoiseCreator>().enabled = state;
    }
  }

  void Start()
  {
    m_RigidbodyColliders = GameObject.FindGameObjectsWithTag(StringManager.Tags.noiseRigidbody);
    ToggleRigidbodyColliders(false);
  }

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
