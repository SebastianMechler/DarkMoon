using UnityEngine;
using System.Collections;

public class ObjectInteractionChest : ObjectInteractionBase
{

  public float m_oxygenMin = 0.0f;
  public float m_oxygenMax = 0.0f;

  public float m_batteryMin = 0.0f;
  public float m_batteryMax = 0.0f;

  void Start()
  {
    InitializeBase(ObjectInteractionType.CHEST);
  }

  void Update()
  {
    UpdateBase();
  }

  public override void Interact()
  {
    base.Interact();

    float oxygenValue = Random.Range(m_oxygenMin, m_oxygenMax);
    float batteryValue = Random.Range(m_batteryMin, m_batteryMax);

#if DEBUG
    Debug.Log("Interacting with chest: " + this.gameObject.name + " -> Found " + oxygenValue.ToString() + " oxygen and " + batteryValue.ToString() + " battery.");
#endif

    

    SingletonManager.Player.GetComponent<PlayerOxygen>().Increase(oxygenValue);
    SingletonManager.Player.GetComponent<PlayerBattery>().Increase(batteryValue);

    Disable();
    Destroy(this.gameObject);
    /*
    if (m_isOpened)
    {
      m_animation.Play(StringManager.Animations.Chest.close);
      m_isOpened = false;
    }
    else
    {
      m_animation.Play(StringManager.Animations.Chest.open);
      m_isOpened = true;
    }
    */
  }
}
