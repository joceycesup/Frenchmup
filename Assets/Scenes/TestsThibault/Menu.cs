using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

	void Start () {
	}

	void Update () {
		if (Input.GetKeyDown (KeyCode.UpArrow)) {
			gameObject.GetComponent<GameSettings> ().LoadGame (true);
		} else if (Input.GetKeyDown (KeyCode.DownArrow)) {
			gameObject.GetComponent<GameSettings> ().LoadGame (false);
		}
	}
}
