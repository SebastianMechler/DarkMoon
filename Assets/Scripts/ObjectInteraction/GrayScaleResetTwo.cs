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

  public override void Interact()
  {
#if DEBUG
    Debug.Log("Interacting with obj: " + this.gameObject.name);
#endif

    GrayScale grayScale = Camera.main.transform.GetComponent<GrayScale>();

    if (grayScale.intensity == 0.0f)
    {
      grayScale.intensity = 1.0f;
    }
    else
    {
      grayScale.intensity = 0.0f;
    }
  }
}
