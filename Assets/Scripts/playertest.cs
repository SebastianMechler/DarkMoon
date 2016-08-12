using UnityEngine;
using System.Collections;

public class playertest : MonoBehaviour {

  public GameObject go;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
    this.transform.LookAt(go.transform);
    Camera.main.transform.LookAt(go.transform);
	}
}
