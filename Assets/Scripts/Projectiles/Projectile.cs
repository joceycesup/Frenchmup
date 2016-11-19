using UnityEngine;
using System.Collections;

public class Projectile : MonoBehaviour {

	public enum BehaviourOverTime {
		DoNothing,
		Split,
		Leaf,//first arg is number, second one is spread angle, third one is gather angle
		Rotate
	}

	public bool isEnemy;
	public float maxLifeSpan = 10.0f;
	public float desintegrateTime = 0.3f;
	private float desintegrateStartTime = 0.0f;
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

	void Start () {
		desintegrateStartTime = IngameTime.time + maxLifeSpan - desintegrateTime;
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
			if ((behaviourTime -= IngameTime.deltaTime) <= 0.0f) {
				switch (behaviour) {
				case BehaviourOverTime.Rotate:
					behaviour = BehaviourOverTime.DoNothing;
					transform.Rotate (new Vector3 (0, 0, behaviourArgs[0]));
					break;
				case BehaviourOverTime.Split:
					behaviour = BehaviourOverTime.DoNothing;
					for (int i = 0; i < behaviourArgs.Length; ++i) {
						GameObject newProjectile = (GameObject)Instantiate (nextProjectile != null ? nextProjectile : gameObject);
						newProjectile.transform.localRotation = transform.localRotation;
						newProjectile.transform.Rotate (new Vector3 (0, 0, behaviourArgs [i]));
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
						newProjectile.transform.Rotate (new Vector3 (0, 0, ((i / (behaviourArgs [0] - 1.0f)) - 0.5f) * behaviourArgs [1]));
						newProjectile.transform.position = gameObject.transform.position;
						newProjectile.GetComponent<Projectile> ().isEnemy = isEnemy;
						newProjectile.GetComponent<Projectile> ().curveAngle = ((i / (behaviourArgs [0] - 1.0f)) - 0.5f) * 2.0f * behaviourArgs [2];
					}
					Destroy (gameObject);
					break;
				}
			}
		}
		transform.Rotate (new Vector3 (0, 0, curveAngle*IngameTime.deltaTime*speed));
		gameObject.transform.Translate (new Vector3 (0, speed * IngameTime.deltaTime, 0));
	}

	public void SetDirection (float dx, float dy) {
		Quaternion tmpRot = Quaternion.FromToRotation (Vector3.up, new Vector3 (dx, dy, 0));
		tmpRot = new Quaternion (tmpRot.z, tmpRot.y, tmpRot.x, tmpRot.w);
		gameObject.transform.rotation = tmpRot;
	}

	public void Disintegrate () {
		desintegrateStartTime = IngameTime.time;
	}

	void OnBecameInvisible () {
		Destroy (gameObject);
	}
}
