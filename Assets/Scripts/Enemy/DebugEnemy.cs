using UnityEngine;
using System.Collections;

public class DebugEnemy : MonoBehaviour
{
  private GameObject enemy;

  void Start()
  {
    enemy = GameObject.FindGameObjectWithTag(StringManager.Tags.enemy);
  }

	// Update is called once per frame
	void Update () {

    // todo every 2 seconds if walking play step sound

	  if (enemy.transform.position.y <= 0.0f)
	  {
	    Debug.Break();
	  }
	}
}
