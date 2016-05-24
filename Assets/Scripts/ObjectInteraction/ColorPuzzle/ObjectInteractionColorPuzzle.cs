using UnityEngine;
using System.Collections;

public class ObjectInteractionColorPuzzle : ObjectInteractionBase
{

  void Start()
  {
    InitializeBase(ObjectInteractionType.COLOR_PUZZLE);
  }

  void Update()
  {
    UpdateBase();
  }

  public override void Interact()
  {
#if DEBUG
    Debug.Log("Interacting with colorPuzzleObject: " + this.gameObject.name);
#endif

    this.transform.parent.GetComponent<ColorPuzzle>().AddSequence(this.gameObject);

  }
}
