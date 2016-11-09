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
		gameObject.transform.position = getPosition ();
	}

	protected abstract Vector3 getPosition ();
}
