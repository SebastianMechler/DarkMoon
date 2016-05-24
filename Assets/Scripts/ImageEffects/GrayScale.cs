using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class GrayScale : MonoBehaviour
{

  public float intensity;
  private Material material;

  // Creates a private material used to the effect
  void Awake()
  {
    material = new Material(Shader.Find("FX/GrayScale"));
    Debug.Log(material);
  }

  float x = 0.001f;

  void Update()
  {
    this.intensity += x;
    if (intensity >= 1.0f)
    {
      x = -x;
    }
    if (intensity <= 0.0f)
    {
      x = Mathf.Abs(x);
    }
  }

  // Postprocess the image
  void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    if (intensity == 0)
    {
      Graphics.Blit(source, destination);
      return;
    }

    material.SetFloat("_EffectAmount", intensity);
    Graphics.Blit(source, destination, material);
  }
}