using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class NearbyPlayerCollision : MonoBehaviour
{
    private GameObject m_Enemy;

	void Start ()
	{
	    m_Enemy = GameObject.FindGameObjectWithTag(StringManager.Tags.enemy);
	}

    void FixedUpdate()
    {
        transform.position = m_Enemy.transform.position;
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
