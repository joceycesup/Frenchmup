using UnityEngine;
using System.Collections;

public abstract class Projectile : MonoBehaviour {
	public bool isEnemy;
	public float speed;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		gameObject.transform.Translate (getDeltaPosition ());
	}

	protected abstract Vector3 getDeltaPosition ();
}
