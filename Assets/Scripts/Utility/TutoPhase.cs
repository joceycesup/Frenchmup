using UnityEngine;
using UnityEngine.Events;
using System.Collections;

[System.Serializable]
public class TutoPhase {
	public string phaseName;
	public UnityEvent initAction;
	public UnityEvent resetAction;

	public void Init () {
		initAction.Invoke ();
	}

	public void Reset () {
		resetAction.Invoke ();
	}
}
