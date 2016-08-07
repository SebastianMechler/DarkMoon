using UnityEngine;

public class PlayerData : MonoBehaviour
{
  public enum PlayerGeneralLocation
  {
    INVALID,
    AREA_ONE,
    AREA_TWO,
    AREA_THREE
  }

  public PlayerGeneralLocation m_PlayerLocation = PlayerGeneralLocation.INVALID;
  public PlayerGeneralLocation m_EnemyLocation = PlayerGeneralLocation.INVALID;
  private GameObject m_Player = null;
  private GameObject m_Enemy = null;

  [Tooltip("Defines the TOP LEFT Corner AREA_ONE")]
  public Transform m_BorderAreaOne;       // Greater in X and Less in Z
  [Tooltip("Defines the BOTTOM LEFT Corner AREA_TWO")]
  public Transform m_BorderAreaTwo;       // Greater in X and Greater in Z
  [Tooltip("Defines the BOTTOM RIGHT Corner AREA_THREE")]
  public Transform m_BorderAreaThree;     // Less in X and Greater in Z

  public float m_RefreshRate = 5.0f;
  private float m_CheckRefresh;

  void Start()
  {
    m_Player = GameObject.FindGameObjectWithTag(StringManager.Tags.player);
    m_Enemy = GameObject.FindGameObjectWithTag(StringManager.Tags.enemy);
    m_CheckRefresh = m_RefreshRate;
  }

  public PlayerGeneralLocation GetPlayerLocation()
  {
    return m_PlayerLocation;
  }

  public PlayerGeneralLocation GetEnemyLocation()
  {
    return m_EnemyLocation;
  }

  private PlayerGeneralLocation UpdateLocation(GameObject target)
  {
    float xPos = target.transform.position.x;
    float zPos = target.transform.position.z;

    // Greater in X and Less in Z
    if (xPos >= m_BorderAreaOne.position.x && zPos <= m_BorderAreaOne.position.z)
    {
      // Debug.Log((target.tag == StringManager.Tags.player ? "Player " : "Enemy ") + "recently entered:  AREA_ONE");
      return PlayerGeneralLocation.AREA_ONE;
    }

    // Greater in X and Greater in Z
    if (xPos >= m_BorderAreaTwo.position.x && zPos >= m_BorderAreaTwo.position.z)
    {
      // Debug.Log((target.tag == StringManager.Tags.player ? "Player " : "Enemy ") + "recently entered:  AREA_TWO");
      return PlayerGeneralLocation.AREA_TWO;
    }

    // Less in X and Greater in Z
    if (xPos <= m_BorderAreaThree.position.x && zPos >= m_BorderAreaThree.position.z)
    {
      // Debug.Log((target.tag == StringManager.Tags.player ? "Player " : "Enemy ") + "recently entered:  AREA_THREE");
      return PlayerGeneralLocation.AREA_THREE;
    }

    return PlayerGeneralLocation.INVALID;
  }

	void FixedUpdate ()
  {

	  if (m_CheckRefresh > 0.0f)
	  {
	    m_CheckRefresh -= Time.fixedDeltaTime;
	    return;
	  }
	  else
	  {
	    m_CheckRefresh = m_RefreshRate;
	  }

    m_PlayerLocation = UpdateLocation(m_Player);
    m_EnemyLocation = UpdateLocation(m_Enemy);
  }
}
