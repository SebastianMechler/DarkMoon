using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Rigidbody))]
public class RigidbodyNoiseCreator : MonoBehaviour {

  private Vector3 m_basePosition;
  private float m_timer = 5.0f;

	// Use this for initialization
	void Start () {
    m_basePosition = this.transform.position;
  }
	
	// Update is called once per frame
	void Update () {

    m_timer -= Time.deltaTime;
    if(m_timer >= 0.0f)
    {
      return;
    }
    m_timer = 5.0f;

    Vector3 curPos = this.transform.position;
    if(curPos != m_basePosition)
    {
      // todo Create Noise
      m_basePosition = curPos;
    }

	}
}
