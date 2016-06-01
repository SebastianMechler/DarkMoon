using UnityEngine;
using System.Collections;

public class GrayScaleResetTwo : ObjectInteractionBase
{
  void Start()
  {
    InitializeBase(ObjectInteractionType.CHEST);
  }

  void Update()
  {
    UpdateBase();
  }

  public new void Interact()
  {
    base.Interact();
#if DEBUG
    Debug.Log("Interacting with obj: " + this.gameObject.name);
#endif

    /*
    GrayScale grayScale = Camera.main.transform.GetComponent<GrayScale>();

    if (grayScale.m_intensity == 0.0f)
    {
      grayScale.m_intensity = 1.0f;
    }
    else
    {
      grayScale.m_intensity = 0.0f;
    }
    */
    
  }
}
