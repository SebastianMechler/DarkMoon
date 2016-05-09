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
    CapsuleCollider m_capsuleCollider;

  void Start()
  {
    m_capsuleCollider = this.GetComponent<CapsuleCollider>();
    if (m_capsuleCollider == null)
    {
      Debug.Log("Hey guys, make sure that the capsuleCollider is attached to the Player, else the crouching must be reworked.");
    }
  }

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
        */

        // Update player state
        UpdatePlayerState();
        
        // multiply movementSpeed and fixedDeltaTime to the movement vector
        m_movementVector *= m_movementSpeed * Time.fixedDeltaTime;

    // Apply movement
    //if (m_movementVector != Vector3.zero)
    //{
    //  Vector3 rot = new Vector3(this.transform.rotation.x, this.transform.rotation.y, this.transform.rotation.z);
    //  this.GetComponent<Rigidbody>().velocity = rot + m_movementVector * 100.0f;
    //}

        this.transform.Translate(m_movementVector);
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
    Debug.Log(value);
    m_capsuleCollider.height = Mathf.Clamp(m_capsuleCollider.height += value * Time.deltaTime, m_crouchHeightMin, m_crouchHeightMax);
  }
}
