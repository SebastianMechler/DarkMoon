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
	  if (enemy.transform.position.y <= 0.0f)
	  {
	    Debug.Break();
	  }
	}
}
