using UnityEngine;
using System.Collections;

public class ColorPuzzleFailure : MonoBehaviour
{
  private float m_time = 1.0f; // duration of effect

  private Renderer m_renderer;

	void Start()
  {
    m_renderer = this.gameObject.GetComponent<Renderer>();
  }

  void Update()
  {
    this.m_time -= Time.deltaTime;

    // if effect is over, remove the component from the object
    if (m_time > 0.8f)
    {
      m_renderer.enabled = false;
    }
    else if (m_time > 0.6f)
    {
      m_renderer.enabled = true;
    }
    else if (m_time > 0.4f)
    {
      m_renderer.enabled = false;
    }
    else if (m_time > 0.2f)
    {
      m_renderer.enabled = true;
    }
    else if (m_time <= 0.0f)
    {
      Component.Destroy(this);
    }
  }
}
