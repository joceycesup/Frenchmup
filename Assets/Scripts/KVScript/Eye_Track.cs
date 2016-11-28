using UnityEngine;
using System.Collections;

public class Eye_Track : MonoBehaviour {

	public Transform Anchor;
	public float maxDistance = 0.5f;
	public Transform Target;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {
		transform.position = Anchor.position + (Target.position - Anchor.position).normalized*maxDistance;
	}
}
