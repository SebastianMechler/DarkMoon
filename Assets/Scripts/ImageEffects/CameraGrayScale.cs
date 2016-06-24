using UnityEngine;
using System.Collections;

public class CameraGrayScale : MonoBehaviour
{

  void OnRenderImage(RenderTexture source, RenderTexture destination)
  {
    float intensity = GrayScaleManager.m_intensity;
    Material mat = GrayScaleManager.m_material;

    if (intensity == 0.0f)
    {
      Graphics.Blit(source, destination);
      return;
    }

    mat.SetFloat("_EffectAmount", intensity);
    Graphics.Blit(source, destination, mat);
  }
}
