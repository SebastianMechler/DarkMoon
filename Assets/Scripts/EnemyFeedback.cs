using UnityEngine;
using System.Collections;

// Just some sample Script which plays a specific sound (e. g. heart-beat) based on the distance from player to enemy
// the shorter the distance the more often the sound will be played

public class EnemyFeedback : MonoBehaviour
{
  public AudioSource m_audioSource;
  private Transform m_player;
  private Transform m_enemy;

  float m_beepDuration = 25.25f;
  float m_timer = 0.25f;
  float m_rate = 1.0f;
  float m_timerFixedUpate = 2.0f;

  public static EnemyFeedback GetInstance()
  {
    return SingletonManager.Player.GetComponent<EnemyFeedback>();
  }

  void Start()
  {
    m_player = SingletonManager.Player.gameObject.transform;
    m_enemy = SingletonManager.Enemy.gameObject.transform;
  }

  void Update ()
  {
    float distance = CalculateDistance();
    
    if (m_timer > 0.0f)
    {
      m_timer -= Time.deltaTime;

      if (m_timer < 0.0f)
      {
        m_timer = 0.0f;
        // Play Audio
        SingletonManager.AudioManager.Play(AudioType.PLAYER_HEARTBEAT, 0.25f);
        m_timer = (m_beepDuration / distance) * m_rate;
      }
    }

	}

  void FixedUpdate()
  {
    m_timerFixedUpate -= Time.fixedDeltaTime;
    if(m_timerFixedUpate > 0.0f)
    {
      return;
    }
    m_timerFixedUpate = 2.0f;

    // every 2 seconds
    m_rate -= 0.1f;
    if (m_rate <= 1.0f)
    {
      m_rate = 1.0f;
    }
  }

  public void increaseRate(float rate)
  {
    m_rate += rate;
  }

  bool playerInView()
  {
    GameObject enemy = SingletonManager.Enemy;
    Vector3 playerPos = GameObject.FindGameObjectWithTag(StringManager.Tags.player).transform.position;
    // Pivot is slightly on top
    playerPos.y += 0.5f;
    float range = Vector3.Distance(playerPos, enemy.transform.position) * 1.2f;
    // Debug.DrawRay(m_HeadPosition.position, (playerPos - m_HeadPosition.position), Color.cyan, 1.0f);

    RaycastHit[] hits = Physics.RaycastAll(transform.position, (playerPos - enemy.transform.position).normalized, range);

    bool playerInPlainView = true;
    for (int i = 0; i < hits.Length; i++)
    {
      // RaycastHit hit = hits[i];
      string thisTag = hits[i].collider.gameObject.tag;

      if (!thisTag.Equals(StringManager.Tags.Waypoints) && !thisTag.Equals(StringManager.Tags.interactableObject) &&
          !thisTag.Equals(StringManager.Tags.enemy) && !thisTag.Equals(StringManager.Tags.player) &&
          !thisTag.Equals(StringManager.Tags.noise) && !thisTag.Equals(StringManager.Tags.floor))
      {
        // Debug.Log("Something named '" + hits[i].collider.gameObject.name + "' tagged '" + thisTag + "' is Blocking the View");
        return false;
      }

    }

    if (playerInPlainView)
    {
      return true;
    }

    return false;
  }

  float CalculateDistance()
  {
    // Raycast, Is Player in Plain View
    if (playerInView())
    {
      return 50.0f - Mathf.Round(Vector3.Distance(m_player.position, m_enemy.position));
    }

    return 50.0f;
  }
}
