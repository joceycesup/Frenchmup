using UnityEngine;
using System.Collections;

public class Enemy : Character {
	public float firingRate = 1;
	public GameObject projectile;

	private float firingDelay;
	private float nextShot = 0;

	// Use this for initialization
	void Start () {
		firingDelay = 1.0f / firingRate;
		isEnemy = true;
	}
	
	// Update is called once per frame
	void Update () {
		if (Time.time > nextShot) {
			nextShot = Time.time + firingDelay;
			GameObject pro = (GameObject) Instantiate (projectile, gameObject.transform.position, Quaternion.identity);
			pro.gameObject.GetComponent<Projectile>().isEnemy = isEnemy;
			pro.gameObject.GetComponent<LinearProjectile> ().SetDirection (1, -2);
		}
	}

	public override string ToString() {
		return "Enemy";
	}
}
