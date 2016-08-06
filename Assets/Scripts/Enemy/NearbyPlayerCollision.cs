using UnityEngine;
using UnityEngine.SceneManagement;

public class NearbyPlayerCollision : MonoBehaviour
{
  private GameObject m_Enemy;
  private GameObject m_Player;

  void Start()
  {
    m_Enemy = GameObject.FindGameObjectWithTag(StringManager.Tags.enemy);
    m_Player = GameObject.FindGameObjectWithTag(StringManager.Tags.player);

    if (m_Enemy == null || m_Player == null)
    {
      GetComponent<NearbyPlayerCollision>().enabled = false;
    }
  }

  void FixedUpdate()
  {
    transform.position = m_Enemy.transform.position;
  }

  void DeathAcionTurnPlayer()
  {
    float turnRate = 800 * Time.deltaTime;
    Quaternion lookRotation = Quaternion.LookRotation(m_Player.transform.position);
    transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, turnRate);
  }

  void OnTriggerStay(Collider other)
  {
    if (StringManager.Tags.player.Equals(other.gameObject.tag))
    {
      SingletonManager.AudioManager.Play(AudioType.PLAYER_DEATH);
      Time.timeScale = 1.0f;
      SingletonManager.MouseManager.SetMouseState(MouseState.UNLOCKED);
      SceneManager.LoadScene(StringManager.Scenes.deathScreen);
    }
  }

}
