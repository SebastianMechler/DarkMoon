using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class DoorBehaviour : MonoBehaviour {

    public enum SoundType
    {
        SILENT,
        NORMAL,
        NOISY
    }

    public enum DoorState
    {
        OPENING,
        OPEN,
        CLOSING,
        CLOSED
    }

    public SoundType m_SoundType = SoundType.SILENT;
    private DoorState m_DoorState = DoorState.CLOSED;
    private const string m_AnimationName = "Take 001";
    private Animator m_Animator = null;
    private float m_Timer = 0.0f;

    public AudioSource m_AudioSourceSilent;
    public AudioSource m_AudioSourceNormal;
    public AudioSource m_AudioSourceNoisy;
	public Color m_OpenColor = Color.cyan;
	public Color m_OpeningColor = Color.green;
	public Color m_ClosedColor = Color.red;
	public Color m_ClosingColor = Color.yellow;
    public GameObject[] m_LightGameObjects = null;
    private int m_LightCount = 0;
    private Light[] m_Lights = null;
    
	
	public void OpeningDoor()
	{
		m_Animator.SetTrigger("triggerExitAnimation");
		ChangeDoorState(DoorState.OPENING);
	    // m_AudioSource.clip = Resources.Load<AudioClip>("");
	}

	private void ChangeDoorState(DoorState state)
	{
		m_DoorState = state;

		if(m_DoorState == DoorState.OPENING)
		{
			m_Animator.SetTrigger("triggerDoorOpening");
			ChangeLightColor(m_OpeningColor);
			m_Animator.speed = 1.0f;
			m_Timer = 0.6f;
		}
	}

	// Use this for initialization
	void Start () {
		m_Animator = GetComponent<Animator>();
	    // m_AudioSource = GetComponent<AudioSource>();

        m_LightCount = m_LightGameObjects.Length;
		m_Lights = new Light[m_LightCount];
		for (int i = 0; i < m_LightCount; i++)
		{
			m_Lights[i] = m_LightGameObjects[i].GetComponent<Light>();
			m_Lights[i].color = m_ClosedColor;
        }
	}

	void OnTriggerEnter(Collider other)
	{
		if(m_DoorState == DoorState.CLOSED || m_DoorState == DoorState.CLOSING)
		{
			if (other.gameObject.tag.Equals(StringManager.Tags.enemy))
			{
				ChangeDoorState(DoorState.OPENING);
			}
		}
	}

	void ChangeLightColor(Color color)
	{
		for (int i = 0; i < m_LightCount; i++)
		{
			m_Lights[i].color = color;
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.O) && m_DoorState == DoorState.CLOSED)
		{
			ChangeDoorState(DoorState.OPENING);
        }

		if(m_Timer > 0.0f)
		{
			m_Timer -= Time.deltaTime;
			return;
		}

		switch (m_DoorState)
		{
			case DoorState.OPENING:
				m_DoorState = DoorState.OPEN;
				m_Timer = 4.0f;
				m_Animator.speed = 0.0f;
				ChangeLightColor(m_OpenColor);
				break;

			case DoorState.OPEN:
				m_DoorState = DoorState.CLOSING;
				m_Timer = 0.6f;
				m_Animator.speed = 1.0f;
				ChangeLightColor(m_ClosingColor);
				break;

			case DoorState.CLOSING:
				m_DoorState = DoorState.CLOSED;
				m_Timer = 0.0f;
				m_Animator.SetTrigger("triggerExitAnimation");
				ChangeLightColor(m_ClosedColor);
				break;
		}
	}
}
