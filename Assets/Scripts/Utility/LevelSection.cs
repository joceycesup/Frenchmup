using UnityEngine;
using System.Collections;

public class LevelSection : MonoBehaviour {

	public enum LevelBehaviour {
		Linear,
		Static,
		Loop
	}

	public GameObject behaviourLock;
	public LevelBehaviour behaviour;

	void Start () {
	}

	void Update () {
	}
}
