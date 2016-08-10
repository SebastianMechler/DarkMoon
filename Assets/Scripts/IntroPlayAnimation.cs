using UnityEngine;
using System.Collections;

public class IntroPlayAnimation : MonoBehaviour
{

	
	void Start ()
  {
    Debug.Log("Playing animation");
    Animation anim = GetComponent<Animation>();
    anim.Play("IntroLogo2");
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
