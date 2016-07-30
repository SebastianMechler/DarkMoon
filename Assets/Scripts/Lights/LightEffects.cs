using UnityEngine;

[RequireComponent(typeof(Light))]
public class LightEffects : MonoBehaviour {

    public AnimationCurve m_FirstCurve = new AnimationCurve();
    public AnimationCurve m_SecondCurve;
    public AnimationCurve m_ThirdCurve;
    public AnimationCurve m_FourthCurve;
    public AnimationCurve m_FifthCurve;

    [Range(0.05f, 1.00f)]
    public float m_SpeedFactor = 0.15f;
    private float m_CurrentValue = 0.0f;
    private Light m_LightSource;

    [Tooltip("Use -1.0f for never dying or else any number greater 0.0f. integers work the best.")]
    public float m_LifeDuration = -1.0f;
    private bool m_DrainLife = false;

    // Use this for initialization
    void Start ()
    {
        m_LightSource = GetComponent<Light>();
        m_DrainLife = (m_LifeDuration > 0.0f);
    }
	
	// Update is called once per frame
	void Update ()
	{
	    m_CurrentValue += (m_CurrentValue > 1.0f ? -1.0f : Time.deltaTime * m_SpeedFactor);
	    
	    float lightRange = 0.0f;
        lightRange += m_FirstCurve.Evaluate(m_CurrentValue);
        lightRange += m_SecondCurve.Evaluate(m_CurrentValue);
        lightRange += m_ThirdCurve.Evaluate(m_CurrentValue);
        lightRange += m_FourthCurve.Evaluate(m_CurrentValue);
        lightRange += m_FifthCurve.Evaluate(m_CurrentValue);

        m_LightSource.intensity = lightRange;
        
	    if (m_DrainLife)
	    {
            m_LifeDuration -= Time.deltaTime;
            if (m_LifeDuration <= 0.0f)
            {
                m_LightSource.enabled = false;
                GetComponent<LightEffects>().enabled = false;
                m_LifeDuration = 0.0f;
            }
        }
	    
	}
}
