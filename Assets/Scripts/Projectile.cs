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
		Leaf,//first arg is number, second one is spread angle, third one is gather angle
		Rotate
	}

	[Space(10)]
	public bool isEnemy;
	public float maxLifeSpan = 10.0f;
	public float desintegrateTime = 0.3f;
	private float startTime = 0.0f;
	public float desintegrateStartTime = 0.0f;
	[Space(10)]
	public float speed;
	public float maxSpeed;
	//If 0 alors on s'en fout
	public float acceleration = 0.0f;
	public float curveAngle = 0.0f;
	//Pour inverser l'angle
	public float waveLenght = 0;
	float waveTime;/*
	public float curveAngle {
		get { return _curveAngle; }
		set { _curveAngle = value / speed; }
	}//*/
	[Space(10)]
	public Pattern pattern;
	public float[] patternArgs = {0.0f};
	private float[] patternLastValues;
	[Space(10)]
	public BehaviourOverTime behaviour;
	public float[] behaviourArgs = {0.0f};
	public float behaviourTime = 0.0f;
	public float behaviourStartTime = 0.0f;
	public GameObject nextProjectile;

	//FXs
	public GameObject fx_Cancel;

	void Awake () {
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
		transform.parent = ViewportHandler.viewport.transform;
		startTime = IngameTime.time;
		desintegrateStartTime = startTime + maxLifeSpan - desintegrateTime;
		behaviourStartTime = startTime + behaviourTime;
		//gameObject.GetComponent<SpriteRenderer> ().color = Color.white;
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
				//gameObject.GetComponent<SpriteRenderer> ().color = tmpColor;
			}
		}
		speed += IngameTime.deltaTime * acceleration;
		if (speed < 0f) {
			speed = Mathf.Abs (speed);
			acceleration = Mathf.Abs (acceleration);
			transform.Rotate (0f, 0f, 180f);
		} else if (speed >= maxSpeed && maxSpeed != 0) {
			speed = maxSpeed;
		}
		if (curveAngle != 0 && waveLenght!=0 && Time.time >= waveTime + waveLenght) {
			waveTime = Time.time;
			curveAngle = -curveAngle;
		}

		if (behaviour != BehaviourOverTime.DoNothing) {
			if (IngameTime.time >= behaviourStartTime + behaviourTime) {
				switch (behaviour) {
				case BehaviourOverTime.Rotate:
					behaviour = BehaviourOverTime.DoNothing;
					transform.Rotate (new Vector3 (0.0f, 0.0f, behaviourArgs[0]));
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
	}//*/

	public void Remove () {
		if(fx_Cancel!=null)
			Instantiate (Resources.Load<GameObject>("Particules/Cancel"),transform.position-Vector3.forward*0.1f,Quaternion.identity);
		if (this.enabled)
			Destroy (gameObject);

	}

	public void SetRotation (Quaternion q) {
		gameObject.transform.rotation = q;
	}

	public void SetRotation (Vector3 angles) {
		gameObject.transform.rotation = new Quaternion (angles.x, angles.y, angles.z, 1.0f);
	}

	public void SetDirection (float dx, float dy) {
		Quaternion tmpRot = Quaternion.FromToRotation (Vector3.up, new Vector3 (dx, dy, 0.0f));
		//Debug.Log (tmpRot);
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
