using UnityEngine;
using System.Collections;

public class SoundBank : MonoBehaviour {

	private static GameObject _bank;

	public static GameObject bank {
		get { return _bank; }
	}

	void Awake () {
		if (GameObject.FindObjectsOfType<SoundBank> ().Length > 1) {
			Destroy (gameObject);
			return;
		}
		_bank = gameObject;
		DontDestroyOnLoad (gameObject);
	}
}
