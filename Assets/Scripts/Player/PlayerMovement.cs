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
    public PlayerState m_movementState = PlayerState.NONE; // current state of the player

    private Vector3 m_movementVector = new Vector3(0.0f, 0.0f, 0.0f); // movement vector

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

        // Update player state
        UpdatePlayerState();
        
        // multiply movementSpeed and fixedDeltaTime to the movement vector
        m_movementVector *= m_movementSpeed * Time.fixedDeltaTime;

        // Apply movement
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
    }

}
