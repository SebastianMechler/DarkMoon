using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class IntroScene : MonoBehaviour {

  public Image gaLogo;
  private float fadingTime = 5.0f;

  public AnimationCurve curve;

	// Use this for initialization
	void Start () {
    Time.timeScale = 1.0f;
	}
	
	// Update is called once per frame
	void Update () {
    fadingTime -= Time.deltaTime;

    if (fadingTime <= 3.0f)
    {
      float alpha = curve.Evaluate(1 - (fadingTime / 3.0f));
      Color clr = gaLogo.color;
      clr.a = alpha;
      gaLogo.color = clr;
    }

    if(fadingTime < 0.0f)
    {
      fadingTime = 0.0f;
      gameObject.SetActive(false);

      SceneManager.LoadScene(1);
    }
	}
}
