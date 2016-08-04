using UnityEngine;
using System.Collections;

// every bit is used for one state
// this is required to encode multiple states into one variable
public enum MovementState
{
    // each state must be multiplied with 2, else the bit's cant be encoded anymore
    NONE = 0x0,
    WALK = 0x1,
    RUN = 0x2,
    STAND = 0x4,
    CROUCH = 0x8,
}

public class PlayerMovement : MonoBehaviour
{
    [Tooltip("[0.0f to max] Defines the normal movement-speed of the player.")]
    public float m_movementSpeed = 2.0f; // movement speed of player

    [Tooltip("[0.0f to max] Defines the run-speed factor which will be multiplied to increase the run-speed (Example: 1.5f would be 1.5x faster).")]
    public float m_runSpeedFactor = 1.5f; // factor which will be multiplied to increase speed when running

    [Tooltip("[0.0f to max] Defines the height of the collider from the player which will be reset to when crouching ends. (1.0f is a good value here)")]
    public float m_crouchHeightMax = 1.0f; // collider will change to this value when uncrouching

    [Tooltip("[0.0f to [crouchHeightMax]] Defines the height of the collider from the player which will lowered to when crouching. (0.25f is a good value here)")]
    public float m_crouchHeightMin = 0.5f; // collider will change to this value when crouching

    [Tooltip("[0.0f to max] Defines the crouch-speed to move up/down the collider when crouching.")]
    public float m_crouchSpeed = 2.0f; // speed of crouching

    [Tooltip("[0.0f to max] defines the factor the speed will be multiplied with if crouching.")]
    public float m_crouchSpeedFactor = 0.5f;

    [Tooltip("[0.0f to max] Defines the distance to obstacles from players position to the air (Up) which the player must have in order to be able to uncrouch.")]
    public float m_uncrouchObstacleDistance = 0.7f;

    private PlayerOxygen m_PlayerOxygen = null;

    private MovementState m_movementState = MovementState.NONE; // current state of the player ==> multiple states will be encoded here bitwise

    private Vector3 m_movementVector = new Vector3(0.0f, 0.0f, 0.0f); // movement vector
    private Rigidbody m_rigidbody;
    CapsuleCollider m_capsuleCollider;
  
    void Start()
    {
        m_PlayerOxygen = this.GetComponent<PlayerOxygen>();
        m_capsuleCollider = this.GetComponent<CapsuleCollider>();
        if (m_capsuleCollider == null)
        {
            Debug.Log("Hey guys, make sure that the capsuleCollider is attached to the Player, else the crouching must be reworked.");
        }

        m_rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // reset movement state
        m_movementState = MovementState.NONE;

        // reset movement Vector
        m_movementVector = Vector3.zero;

        // Forward
        if (Input.GetKey(SingletonManager.GameManager.m_gameControls.forward))
        {
            m_movementVector += Vector3.Normalize(Camera.main.transform.forward);
        }

        // Backward
        if (Input.GetKey(SingletonManager.GameManager.m_gameControls.backward))
        {
            m_movementVector += -Vector3.Normalize(Camera.main.transform.forward);

        }

        // Left
        if (Input.GetKey(SingletonManager.GameManager.m_gameControls.left))
        {
            Vector3 tmp = Vector3.Normalize(Camera.main.transform.forward);
            m_movementVector += Vector3.Cross(tmp, Vector3.up);
        }

        // Right
        if (Input.GetKey(SingletonManager.GameManager.m_gameControls.right))
        {
            Vector3 tmp = Vector3.Normalize(Camera.main.transform.forward);
            m_movementVector += -Vector3.Cross(tmp, Vector3.up);
        }

        // Debug.DrawLine
        // XXXXXXXXXXXXXXXXXXXXXXXXXXXXXXX

        // make movementVector with length of 1
        m_movementVector.Normalize();

        // Update player state
        UpdatePlayerState();

    // Test output for all encoded MovementStates

    if (Input.GetKeyDown(SingletonManager.GameManager.m_gameControls.interactWithObject))
    {
      Debug.Log("===============================================");
      Debug.Log("WALK: " + HasMovementState(MovementState.WALK));
      Debug.Log("RUN: " + HasMovementState(MovementState.RUN));
      Debug.Log("STAND: " + HasMovementState(MovementState.STAND));
      Debug.Log("CROUCH: " + HasMovementState(MovementState.CROUCH));
      Debug.Log("===============================================");
    }
    

    CanUncrouch();

        if (m_movementVector != Vector3.zero)
        {
            m_movementVector = m_movementVector * m_movementSpeed * 100.0f * Time.fixedDeltaTime;
            m_movementVector.y = m_rigidbody.velocity.y; // this needs to be here, else the gravity on y-axis won't take effect on player while moving
            m_rigidbody.velocity = m_movementVector;
        }
        else
        {
            // player won't slide anymore
            m_rigidbody.velocity = new Vector3(0.0f, m_rigidbody.velocity.y, 0.0f);
        }
    }

