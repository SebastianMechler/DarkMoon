using UnityEngine;
using System.Collections;

public class DotHskDoorCloser : StateMachineBehaviour {
	
	private const float _shiftY = 1.48f;
	private const float _shiftZ = 0.976f;

	private Transform[] _slider = new Transform[3];
	override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
		if(_slider[0] == null) {
			_slider[0] = animator.transform.parent.Find("door2_up");
			_slider[1] = animator.transform.parent.Find("door2_side_1");
			_slider[2] = animator.transform.parent.Find("door2_side_2");
		}
		if (_slider[0] != null) {
			Vector3 _pos = _slider[0].localPosition;
			if (Mathf.Abs (_pos.y) < _shiftY / 2.0) {
				_pos.y = 0.0f;
				_slider[1].localPosition = _slider[2].localPosition = new Vector3 (0, 0, 0);
			} else {
				_pos.y = _shiftY;
				_slider[1].localPosition = new Vector3 (0, 0, +_shiftZ);
				_slider[2].localPosition = new Vector3 (0, 0, -_shiftZ);
			}
			_slider[0].localPosition = _pos;
		}
	}
	
}
