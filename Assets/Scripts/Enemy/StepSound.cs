using UnityEngine;
using System.Collections;

public class StepSound : MonoBehaviour {

  public float m_threshold = 0.15f;
  public float m_thresholdRunning = 0.15f;
  private bool m_playOnce = false;
  // public float min = float.MaxValue;

  void Update()
  {
    // min = (transform.position.y < min ? transform.position.y : min);
    float speed = SingletonManager.Enemy.GetComponent<EnemyAiScript>().getMovementSpeed();
    if (speed > 7.0f)
    {
      // running
      if (!m_playOnce && transform.position.y < m_thresholdRunning)
      {
        // Debug.Log(gameObject.name + " >> running >> " + transform.position.y);

        float dist = Vector3.Distance(SingletonManager.Player.transform.position, SingletonManager.Enemy.transform.position);
        dist = 1 - (dist / 40);
        float vol = Mathf.Clamp(dist, 0.02f, 0.60f);
        // Debug.Log("Play on '"+gameObject.name+"' with volume: " + vol);

        m_playOnce = true;
        int rand = Random.Range(0, 2);
        SingletonManager.AudioManager.Play(AudioType.ENEMY_STEP_ONE + rand, vol);
      }
      else if (m_playOnce && transform.position.y >= m_thresholdRunning)
      {
        m_playOnce = false;
      }
    }
    else
    {
      // walking
      if (!m_playOnce && transform.position.y < m_threshold)
      {
        // Debug.Log(gameObject.name + " >> walking >> " + transform.position.y);

        float dist = Vector3.Distance(SingletonManager.Player.transform.position, SingletonManager.Enemy.transform.position);
        dist = 1 - (dist / 40);
        float vol = Mathf.Clamp(dist, 0.02f, 0.60f);
        // Debug.Log("Play on '"+gameObject.name+"' with volume: " + vol);

        m_playOnce = true;
        int rand = Random.Range(0, 2);
        SingletonManager.AudioManager.Play(AudioType.ENEMY_STEP_ONE + rand, vol);
      }
      else if (m_playOnce && transform.position.y >= m_threshold)
      {
        m_playOnce = false;
      }
    }


  }
  
}
