using UnityEngine;
using System.Collections;

// Just some sample Script which plays a specific sound (e. g. heart-beat) based on the distance from player to enemy
// the shorter the distance the more often the sound will be played

public class EnemyFeedback : MonoBehaviour
{
  public AudioSource m_audioSource;
  public Transform m_player;
  public Transform m_enemy;

  float m_beepDuration = 25.25f;
  float m_timer = 0.25f;

  void Update ()
  {
    int distance = CalculateDistance();
    
    if (m_timer > 0.0f)
    {
      m_timer -= Time.deltaTime;

      if (m_timer < 0.0f)
      {
        m_timer = 0.0f;
        // Play Audio
        SingletonManager.AudioManager.Play(AudioType.INTERACT);
        m_timer = m_beepDuration / distance;
      }
    }

	}

  int CalculateDistance()
  {
    return 50 - (int)Mathf.Round(Vector3.Distance(m_player.position, m_enemy.position));
  }
}
