using UnityEngine;
using System.Collections;

public class GameSettings : MonoBehaviour {
	public static bool tutorial {
		get;
		private set;
	}

	void Awake () {
		tutorial = false;
	}

	void Start () {
	}

	void Update () {
	}
}
