using UnityEngine;
using System.Collections;

public class ProjectileEmitter : MonoBehaviour {

	public enum EmitterBehaviour {
		Static,
		TargetPlayer,//first arg is max player distance
		Star,
		Leaf,//first arg is number, second one is spread angle, third one is gather angle
		Rotate
	}

	public bool isEnemy;
	public GameObject projectile;
	public EmitterBehaviour behaviour;
	public float[] behaviourArgs = {0.0f};
	private GameObject target;
	private float firingDelay;
	private float nextShot = 0;
	public float firingRate = 1;

	void Start () {
		firingDelay = 1.0f / firingRate;
	}

	void Update () {
		if (IngameTime.time > nextShot) {
			if (behaviour == EmitterBehaviour.TargetPlayer) {
				GameObject[] players = GameObject.FindGameObjectsWithTag ("Player");
				float targetDistance = target == null ? float.MaxValue : Vector3.Distance (transform.position, target.transform.position);
				for (int i = 0; i < players.Length; ++i) {
					if (Vector3.Distance (transform.position, players [i].transform.position) < targetDistance) {
						target = players [i];
					}
				}
			}
			nextShot = IngameTime.time + firingDelay;
			if (projectile != null) {
				GameObject pro = (GameObject) Instantiate (projectile, gameObject.transform.position, Quaternion.identity);
				pro.gameObject.GetComponent<Projectile>().isEnemy = isEnemy;
				//pro.gameObject.GetComponent<CurvedProjectile> ().SetDirection (1, -2);
				if (target != null) {
					pro.gameObject.GetComponent<Projectile> ().SetDirection (target);
				} else {
					pro.gameObject.GetComponent<Projectile> ().SetRotation (transform.rotation);
				}
				//pro.gameObject.GetComponent<Projectile> ().curveAngle = 10;
			}
		}
	}
}
