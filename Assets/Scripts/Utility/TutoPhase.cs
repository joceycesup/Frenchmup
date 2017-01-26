using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections;
#if UNITY_EDITOR
using UnityEditorInternal;
#endif
[System.Serializable]
public class TutoPhase {
	public string phaseName;
	public UnityEvent initAction;
	public UnityEvent predicateCheck;
	public UnityEvent resetAction;

	public void Init () {
		initAction.Invoke ();
	}

	public void Reset () {
		resetAction.Invoke ();
	}
}
