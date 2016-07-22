using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class CollisionDetection : MonoBehaviour
{
    public Transform m_HeadPosition;
    private float m_PivotYOffset = 0.5f;
    private float m_TreshholdFactor = 1.2f;

    void OnTriggerStay(Collider other)
    {

        

        if (StringManager.Tags.player.Equals(other.gameObject.tag) && HidingZone.g_isPlayerHidden == false)
        {
            Vector3 playerPos = GameObject.FindGameObjectWithTag(StringManager.Tags.player).transform.position;
            // Pivot is slightly on top
            playerPos.y += m_PivotYOffset;
            float range = Vector3.Distance(playerPos, m_HeadPosition.position) * m_TreshholdFactor;
            // Debug.DrawRay(m_HeadPosition.position, (playerPos - m_HeadPosition.position), Color.cyan, 1.0f);

            RaycastHit[] hits = Physics.RaycastAll(transform.position, (playerPos - m_HeadPosition.position).normalized, range);

            bool playerInPlainView = true;
            for (int i = 0; i < hits.Length; i++)
            {
                // RaycastHit hit = hits[i];
                string thisTag = hits[i].collider.gameObject.tag;

                if (!thisTag.Equals(StringManager.Tags.Waypoints) && !thisTag.Equals(StringManager.Tags.interactableObject) && 
                    !thisTag.Equals(StringManager.Tags.enemy) && !thisTag.Equals(StringManager.Tags.player) &&
                    !thisTag.Equals(StringManager.Tags.noise) && !thisTag.Equals(StringManager.Tags.floor))
                {
                    // Debug.Log("Something named '" + hits[i].collider.gameObject.name + "' tagged '" + thisTag + "' is Blocking the View");
                    return;
                }
                
            }

            if (playerInPlainView)
            {
                Debug.Log("Player in Plain View!");
            }

            //GameManager.ClearDebugConsole();
            //Debug.Log(" †††† The Player just died a bit ††††");
            SingletonManager.AudioManager.Play(AudioType.PLAYER_DEATH);
            // Player died, load deathscreen
            Time.timeScale = 1.0f;
            SingletonManager.MouseManager.SetMouseState(MouseState.UNLOCKED);
            SceneManager.LoadScene(StringManager.Scenes.deathScreen);

        }
    }
}
