using System;
using UnityEngine;
using System.Collections;

public class DotHskGate2: MonoBehaviour {

	// Static data
	private static bool _initiated = false;
	private static Texture tipOpenGate;
	private static Texture tipCloseGate;
	private static bool _tips_loaded = false;
	private static AudioClip[] _sounds = new AudioClip[2];
	private static bool _snd_loaded = false;

	// Instance specific data
	private bool _active = false;
	private Animator _animator = null;
	private AudioSource _gateSnd = null;

	void Start(){
		GameObject _go = null;
		if (!_initiated) {
			_initiated = true;
			_go = GameObject.FindGameObjectWithTag ("Player");
			if (_go == null) {
				Debug.LogWarning ("Not found FirstPersonController component with tag \"Player\"!");
			} else {
				tipOpenGate = Resources.Load ("tip_openGate", typeof(Texture)) as Texture;
				tipCloseGate = Resources.Load ("tip_closeGate", typeof(Texture)) as Texture;
				_tips_loaded = (tipOpenGate != null) && (tipCloseGate != null);
				if (!_tips_loaded) {
					Debug.LogWarning ("Texture file(s) for tips \"tip_openGate.png\" and / or \"tip_closeGate.png\" not found in the \"Resources\" directory");
				}
				_sounds[0] = Resources.Load("Open_Sound") as AudioClip;
				_sounds[1] = Resources.Load("Close_Sound") as AudioClip;
				_snd_loaded = (_sounds[0]!=null) && (_sounds[1] != null);
				if (!_snd_loaded) {
					Debug.LogWarning("Silence mode:  audioclips \"Open_Sound\" and / or \"Close_Sound\" not found in the \"Resources\" directory");
				}
			}
		}
		if (_initiated) {
			foreach(Transform child in transform.parent.transform.parent.transform){
				switch(child.name){
				case "Gate2": _animator = child.GetComponent<Animator>(); break;
				case "Gate2_Sound": _gateSnd = child.GetComponent<AudioSource>(); break;
				}
			};
		}
	}
	
	void Update() {
		string st;
		if(_active && Input.GetKeyDown(KeyCode.E)){
			st = getNextStatus();
			if(st!=""){
				if (_animator != null) {
					_animator.Play ("G1_" + st);
				}
				if (_snd_loaded) {
					_gateSnd.clip = _sounds [(st == "Open") ? 0 : 1];
					_gateSnd.Play ();
				}
			}
		}
	}
	
	void OnTriggerEnter (Collider other) {
		_active = (other.gameObject.tag == "Player");
	}

	void OnTriggerExit (Collider other) {
		if(other.gameObject.tag == "Player"){
			_active = false;
		}
	}
	
	void OnGUI(){
		Texture _tmp;
		string st;
		if (_active && (_tips_loaded)) {
			st = getNextStatus();
			if(st !=""){
				_tmp = (st=="Close") ? tipCloseGate : tipOpenGate;
				float _sw = Screen.width;
				float _sh = Screen.height;
				float _tw = _tmp.width;
				float _th = _tmp.height;
				GUI.DrawTexture (new Rect ((_sw - _tw) / 2, _sh - 36 - _th, _tw, _th), _tmp, ScaleMode.ScaleToFit, true); 
			}
		}
	}

	string getNextStatus(){
		AnimatorStateInfo st = _animator.GetCurrentAnimatorStateInfo(0);
		return st.IsName("Idle_closed") ? "Open" : (st.IsName("Idle_open") ? "Close" : "");
	}

}
