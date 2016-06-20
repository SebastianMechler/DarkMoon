using UnityEngine;
using System.Collections;

public class Minimap : MonoBehaviour
{
  public Transform target;
	void Start ()
  {
	
	}
	
	void LateUpdate()
  {
    this.transform.position = new Vector3(target.transform.position.x, this.transform.position.y, target.transform.position.z);
	}
}
