using UnityEngine;
using System.Collections;

public enum PlayerState
{
    WALK,
    RUN,
    STAND,
    CROUCH,
    NONE,
}

public class PlayerMovement : MonoBehaviour
{
    public float m_movementSpeed = 2.0f; // movement speed of player
    public float m_runSpeedFactor = 1.5f; // factor which will be multiplied to increase speed when running
    public float m_crouchHeightMax = 1.0f; // collider will change to this value when uncrouching
    public float m_crouchHeightMin = 0.5f; // collider will change to this value when crouching
    public float m_crouchSpeed = 2.0f; // speed of crouching
    
    private PlayerState m_movementState = PlayerState.NONE; // current state of the player

    private Vector3 m_movementVector = new Vector3(0.0f, 0.0f, 0.0f); // movement vector
    private Rigidbody m_rigidbody;
    CapsuleCollider m_capsuleCollider;

  void Start()
  {
    m_capsuleCollider = this.GetComponent<CapsuleCollider>();
    if (m_capsuleCollider == null)
    {
      Debug.Log("Hey guys, make sure that the capsuleCollider is attached to the Player, else the crouching must be reworked.");
    }

    m_rigidbody = GetComponent<Rigidbody>();
  }

  void FixedUpdate()
  {
    // reset movement Vector
    m_movementVector = Vector3.zero;

    // Forward
    if (Input.GetKey(SingletonManager.GameManager.m_gameControls.forward))
    {
      m_movementVector += Vector3.Normalize(Camera.main.transform.forward);
      m_movementVector.y = 0.0f;
    }

    // Backward
    if (Input.GetKey(SingletonManager.GameManager.m_gameControls.backward))
    {
      m_movementVector += -Vector3.Normalize(Camera.main.transform.forward);
      m_movementVector.y = 0.0f;
    }

    // Left
    if (Input.GetKey(SingletonManager.GameManager.m_gameControls.left))
    {
      Vector3 tmp = Vector3.Normalize(Camera.main.transform.forward);
      m_movementVector += Vector3.Cross(tmp, Vector3.up);
      m_movementVector.y = 0.0f;
    }

    // Right
    if (Input.GetKey(SingletonManager.GameManager.m_gameControls.right))
    {
      Vector3 tmp = Vector3.Normalize(Camera.main.transform.forward);
      m_movementVector += -Vector3.Cross(tmp, Vector3.up);
      m_movementVector.y = 0.0f;
    }

    // make movementVector with length of 1
    m_movementVector.Normalize();

    // multiply run speed if key is pressed
    if (Input.GetKey(SingletonManager.GameManager.m_gameControls.run))
    {
      m_movementVector *= m_runSpeedFactor;
    }

     /* CROUCH WHILE WALKING
    if (Input.GetKey(SingletonManager.GameManager.m_gameControls.crouch))
    {
      Crouch(-m_crouchSpeed);
    }
    else
    {
      if (m_capsuleCollider.height != m_crouchHeightMax)
        Crouch(m_crouchSpeed);
    }*/
  

    // Update player state
    UpdatePlayerState();
    
    m_rigidbody.velocity = m_movementVector * 100.0f * Time.fixedDeltaTime;
  }

    void UpdatePlayerState()
    {
        if (m_movementVector == Vector3.zero)
        {
            // player is standing or crouching
            if (Input.GetKey(SingletonManager.GameManager.m_gameControls.crouch))
            {
                m_movementState = PlayerState.CROUCH;
                Crouch(-m_crouchSpeed);
            }
            else
            {
                m_movementState = PlayerState.STAND;
            }
        }
        else
        {
            // player is running or walking
            if (Input.GetKey(SingletonManager.GameManager.m_gameControls.run))
            {
                m_movementState = PlayerState.RUN;
            }
            else
            {
                m_movementState = PlayerState.WALK;
            }
        }

        if (m_movementState != PlayerState.CROUCH)
        {
          if (m_capsuleCollider.height != m_crouchHeightMax)
            Crouch(m_crouchSpeed);
        }
    }

  void Crouch(float value)
  {
    m_capsuleCollider.height = Mathf.Clamp(m_capsuleCollider.height += value * 500.0f * Time.fixedDeltaTime, m_crouchHeightMin, m_crouchHeightMax);
  }
}



/*
void FixedUpdate()
{
  // reset movement vector
  m_movementVector = Vector3.zero;

  //
  // Keyboard movement (WASD)
  //
  if (Input.GetKey(SingletonManager.GameManager.m_gameControls.forward))
  {
    m_movementVector += Vector3.forward;
  }
  if (Input.GetKey(SingletonManager.GameManager.m_gameControls.backward))
  {
    m_movementVector += Vector3.back;
  }
  if (Input.GetKey(SingletonManager.GameManager.m_gameControls.left))
  {
    m_movementVector += Vector3.left;
  }
  if (Input.GetKey(SingletonManager.GameManager.m_gameControls.right))
  {
    m_movementVector += Vector3.right;
  }

  // multiply run speed if key is pressed
  if (Input.GetKey(SingletonManager.GameManager.m_gameControls.run))
  {
    m_movementVector *= m_runSpeedFactor;
  }

  /* CROUCH WHILE WALKING
  if (Input.GetKey(SingletonManager.GameManager.m_gameControls.crouch))
  {
    Crouch(-m_crouchSpeed);
  }
  else
  {
    if (m_capsuleCollider.height != m_crouchHeightMax)
      Crouch(m_crouchSpeed);
  }
  

  // Update player state
  UpdatePlayerState();

  // multiply movementSpeed and fixedDeltaTime to the movement vector
  m_movementVector *= m_movementSpeed * Time.fixedDeltaTime;


  this.transform.Translate(m_movementVector);
}
*/
  