using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CollisionDetection : MonoBehaviour
{
  public Transform m_HeadPosition;
  private float m_PivotYOffset = 0.5f;
  private float m_TreshholdFactor = 1.2f;

  public Animator m_Animator;
  private float m_Timer = 4.0f;
  private bool m_PlayKilling = false;

  public void ignoreCollisionWithWaypoints(Collider collider)
  {
    GameObject[] allWaypoints = GameObject.FindGameObjectsWithTag(StringManager.Tags.Waypoints);
    for (int i = 0; i < allWaypoints.Length; i++)
    {
      Physics.IgnoreCollision(collider, allWaypoints[i].GetComponent<BoxCollider>());
    }

    allWaypoints = GameObject.FindGameObjectsWithTag(StringManager.Tags.noise);
    for (int i = 0; i < allWaypoints.Length; i++)
    {
      Physics.IgnoreCollision(collider, allWaypoints[i].GetComponent<BoxCollider>());
    }
  }

  void Start()
  {
    Collider collider = GetComponent<Collider>();
    ignoreCollisionWithWaypoints(collider);
  }

  void FixedUpdate()
  {
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

  void OnTriggerStay(Collider other)
  {
    if (!m_PlayKilling && StringManager.Tags.player.Equals(other.gameObject.tag) && HidingZone.g_isPlayerHidden == false)
    {
      bool playerInPlainView = true;
      
      // todo 2 more rays
      for (int j = 0; j < 5; j++)
      {

        Vector3 playerPos = GameObject.FindGameObjectWithTag(StringManager.Tags.player).transform.position;

        // Pivot is slightly on top
        playerPos.y += m_PivotYOffset - (j * 0.15f);
        float range = Vector3.Distance(playerPos, m_HeadPosition.position) * m_TreshholdFactor;

        Debug.DrawRay(m_HeadPosition.position, (playerPos - m_HeadPosition.position), Color.cyan, 2.0f);
        // Debug.DrawLine(m_HeadPosition.position, playerPos, Color.red, 2.0f);
        
        RaycastHit[] hits = Physics.RaycastAll(transform.position, (playerPos - m_HeadPosition.position).normalized, range);
        
        for (int i = 0; i < hits.Length; i++)
        {
          // RaycastHit hit = hits[i];
          string thisTag = hits[i].collider.gameObject.tag;

          if (!thisTag.Equals(StringManager.Tags.Waypoints) && !thisTag.Equals(StringManager.Tags.interactableObject) &&
              !thisTag.Equals(StringManager.Tags.enemy) && !thisTag.Equals(StringManager.Tags.player) &&
              !thisTag.Equals(StringManager.Tags.noise) && !thisTag.Equals(StringManager.Tags.floor))
          {
            // Debug.Log("Something named '" + hits[i].collider.gameObject.name + "' tagged '" + thisTag + "' is Blocking the View");
            playerInPlainView = false;
            break;
          }

        }
      }

      if (playerInPlainView)
      {
        Debug.Log("Player in Plain View!");
        SingletonManager.Enemy.GetComponent<KillPlayer>().enabled = true;

        // teleport
        //Vector3 direction = (SingletonManager.Player.transform.position - m_HeadPosition.position).normalized;
        //Vector3 start = SingletonManager.Enemy.transform.position;
        //Vector3 playerEnd = start + (3 * direction);
        //playerEnd.y = 0.9f;
        //SingletonManager.Player.transform.position = playerEnd;

        //// look
        //LookAtEnemy();


        //m_PlayKilling = true;
        //m_Timer = 1.5f;
        //// m_Animator.SetBool("triggerKillPlayer", true);
        //m_Animator.SetTrigger("triggerKill");
        //SingletonManager.Enemy.GetComponent<EnemyAiScript>().SetKillingActive(); // enable killing state
        //SingletonManager.Player.GetComponent<PlayerMovement>().enabled = false;
        //SingletonManager.Player.GetComponent<CapsuleCollider>().enabled = false;
        //SingletonManager.Player.GetComponent<CameraRotation>().enabled = false;
        //SingletonManager.Player.GetComponent<Rigidbody>().useGravity = false;
        //SingletonManager.Player.GetComponent<Rigidbody>().isKinematic = true;
        //// m_Animator.SetBool("triggerKillPlayer", false);
      }
    }
  }

  void LookAtEnemy()
  {
    //Vector3 lookAt = SingletonManager.Enemy.transform.position - SingletonManager.Player.transform.position;
    //SingletonManager.Player.transform.LookAt(lookAt);
    SingletonManager.Player.transform.LookAt(SingletonManager.Enemy.transform.position);
  }
}






/*
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CollisionDetection : MonoBehaviour
{
  public Transform m_HeadPosition;
  private float m_PivotYOffset = 0.5f;
  private float m_TreshholdFactor = 1.2f;

  public Animator m_Animator;
  private float m_Timer = 4.0f;
  private bool m_PlayKilling = false;

  public void ignoreCollisionWithWaypoints(Collider collider)
  {
    GameObject[] allWaypoints = GameObject.FindGameObjectsWithTag(StringManager.Tags.Waypoints);
    for (int i = 0; i < allWaypoints.Length; i++)
    {
      Physics.IgnoreCollision(collider, allWaypoints[i].GetComponent<BoxCollider>());
    }

    allWaypoints = GameObject.FindGameObjectsWithTag(StringManager.Tags.noise);
    for (int i = 0; i < allWaypoints.Length; i++)
    {
      Physics.IgnoreCollision(collider, allWaypoints[i].GetComponent<BoxCollider>());
    }
  }

  void Start()
  {
    Collider collider = GetComponent<Collider>();
    ignoreCollisionWithWaypoints(collider);
  }

  void FixedUpdate()
  {
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

  void OnTriggerStay(Collider other)
  {
    if (!m_PlayKilling && StringManager.Tags.player.Equals(other.gameObject.tag) && HidingZone.g_isPlayerHidden == false)
    {
      bool playerInPlainView = true;
      
      // todo 2 more rays
      for (int j = 0; j < 5; j++)
      {

        Vector3 playerPos = GameObject.FindGameObjectWithTag(StringManager.Tags.player).transform.position;

        // Pivot is slightly on top
        playerPos.y += m_PivotYOffset - (j * 0.15f);
        float range = Vector3.Distance(playerPos, m_HeadPosition.position) * m_TreshholdFactor;

        Debug.DrawRay(m_HeadPosition.position, (playerPos - m_HeadPosition.position), Color.cyan, 2.0f);
        // Debug.DrawLine(m_HeadPosition.position, playerPos, Color.red, 2.0f);
        
        RaycastHit[] hits = Physics.RaycastAll(transform.position, (playerPos - m_HeadPosition.position).normalized, range);
        
        for (int i = 0; i < hits.Length; i++)
        {
          // RaycastHit hit = hits[i];
          string thisTag = hits[i].collider.gameObject.tag;

          if (!thisTag.Equals(StringManager.Tags.Waypoints) && !thisTag.Equals(StringManager.Tags.interactableObject) &&
              !thisTag.Equals(StringManager.Tags.enemy) && !thisTag.Equals(StringManager.Tags.player) &&
              !thisTag.Equals(StringManager.Tags.noise) && !thisTag.Equals(StringManager.Tags.floor))
          {
            // Debug.Log("Something named '" + hits[i].collider.gameObject.name + "' tagged '" + thisTag + "' is Blocking the View");
            playerInPlainView = false;
            break;
          }

        }
      }

      if (playerInPlainView)
      {
        Debug.Log("Player in Plain View!");

        Vector3 direction = (SingletonManager.Player.transform.position - m_HeadPosition.position).normalized;
        Vector3 start = SingletonManager.Enemy.transform.position;
        Vector3 playerEnd = start + (3 * direction);
        playerEnd.y = 0.9f;
        SingletonManager.Player.transform.position = playerEnd;

        LookAtEnemy();


        m_PlayKilling = true;
        m_Timer = 1.5f;
        // m_Animator.SetBool("triggerKillPlayer", true);
        m_Animator.SetTrigger("triggerKill");
        SingletonManager.Enemy.GetComponent<EnemyAiScript>().SetKillingActive();
        SingletonManager.Player.GetComponent<PlayerMovement>().enabled = false;
        SingletonManager.Player.GetComponent<CapsuleCollider>().enabled = false;
        SingletonManager.Player.GetComponent<CameraRotation>().enabled = false;
        SingletonManager.Player.GetComponent<Rigidbody>().useGravity = false;
        SingletonManager.Player.GetComponent<Rigidbody>().isKinematic = true;
        // m_Animator.SetBool("triggerKillPlayer", false);
      }
    }
  }

  void LookAtEnemy()
  {
    //Vector3 lookAt = SingletonManager.Enemy.transform.position - SingletonManager.Player.transform.position;
    //SingletonManager.Player.transform.LookAt(lookAt);
    SingletonManager.Player.transform.LookAt(SingletonManager.Enemy.transform.position);
  }
}
  */


