using UnityEngine;
using System.Collections;

public class ProjectileEmitter : MonoBehaviour {

	public enum EmitterBehaviour {
		Static,
		TargetAdversary,//first arg is max player distance
		Star,//first arg is number
		Shotgun//first arg is number, second one is spread angle, third one is minSpeedFactor
	}

	public bool isEnemy {
		get;
		set;
	}
	public GameObject projectile;
	public int volleyCount = int.MaxValue;
	public float cooldown = 0f;
	private int volleyCounter = 0;
	public float volleyStartTime = 0f;

	[Header("Comportement de l'emetteur")]
	public EmitterBehaviour behaviour;
	public float[] behaviourArgs = {0.0f};
	private GameObject target;
	private float firingDelay;
	private float nextShot = 0;
	public float firingRate = 1;

	public bool keepTracking;
	//*
	public void SetProperties (ProjectileEmitter other) {
		this.isEnemy = other.isEnemy;
		this.projectile = other.projectile;
		this.volleyCount = other.volleyCount;
		this.volleyStartTime = other.volleyStartTime;
		this.cooldown = other.cooldown;
		this.behaviour = other.behaviour;
		this.behaviourArgs = new float[other.behaviourArgs.Length];
		other.behaviourArgs.CopyTo (this.behaviourArgs, 0);
		this.firingRate = other.firingRate;
	}//*/

	void Awake() {
		Debug.Log (projectile + " : " + projectile.GetInstanceID ());
	}

	void Start () {
		if (tag == "BossWeapon") {
			isEnemy = true;
		}
		firingDelay = 1.0f / firingRate;
		if (gameObject.transform.parent.gameObject.GetComponent<Character> ()) {
			isEnemy = gameObject.transform.parent.gameObject.GetComponent<Character> ().isEnemy;
		}
		if (behaviourArgs.Length >= 1) {
			behaviourArgs [0] = Mathf.Ceil (behaviourArgs [0]);
		}
		volleyStartTime = IngameTime.time + volleyStartTime;
	}

	void Update () {
		if (IngameTime.pause)
			return;
		if (transform.parent.gameObject.GetComponent<GargScript> () != null && behaviour != EmitterBehaviour.Star)
			transform.parent.gameObject.GetComponent<GargScript> ().Aiming =  (IngameTime.time < volleyStartTime);
		int tmpVolleyCounter = volleyCounter;
		if (IngameTime.time < volleyStartTime) {
			return;
		}
		if (IngameTime.time > nextShot) {
			if (projectile != null) {
				switch (behaviour) {
				case EmitterBehaviour.TargetAdversary:
					Player[] chars = GameObject.FindObjectsOfType (typeof(Player)) as Player[];
					//Debug.Log ("target players : " + chars.Length);
					float targetDistance = float.MaxValue;
					target = null;
					for (int i = 0; i < chars.Length; ++i) {
						float tmpDistance = Vector3.Distance (transform.position, chars [i].transform.position);
						if (chars[i].isEnemy != isEnemy && tmpDistance < targetDistance && tmpDistance < behaviourArgs[0]) {
							targetDistance = tmpDistance;
							target = chars [i].gameObject;
						}
					}
					//Debug.Log ("target players : " + chars.Length + " ; target is " + target);
					if (target != null) {
						GameObject pro = (GameObject) Instantiate (projectile, gameObject.transform.position, Quaternion.identity);
						pro.gameObject.GetComponent<Projectile>().isEnemy = isEnemy;
						pro.gameObject.GetComponent<Projectile> ().SetTarget (target);
						nextShot = IngameTime.time + firingDelay;
						volleyCounter++;
					}
					break;
				case EmitterBehaviour.Static:
					{
						GameObject pro = (GameObject) Instantiate (projectile, gameObject.transform.position, Quaternion.identity);
						pro.gameObject.GetComponent<Projectile>().isEnemy = isEnemy;
						pro.gameObject.GetComponent<Projectile> ().SetRotation (isEnemy ? transform.rotation * Quaternion.Euler (Vector3.forward * 180f) : transform.rotation);
						nextShot = IngameTime.time + firingDelay;
						volleyCounter++;
					}
					break;
				case EmitterBehaviour.Star:
					{
						for (float i = 0; i < behaviourArgs [0]; ++i) {
							GameObject pro = (GameObject) Instantiate (projectile, gameObject.transform.position, Quaternion.identity);
							pro.gameObject.GetComponent<Projectile>().isEnemy = isEnemy;
							pro.gameObject.GetComponent<Projectile> ().SetRotation (transform.rotation * Quaternion.Euler (Vector3.forward * ((360f / behaviourArgs [0]) * i + behaviourArgs[2] + (isEnemy ? 180f : 0f))));
						}
                            //Je rajoute un offset #désopasdéso
                            behaviourArgs[2] += behaviourArgs[1];
						nextShot = IngameTime.time + firingDelay;
						volleyCounter++;
					}
					break;
				case EmitterBehaviour.Shotgun:
					{
						for (float i = 0; i < behaviourArgs [0]; ++i) {
							GameObject pro = (GameObject) Instantiate (projectile, gameObject.transform.position, Quaternion.identity);
							pro.gameObject.GetComponent<Projectile>().isEnemy = isEnemy;
							float angleRange = (i / (behaviourArgs [0] - 1f)) * behaviourArgs [1] / 2f;
							pro.gameObject.GetComponent<Projectile> ().SetRotation (transform.rotation * Quaternion.Euler (Vector3.forward * ((isEnemy ? 180f : 0f) + Random.Range (-angleRange, angleRange))));
							pro.gameObject.GetComponent<Projectile> ().speed = pro.gameObject.GetComponent<Projectile> ().speed * ((behaviourArgs [2] - 1f) * (i / (behaviourArgs [0] - 1f)) + 1f);
						}
						nextShot = IngameTime.time + firingDelay;
						volleyCounter++;
					}
					break;
				}
				//pro.gameObject.GetComponent<Projectile> ().curveAngle = 10;
			}
		}
		if (tmpVolleyCounter != volleyCounter) {
			if (volleyCounter != (volleyCounter %= volleyCount)) {
				volleyStartTime = IngameTime.time + cooldown;
			}
		}
	}
}
