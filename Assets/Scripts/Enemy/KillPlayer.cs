using UnityEngine;
using UnityEngine.SceneManagement;

public class KillPlayer : MonoBehaviour {

  private Transform m_Player;
  private Transform m_Enemy;
  public Transform m_Head;
  public Animator m_EnemyAnimator;
  public Camera m_killCamera;

  public float m_speed = 2.0f;
  public float m_attackRange = 2.0f;
  private Vector3 direction;

  public float m_timer = 2.5f;
  private bool m_timerActive = false;

  private float distance;
  public float m_CameraOffset = 2.0f;
  
  void Start()
  {
    m_Enemy = SingletonManager.Enemy.transform;
    m_Player = SingletonManager.Player.transform;

    // Enemy Pivot cannot be used as it is not center of model
    Vector3 look = m_Head.position ;
    direction = (m_Enemy.position - m_Player.position).normalized;

    SingletonManager.Enemy.GetComponent<EnemyAiScript>().SetKillingActive();
    SingletonManager.Enemy.GetComponent<EnemyAiScript>().enabled = false;

    SingletonManager.Player.GetComponent<PlayerMovement>().enabled = false;
    SingletonManager.Player.GetComponent<CapsuleCollider>().enabled = false;
    SingletonManager.Player.GetComponent<CameraRotation>().enabled = false;

    SingletonManager.Player.GetComponent<Rigidbody>().useGravity = false;
    SingletonManager.Player.GetComponent<Rigidbody>().isKinematic = true;
    
    Vector3 cam = Camera.main.transform.position;
    m_killCamera.transform.position = cam;
    m_killCamera.transform.LookAt(look);
    m_killCamera.gameObject.SetActive(true);
    SingletonManager.Player.SetActive(false);
  }

  // Update is called once per frame
  void Update ()
  {
    if (m_timerActive)
    {
      m_timer -= Time.deltaTime;

      if (m_timer <= 0.0f)
      {
        // disable this gameobject
        gameObject.SetActive(false);

        // change Scene
        SingletonManager.AudioManager.Play(AudioType.PLAYER_DEATH);
        Time.timeScale = 1.0f;
        SingletonManager.MouseManager.SetMouseState(MouseState.UNLOCKED);
        SceneManager.LoadScene(StringManager.Scenes.deathScreen);
        return;
      }
    }
    else
    {
      distance = Vector3.Distance(m_Enemy.position, m_Player.position);
      Debug.Log("Distance to Player: " + distance);
      if (distance < m_attackRange)
      {
        // disable move option
        m_EnemyAnimator.SetBool("isWalking", false);
        m_EnemyAnimator.SetBool("isRunning", false);

        // do attack animation
        m_EnemyAnimator.SetTrigger("triggerKill");
        m_timerActive = true;
      }
      else
      {
        // do move animation
        m_EnemyAnimator.SetBool("isWalking", true);
        m_EnemyAnimator.SetBool("isRunning", false);

        Vector3 current = m_Enemy.position;
        m_Enemy.transform.Translate(direction * m_speed * Time.deltaTime);
        //m_Enemy.position = current - (direction * m_speed * Time.deltaTime);
      }
    }
  }
}
