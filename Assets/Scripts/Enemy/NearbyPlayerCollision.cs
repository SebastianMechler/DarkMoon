using UnityEngine;
using UnityEngine.SceneManagement;

public class NearbyPlayerCollision : MonoBehaviour
{
  private GameObject m_Enemy;
  private GameObject m_Player;

  public Animator m_Animator;
  private float m_Timer = 4.0f;
  private bool m_PlayKilling = false;

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

    if (m_PlayKilling)
    {
      m_Timer -= Time.fixedDeltaTime;

      if (m_Timer <= 0.0f)
      {
        SingletonManager.AudioManager.Play(AudioType.PLAYER_DEATH);
        Time.timeScale = 1.0f;
        SingletonManager.MouseManager.SetMouseState(MouseState.UNLOCKED);
        SceneManager.LoadScene(StringManager.Scenes.deathScreen);

        gameObject.SetActive(false);
      }
    }
  }

  void DeathAcionTurnPlayer()
  {
    float turnRate = 800 * Time.deltaTime;
    Quaternion lookRotation = Quaternion.LookRotation(m_Player.transform.position);
    transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, turnRate);
  }

  void OnTriggerStay(Collider other)
  {
    if (!m_PlayKilling && StringManager.Tags.player.Equals(other.gameObject.tag))
    {
      // m_Animator.SetBool("triggerKillPlayer", true);
      m_Animator.SetTrigger("triggerKill");
      SingletonManager.Enemy.GetComponent<EnemyAiScript>().SetKillingActive();
      SingletonManager.Player.GetComponent<PlayerMovement>().enabled = false;
      m_Timer = 4.0f;
      m_PlayKilling = true;

      //Vector3 toTarget = SingletonManager.Player.transform.position - SingletonManager.Enemy.transform.position;
      //toTarget.y = 0.0f;
      //float turnRate = 10000.0f;
      //Quaternion lookRotation = Quaternion.LookRotation(toTarget);
      //transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, turnRate);
    }
  }

}
