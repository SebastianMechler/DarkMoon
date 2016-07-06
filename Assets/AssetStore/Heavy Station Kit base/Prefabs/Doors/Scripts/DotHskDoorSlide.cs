using UnityEngine;
using System.Collections;

public class DotHskDoorSlide : MonoBehaviour {

	private Animator[] _animator = new Animator[3];
	private AudioSource _doorSnd = null;
	private AudioClip[] _sounds = new AudioClip[2];
	private bool _sndLoaded = false;
	private int _plaingSnd = -1; // 0-Open, 1-Close 

	void Start () {
		foreach(Transform child in transform.parent.transform){
			switch(child.name){
			case "door2_up": _animator[0] = child.GetComponent<Animator>(); break;
			case "door2_side_1": _animator[1] = child.GetComponent<Animator>(); break;
			case "door2_side_2": _animator[2] = child.GetComponent<Animator>(); break;
			case "D2_Sound": _doorSnd  = child.GetComponent<AudioSource>(); break;
			}
		};
		_sounds[0] = Resources.Load("Open_Sound") as AudioClip;
		_sounds[1] = Resources.Load("Close_Sound") as AudioClip;
		_sndLoaded = (_sounds[0]!=null) && (_sounds[1] != null);
		if (!_sndLoaded) {
			Debug.LogWarning("Silence mode:  audioclips \"Open_Sound\" and / or \"Close_Sound\" not found in the \"Resources\" directory");
		}
	}
	
 	void slide_door(Collider other, int _id){ // 0 - Open, 1 - Close
		string _anim = "Door_"+((_id == 0) ? "Open" : "Close");

		Debug.Log (_anim );

		if( ( _animator[0] != null) && (other.gameObject.tag == "Player") ){
			AnimatorStateInfo _st = _animator[0].GetCurrentAnimatorStateInfo(0);
			if( !_st.IsName(_anim) ){
				float _time = _st.normalizedTime;
				_time = (_time<1.0f && (_st.IsName("Door_Open") || _st.IsName("Door_Close"))) ? 1 - _time : 0.0f;
				if( _sndLoaded ){
					float _timeSnd = 0.0f;
					if( _doorSnd.isPlaying && (_id > 0) && (_plaingSnd != _id) ){
						_timeSnd = _sounds[_id].length - _doorSnd.time;
					}
					_doorSnd.clip = _sounds[_id];
					_doorSnd.time = _timeSnd;
					_plaingSnd = _id;
					_doorSnd.Play();
				}
				for (int i = 0; i < 3; i++) {
					_animator[i].Play (_anim, -1, _time);
				}
			}
		}
	}

	void OnTriggerEnter(Collider other){
		Debug.Log ("enter");
		slide_door(other, 0); // Open door
	}	

	void OnTriggerExit(Collider other){
		Debug.Log ("out");
		slide_door(other, 1); // Close door
	}	

}