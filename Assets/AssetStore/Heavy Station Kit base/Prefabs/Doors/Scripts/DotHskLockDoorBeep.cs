using UnityEngine;
using System.Collections;

public class DotHskLockDoorBeep : MonoBehaviour {

	private AudioSource _doorSnd = null;

	void Start () {
		GameObject _sound;
		if ((_sound = transform.parent.transform.Find ("Lock_Sound").gameObject) != null) {
			_doorSnd = _sound.GetComponent<AudioSource>();
		}
	}

	void OnTriggerEnter(Collider other){
		if ((_doorSnd != null) && (other.gameObject.tag == "Player")) {
			_doorSnd.Play ();
		}
	}	


}