using UnityEngine;
using System.Collections;

public abstract class Projectile : MonoBehaviour {
	public bool isEnemy;
	public float speed;
	public float acceleration = 0.0f;

	void Start () {
		transform.parent = ViewportHandler.viewport.transform;
	}
	/*
	public virtual void Update () {
		Debug.Log ("base class");
		gameObject.transform.Translate (getDeltaPosition ());
	}/*/
	void Update () {
		speed += Time.deltaTime * acceleration;
		UpdateProjectile ();
	}//*/

	protected virtual void UpdateProjectile () {
		gameObject.transform.Translate (new Vector3 (0, speed * Time.deltaTime, 0));
	}
}
