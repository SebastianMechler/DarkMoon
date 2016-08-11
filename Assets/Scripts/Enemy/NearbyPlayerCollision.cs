using UnityEngine;
using UnityEngine.SceneManagement;

public class NearbyPlayerCollision : MonoBehaviour
{
  private GameObject m_Enemy;
  private GameObject m_Player;

  public Animator m_Animator;
  private float m_Timer = 4.0f;
  private bool m_PlayKilling = false;

  private bool m_hasTurned = false;

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

      
      if (!m_hasTurned)
      {
        // LookAtEnemy();
        m_hasTurned = true;
      }
      

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

  void LookAtEnemy()
  {
    // Vector3 lookAt = SingletonManager.Enemy.transform.position - SingletonManager.Player.transform.position;
    // SingletonManager.Player.transform.LookAt(lookAt);
    SingletonManager.Player.transform.LookAt(SingletonManager.Enemy.transform.position);
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
      SingletonManager.Player.GetComponent<CapsuleCollider>().enabled = false;
      SingletonManager.Player.GetComponent<CameraRotation>().enabled = false;

      SingletonManager.Player.GetComponent<Rigidbody>().useGravity = false;
      SingletonManager.Player.GetComponent<Rigidbody>().isKinematic = true;
      m_Timer = 1.5f;
      m_PlayKilling = true;

      Vector3 direction = (SingletonManager.Player.transform.position - SingletonManager.Enemy.transform.position).normalized;
      Vector3 start = SingletonManager.Enemy.transform.position;
      Vector3 playerEnd = start + (3 * direction);
      playerEnd.y = 0.9f;
      SingletonManager.Player.transform.position = playerEnd;

      LookAtEnemy();

      //Vector3 toTarget = SingletonManager.Player.transform.position - SingletonManager.Enemy.transform.position;
      //toTarget.y = 0.0f;
      //float turnRate = 10000.0f;
      //Quaternion lookRotation = Quaternion.LookRotation(toTarget);
      //transform.rotation = Quaternion.RotateTowards(transform.rotation, lookRotation, turnRate);
    }
  }

}
