using UnityEngine;
using System.Collections;

public class ForTestingPurposeOnly : MonoBehaviour
{

	// Use this for initialization
	void Start ()
  {
    Time.timeScale = 1.0f;
    SingletonManager.MouseManager.SetMouseState(MouseState.LOCKED);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
