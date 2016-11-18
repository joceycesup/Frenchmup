using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public enum BehaviourOverTime {
		DoNothing,
		Spread,
		Rotate
	}

	public bool isEnemy;
	public float speed;
	public float acceleration = 0.0f;
	public float curveAngle = 0.0f;/*
	public float curveAngle {
		get { return _curveAngle; }
		set { _curveAngle = value / speed; }
	}//*/
	public BehaviourOverTime behaviour;
	public float behaviourTime = 0.0f;
	public GameObject nextProjectile;
	public float[] behaviourArgs = {0.0f};

	void Awake () {
		transform.parent = ViewportHandler.viewport.transform;
	}
	/*
	public virtual void Update () {
		Debug.Log ("base class");
		gameObject.transform.Translate (getDeltaPosition ());
	}/*/
	void Update () {
		speed += IngameTime.deltaTime * acceleration;
		UpdateProjectile ();
	}//*/

	protected virtual void UpdateProjectile () {
		if (behaviour != BehaviourOverTime.DoNothing) {
			if ((behaviourTime -= IngameTime.deltaTime) <= 0.0f) {
				switch (behaviour) {
				case BehaviourOverTime.Rotate:
					transform.Rotate (new Vector3 (0, 0, behaviourArgs[0]));
					break;
				case BehaviourOverTime.Spread:
					for (int i = 0; i < behaviourArgs.Length; ++i) {
						((GameObject)Instantiate (nextProjectile != null ? nextProjectile : gameObject)).transform.Rotate (new Vector3 (0, 0, behaviourArgs [i]));
					}
					Destroy (gameObject);
					break;
				}
				behaviour = BehaviourOverTime.DoNothing;
			}
		}
		transform.Rotate (new Vector3 (0, 0, curveAngle*IngameTime.deltaTime*speed));
		gameObject.transform.Translate (new Vector3 (0, speed * IngameTime.deltaTime, 0));
	}

	public void SetDirection (float dx, float dy) {
		gameObject.transform.rotation = Quaternion.FromToRotation (Vector3.up, new Vector3 (dx, dy, 0));
	}

	void OnBecameInvisible () {
		Destroy (gameObject);
	}
}
