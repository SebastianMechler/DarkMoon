using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody), typeof(BoxCollider))]
public class RigidbodyNoiseCreator : MonoBehaviour {

  private Vector3 m_basePosition;
  private float m_timer = 2.56f;

  // Important Data
  private GameObject m_Enemy;
  public string m_UniqueName;
  // Calc Distance to nearest Waypoint
  float lastNearestDistance = float.MaxValue;
  private GameObject m_NearestWaypoint;


  // Use this for initialization
  void Start () {
    m_basePosition = this.transform.position;
    m_Enemy = SingletonManager.Enemy;

    if (GetComponent<MeshCollider>() != null)
    {
      Destroy(GetComponent<MeshCollider>());
    }

    if (GetComponent<BoxCollider>() == null)
    {
      Debug.LogError("Missing Box Collider for 'RigidbodyNoiseCreator' on '"+gameObject.name+" ("+m_UniqueName+")'!");
    }

    this.gameObject.tag = StringManager.Tags.noiseRigidbody;
  }
	
	// Update is called once per frame
	void Update () {

    m_timer -= Time.deltaTime;
    if(m_timer >= 0.0f)
    {
      return;
    }
    m_timer = 2.56f;

    Vector3 curPos = this.transform.position;
    if(curPos != m_basePosition)
    {
      m_basePosition = curPos;
      MakeNoiseAtCurrentPosition();
    }

	}

  void OnCollisionEnter(Collision col)
  {
    if(col.gameObject.tag != StringManager.Tags.player)
    {
      return;
    }
    // todo maybe better than Update check
    SingletonManager.AudioManager.Play(AudioType.RIGIDBODY_METAL_NOISE, 1.0f);
  }

  public void UpdateNearestWaypoint()
  {
    m_UniqueName = gameObject.GetHashCode().ToString();

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
  }

  public void MakeNoiseAtCurrentPosition()
  {
    if (!m_Enemy.activeSelf)
    {
      return;
    }

    Debug.Log("Noise created due to Rigoidbody Movement of an Object.");
    UpdateNearestWaypoint();
    SingletonManager.AudioManager.Play(AudioType.ENEMY_SHOUT);
    m_Enemy.GetComponent<EnemyAiScript>().changeMovementPattern(EnemyAiScript.MovementPattern.STATIC, gameObject, m_NearestWaypoint);
    SingletonManager.EnemyFeedback.increaseRate(2.0f);
  }

}
