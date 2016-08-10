using UnityEngine;
using UnityEngine.UI;

public class Blinking : MonoBehaviour {

  public Texture2D m_fadeTexture;
  private float m_fadeSpeed = 0.2f;
  private int m_drawDepth = -1000;

  private float m_alpha = 1.0f;
  private float m_fadeDir = -1.0f;

  public bool m_run = false;

  public void Run()
  {
    m_run = true;
  }

  void OnGUI()
  {
    if (!m_run) return;

    m_alpha += m_fadeDir * m_fadeSpeed * Time.deltaTime;
    m_alpha = Mathf.Clamp01(m_alpha);

    Color clr = GUI.color;
    clr.a = m_alpha;
    GUI.color = clr;

    GUI.depth = m_drawDepth;
    GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), m_fadeTexture);
  }
  
}
