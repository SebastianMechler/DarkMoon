using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class GrayScale : MonoBehaviour
{
  [Tooltip("[0.0f to max] Defines the time the GrayScale effect will fade in in seconds. (30.0f would be 30 seconds)")]
  public float m_effectTime = 30.0f; // time in seconds for grayscale effect fading

  private float m_intensity;
  private Material m_material;
  private bool m_isEnabled = false;

  private float m_intensityMin = 0.0f;
  private float m_intensityMax = 1.0f;

  // Creates a private material used to the effect
  void Awake()
  {
    m_material = new Material(Shader.Find("FX/GrayScale"));
  }

  void Update()
  {
    if (m_isEnabled)
    {
      // add value over time
      m_intensity += Mathf.Lerp(m_intensityMin, m_intensityMax, Time.deltaTime / m_effectTime);
    }
    else
    {
      // subtract value over time
      m_intensity -= Mathf.Lerp(m_intensityMin, m_intensityMax, Time.deltaTime / m_effectTime);
    }
    
  }

  void LateUpdate()
  {
    m_intensity = Mathf.Clamp(m_intensity, m_intensityMin, m_intensityMax); // IMPORTANT
  }

  // Postprocess the image
  void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    if (m_intensity == 0)
    {
      Graphics.Blit(source, destination);
      return;
    }

    m_material.SetFloat("_EffectAmount", m_intensity);
    Graphics.Blit(source, destination, m_material);
  }

  public void Enable()
  {
    m_isEnabled = true;
  }

  public void Disable()
  {
    m_isEnabled = false;
  }
}