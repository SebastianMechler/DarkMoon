using UnityEngine;
using System.Collections;

public class StepSound : MonoBehaviour {

  float m_delay = 2.0f;

  void FixedUpdate()
  {
    m_delay -= Time.fixedDeltaTime;
  }

  void OnCollisionStay(Collision other)
  {
    // if(other.gameObject.layer.ToString() == StringManager.Tags.floor && m_delay < 0.0f)
    // {
      m_delay = 2.0f;
      int rand = Random.Range(0, 2);
      SingletonManager.AudioManager.Play(AudioType.ENEMY_STEP_ONE + rand);
    // }
  }
}
