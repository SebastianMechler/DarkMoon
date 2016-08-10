using UnityEngine;
using System.Collections;

public class CreateNoise : MonoBehaviour {

	private GameObject m_Enemy;
	public string m_UniqueName;
	public bool m_PlaySoundOnEnter;
	public GameObject m_NearestWaypoint;
  public GameObject m_hidingZone = null;

	float lastNearestDistance = float.MaxValue;

	// Use this for initialization
	void Awake ()
	{
		m_UniqueName = Mathf.Abs(gameObject.GetHashCode()).ToString();
        
		GameObject[] list = GameObject.FindGameObjectsWithTag(StringManager.Tags.Waypoints);
		Vector3 thisGameObject = gameObject.transform.position;
		Vector3 next;

		int size = list.Length;
		for (int i = 0; i < size; i++)
		{
			next = list[i].transform.position;
			float distance = Vector3.Distance(thisGameObject, next);
			if (distance < lastNearestDistance)
			{
				lastNearestDistance = distance;
				m_NearestWaypoint = list[i];
			}
		}
    // Debug.Log("[!] Shortest Distance starting from " + m_UniqueName + ": " + m_NearestWaypoint.name);
	  m_Enemy = SingletonManager.Enemy;
	}

  void Start()
    {
        // m_Enemy = GameObject.FindGameObjectWithTag(StringManager.Tags.enemy);
    }

	void OnTriggerEnter(Collider other)
	{
		if(other.tag == StringManager.Tags.player && SingletonManager.Enemy.activeSelf)
		{
  		if (m_PlaySoundOnEnter)
			{
        // gameObject.GetComponent<AudioSource>().Play();
        SingletonManager.AudioManager.Play(AudioType.ENEMY_SHOUT);
			}
		  // Debug.Log("[:] CreateNoise OnTriggerEnter Called");
  		m_Enemy.GetComponent<EnemyAiScript>().changeMovementPattern(EnemyAiScript.MovementPattern.STATIC, gameObject, m_NearestWaypoint);

      // effect occours only once
      gameObject.GetComponent<Renderer>().enabled = false;
      gameObject.GetComponent<BoxCollider>().enabled = false;
      //gameObject.SetActive(false);

      // SingletonManager.EnemyFeedback.increaseRate(0.5f);

      // disable hiding zone
      if (m_hidingZone != null)
      {
        m_hidingZone.GetComponent<HidingZone>().OnNoiseTrigger();
      }
    }
	}

	public string getName()
	{
		return m_UniqueName;
	}

	public GameObject getNearestWaypoint()
	{
		return m_NearestWaypoint;
	}
}
