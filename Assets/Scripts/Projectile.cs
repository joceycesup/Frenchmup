using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public enum Pattern {
		None,
		Sinusoidal, // first arg is amplitude, second one is frequency
		Spiral // same as Sinusoidal
	}

	public enum BehaviourOverTime {
		DoNothing,
		Split,
		Leaf,//first arg is number, second one is spread angle, third one is gather angle
		Rotate
	}

	public bool isEnemy;
	public float maxLifeSpan = 10.0f;
	public float desintegrateTime = 0.3f;
	private float startTime = 0.0f;
	public float desintegrateStartTime = 0.0f;
	public float speed;
	public float acceleration = 0.0f;
	public float curveAngle = 0.0f;/*
	public float curveAngle {
		get { return _curveAngle; }
		set { _curveAngle = value / speed; }
	}//*/
	public Pattern pattern;
	public float[] patternArgs = {0.0f};
	private float[] patternLastValues;
	public BehaviourOverTime behaviour;
	public float[] behaviourArgs = {0.0f};
	public float behaviourTime = 0.0f;
	public float behaviourStartTime = 0.0f;
	public GameObject nextProjectile;

	void Awake () {
		transform.parent = ViewportHandler.viewport.transform;
		switch (pattern) {
		case Pattern.Sinusoidal:
			patternLastValues = new float[] { 0.0f };
			break;
		case Pattern.Spiral:
			patternLastValues = new float[] { 0.0f, 0.0f };
			break;
		}
	}

	void Start () {
		startTime = IngameTime.time;
		desintegrateStartTime = startTime + maxLifeSpan - desintegrateTime;
		behaviourStartTime = startTime + behaviourTime;
		gameObject.GetComponent<SpriteRenderer> ().color = Color.white;
	}
	/*
	public virtual void Update () {
		Debug.Log ("base class");
		gameObject.transform.Translate (getDeltaPosition ());
	}/*/
	void Update () {
		if (IngameTime.time >= desintegrateStartTime) {
			if (IngameTime.time >= desintegrateStartTime + desintegrateTime) {
				Destroy (gameObject);
			} else {
				Color tmpColor = gameObject.GetComponent<SpriteRenderer> ().color;
				tmpColor.a = 1.0f - (IngameTime.time - desintegrateStartTime) / desintegrateTime;
				gameObject.GetComponent<SpriteRenderer> ().color = tmpColor;
			}
		}
		speed += IngameTime.deltaTime * acceleration;
		UpdateProjectile ();
	}//*/

	protected virtual void UpdateProjectile () {
		if (behaviour != BehaviourOverTime.DoNothing) {
			if (IngameTime.time >= behaviourStartTime + behaviourTime) {
				switch (behaviour) {
				case BehaviourOverTime.Rotate:
					behaviour = BehaviourOverTime.DoNothing;
					transform.Rotate (new Vector3 (0.0f, 0.0f, behaviourArgs[0]));
					break;
				case BehaviourOverTime.Split:
					behaviour = BehaviourOverTime.DoNothing;
					for (int i = 0; i < behaviourArgs.Length; ++i) {
						GameObject newProjectile = (GameObject)Instantiate (nextProjectile != null ? nextProjectile : gameObject);
						newProjectile.transform.localRotation = transform.localRotation;
						newProjectile.transform.Rotate (new Vector3 (0.0f, 0.0f, behaviourArgs [i]));
						newProjectile.transform.position = gameObject.transform.position;
						newProjectile.GetComponent<Projectile> ().isEnemy = isEnemy;
					}
					Destroy (gameObject);
					break;
				case BehaviourOverTime.Leaf:
					behaviour = BehaviourOverTime.DoNothing;
					for (float i = 0; i < behaviourArgs[0]; ++i) {
						GameObject newProjectile = (GameObject)Instantiate (nextProjectile != null ? nextProjectile : gameObject);
						newProjectile.transform.localRotation = transform.localRotation;
						newProjectile.transform.Rotate (new Vector3 (0.0f, 0.0f, ((i / (behaviourArgs [0] - 1.0f)) - 0.5f) * behaviourArgs [1]));
						newProjectile.transform.position = gameObject.transform.position;
						newProjectile.GetComponent<Projectile> ().isEnemy = isEnemy;
						newProjectile.GetComponent<Projectile> ().curveAngle = ((i / (behaviourArgs [0] - 1.0f)) - 0.5f) * 2.0f * behaviourArgs [2];
					}
					Destroy (gameObject);
					break;
				}
			}
		}
		if (pattern == Pattern.Sinusoidal || pattern == Pattern.Spiral) {
			float factor = Mathf.Sin ((IngameTime.time - startTime) * patternArgs [1]);
			float dx = (factor - patternLastValues[0]) * patternArgs [0];
			float dy = 0.0f;
			patternLastValues[0] = factor;
			if (pattern == Pattern.Spiral) {
				factor = Mathf.Cos ((IngameTime.time - startTime) * patternArgs [1]);
				dy = (factor - patternLastValues[1]) * patternArgs [0];
				patternLastValues[1] = factor;
			}
			gameObject.transform.Translate (new Vector3 (dx, dy, 0.0f));
		}
		transform.Rotate (new Vector3 (0.0f, 0.0f, curveAngle * IngameTime.deltaTime * speed));
		gameObject.transform.Translate (new Vector3 (0.0f, speed * IngameTime.deltaTime, 0.0f));
	}

	public void SetRotation (Quaternion q) {
		gameObject.transform.rotation = q;
	}

	public void SetRotation (Vector3 angles) {
		gameObject.transform.rotation = new Quaternion (angles.x, angles.y, angles.z, 1.0f);
	}

	public void SetDirection (float dx, float dy) {
		Quaternion tmpRot = Quaternion.FromToRotation (Vector3.up, new Vector3 (dx, dy, 0.0f));
		Debug.Log (tmpRot);
		gameObject.transform.rotation = tmpRot;
	}

	public void SetDirection (Vector3 direction) {
		SetDirection (direction.x, direction.y);
	}

	public void SetTarget (GameObject target) {
		SetDirection (target.transform.position - transform.position);
	}

	public void Disintegrate () {
		desintegrateStartTime = IngameTime.time;
	}

	void OnBecameInvisible () {
		Destroy (gameObject);
	}
}