    void UpdatePlayerState()
    {
        bool isRunKeyPressed = Input.GetKey(SingletonManager.GameManager.m_gameControls.run);
        bool isCrouchKeyPressed = Input.GetKey(SingletonManager.GameManager.m_gameControls.crouch);

        if (m_movementVector == Vector3.zero)
        {
            EncodeMovementState(MovementState.STAND);
            m_PlayerOxygen.m_regenerateStep = SingletonManager.GameManager.CurrentGameDifficultySettings.m_regnerationOxygenNormal;
        }
        else
        {
            // player is running or walking
            if (isRunKeyPressed && isCrouchKeyPressed == false)
            {
                m_movementVector *= m_runSpeedFactor;
                EncodeMovementState(MovementState.RUN);
                m_PlayerOxygen.m_regenerateStep = SingletonManager.GameManager.CurrentGameDifficultySettings.m_regenerationOxygenRunning;
            }
            else
            {
                EncodeMovementState(MovementState.WALK);
                m_PlayerOxygen.m_regenerateStep = SingletonManager.GameManager.CurrentGameDifficultySettings.m_regnerationOxygenNormal;
            }
        }

        // player is standing or crouching
        if (isCrouchKeyPressed)
        {
            EncodeMovementState(MovementState.CROUCH);
            Crouch(-m_crouchSpeed);
            m_movementVector *= m_crouchSpeedFactor;
        }
        else
        {
            if (m_capsuleCollider.height != m_crouchHeightMax)
            {
              if (CanUncrouch())
              {
                  Crouch(m_crouchSpeed);
              }
              else
              {
                // player is still crouching!
                EncodeMovementState(MovementState.CROUCH);
              }
            }
        }
    }

    void Crouch(float value)
    {
        m_capsuleCollider.height = Mathf.Clamp(m_capsuleCollider.height += value * 1.0f * Time.fixedDeltaTime, m_crouchHeightMin, m_crouchHeightMax);
    }

    void EncodeMovementState(MovementState state)
    {
        m_movementState |= state; // encode it with bit-or
    }

    public bool HasMovementState(MovementState state)
    {
        // check if the bit is set
        if ((m_movementState & state) == state)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

  public bool CanUncrouch()
  {
    // checks through raycast if the player has enough space to WorldSpace up to uncrouch himself
    float rayScale = 0.15f;

    // a ray will be cast to each side from the players position with a given offset
    // looks like this:
    //      *
    // * player *
    //      *
    // each * is a raycast and a raycast from the player itself will also be cast
    // with this method we can determine if he has any obstacles that would block him from uncrouching

    Ray rayMiddle = new Ray(this.gameObject.transform.position, Vector3.up);
    Ray rayLeft = new Ray(this.gameObject.transform.position + Vector3.left * rayScale, Vector3.up);
    Ray rayRight = new Ray(this.gameObject.transform.position + Vector3.right * rayScale, Vector3.up);
    Ray rayForward = new Ray(this.gameObject.transform.position + Vector3.forward * rayScale, Vector3.up);
    Ray rayBack = new Ray(this.gameObject.transform.position + Vector3.back * rayScale, Vector3.up);

    Debug.DrawLine(this.gameObject.transform.position, this.gameObject.transform.position + 5 * Vector3.up);
    if (Physics.Raycast(rayMiddle, m_uncrouchObstacleDistance) || Physics.Raycast(rayLeft, m_uncrouchObstacleDistance) || Physics.Raycast(rayRight, m_uncrouchObstacleDistance)
      || Physics.Raycast(rayForward, m_uncrouchObstacleDistance) || Physics.Raycast(rayBack, m_uncrouchObstacleDistance))
    {
      return false;
    }

    return true;
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
