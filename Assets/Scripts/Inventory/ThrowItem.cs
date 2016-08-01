using UnityEngine;
using System.Collections;

public class ThrowItem : MonoBehaviour
{
  Rigidbody m_rigidbody;
  bool m_isFlying = false;
  private float m_soundDelay = 0.25f;
  private float m_currentSound = 0.0f;

  void Start()
  {
    m_rigidbody = this.gameObject.GetComponent<Rigidbody>();
  }

  void Update()
  {
    if (m_isFlying)
    {
      if (m_rigidbody.velocity == Vector3.zero)
      {
        // play sound
        //SingletonManager.AudioManager.Play(AudioType.PUZZLE_FAILURE);
        m_isFlying = false;
        // ignore physics-collision between item and enemy
        //Physics.IgnoreCollision(this.gameObject.GetComponent<Collider>(), SingletonManager.Enemy.GetComponent<Collider>(), false);
      }
      //Debug.Log("VELOCITY: " + m_rigidbody.velocity);
    }

    if (m_currentSound > 0.0f)
    {
      m_currentSound -= Time.deltaTime;
      if (m_currentSound < 0.0f)
      {
        m_currentSound = 0.0f;
      }
    }
  }

  public void OnCollisionEnter(Collision other)
  {
    if (m_isFlying && other.gameObject.tag != StringManager.Tags.player && other.gameObject.tag != StringManager.Tags.enemy)
    {
      if (m_currentSound <= 0.0f)
      {
        SingletonManager.AudioManager.Play(AudioType.ITEM_COLLISION_TOOLWRENCH);
        m_currentSound = m_soundDelay;
      }
    }
      
  }

  public void Throw(float itemForce)
  {
    Debug.Log("Throwing item...");
    m_isFlying = true;
    SingletonManager.AudioManager.Play(AudioType.THROW_ITEM);
    Vector3 forward = Camera.main.transform.forward;
    m_rigidbody.AddForce(new Vector3(forward.x * itemForce, forward.y * itemForce, forward.z * itemForce), ForceMode.Impulse);

    // ignore physics-collision between item and enemy
    Physics.IgnoreCollision(this.gameObject.GetComponent<Collider>(), SingletonManager.Enemy.GetComponent<Collider>(), true);

    if (gameObject.GetComponent<Light>() != null)
      gameObject.AddComponent<SnapLightFading>();

    // Recollectable item?
    if (SingletonManager.GameManager.CurrentGameDifficultySettings.m_itemsRecollectable == false)
    {
      //GetComponent<ObjectInteractionItem>().m_item.m_isThrowable = false;
      GetComponent<ObjectInteractionBase>().SetInteractionState(false);
    }
  }

  public bool IsFlying()
  {
    return m_isFlying;
  }
}
