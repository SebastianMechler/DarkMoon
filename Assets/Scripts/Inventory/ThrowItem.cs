using UnityEngine;
using System.Collections;

public class ThrowItem : MonoBehaviour
{
  Rigidbody m_rigidbody;

  void Start()
  {
    m_rigidbody = this.gameObject.GetComponent<Rigidbody>();
  }

	public void Throw(float itemForce)
  {
    Debug.Log("Throwing item...");

    Vector3 forward = Camera.main.transform.forward;
    m_rigidbody.AddForce(new Vector3(forward.x * itemForce, 0,  forward.z * itemForce), ForceMode.Impulse);
  }
}
