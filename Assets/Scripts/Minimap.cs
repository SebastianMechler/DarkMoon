using UnityEngine;
using System.Collections;

public class Minimap : MonoBehaviour
{
  public Transform m_target; // player
  public bool m_allowRotation = true; // should the minimap be rotated when the player rotates?
  public float m_viewDistance = 30.0f; // view distance of camera, the higher, the more the player will see in minimap
  public float m_batteryCost = 1.0f; // costs when enabled each frame
  private bool m_enabled = true;

  void Start()
  {
    SetViewDistance(m_viewDistance);
  }

	void Update()
  {

    // TESTING PURPOSE ONLY: increase size
    if (Input.GetKeyDown(KeyCode.KeypadPlus))
    {
      SetViewDistance(GetViewDistance() + 5.0f);
    }

    // TESTING PURPOSE ONLY: decrease size
    if (Input.GetKeyDown(KeyCode.KeypadMinus))
    {
      SetViewDistance(GetViewDistance() - 5.0f);
    }

    // enable or disable
    if (Input.GetKeyDown(SingletonManager.GameManager.m_gameControls.ui_toggleMinimap))
    {
      SetEnableState(!m_enabled);
    }

    if (m_enabled == false)
      return;

    this.transform.position = new Vector3(m_target.transform.position.x, this.transform.position.y, m_target.transform.position.z);
    SingletonManager.Player.GetComponent<PlayerBattery>().Increase(-m_batteryCost * Time.deltaTime);

    if (m_allowRotation)
    {
      // Get Y-Axis rotation from player and rotate Y-Axis of camera
      transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, m_target.transform.localEulerAngles.y, transform.localEulerAngles.z);
    }
  }

  public float GetViewDistance()
  {
    return GetComponent<Camera>().orthographicSize;
  }

  public void SetViewDistance(float viewDistance)
  {
    GetComponent<Camera>().orthographicSize = Mathf.Clamp(viewDistance, 0.0f, 100.0f);
  }

  public void SetEnableState(bool enabled)
  {
    this.m_enabled = enabled;
    SingletonManager.UIManager.ToggleMinimap(enabled);
  }

  public static Minimap GetInstance()
  {
    return GameObject.Find(StringManager.Names.minimap).GetComponent<Minimap>();
  }
}
