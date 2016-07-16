using UnityEngine;
using System.Collections;

public class SnapLightFading : MonoBehaviour
{
    public float m_DurationMinimum = 20.0f;
    public float m_DurationMaximum = 120.0f;

    private float m_Counter = -1.0f;
    private Light m_LightSource = null;

    // Use this for initialization
    void Start ()
    {
        m_LightSource = this.GetComponent<Light>();
        m_Counter = Random.Range(m_DurationMinimum, m_DurationMaximum);
    }
	
	// Update is called once per frame
	void Update () {
        m_Counter -= Time.deltaTime;

	    if (m_Counter < 0.0f)
	    {
	        m_LightSource.intensity = 0.0f;
            GetComponent<SnapLightFading>().enabled = false;
	    }
	}
}
