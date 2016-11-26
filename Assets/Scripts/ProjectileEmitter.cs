﻿using UnityEngine;
using System.Collections;

public class ProjectileEmitter : MonoBehaviour {


	public enum EmitterBehaviour {
		Static,
		TargetAdversary,//first arg is max player distance
		Star,//first arg is number
		Shotgun//first arg is number, second one is spread angle, third one is minSpeedFactor
	}

	private bool isEnemy;
	public GameObject projectile;
	[Header("Comportement de l'emetteur")]
	public EmitterBehaviour behaviour;
	public float[] behaviourArgs = {0.0f};
	private GameObject target;
	private float firingDelay;
	private float nextShot = 0;
	public float firingRate = 1;

	void Start () {
		firingDelay = 1.0f / firingRate;
		if (gameObject.GetComponent<Character> ()) {
			isEnemy = gameObject.GetComponent<Character> ().isEnemy;
		}
		if (behaviourArgs.Length >= 1) {
			behaviourArgs [0] = Mathf.Ceil (behaviourArgs [0]);
		}
	}

	void Update () {
		if (IngameTime.time > nextShot) {
			if (projectile != null) {
				switch (behaviour) {
				case EmitterBehaviour.TargetAdversary:
					Character[] chars = GameObject.FindObjectsOfType (typeof(Character)) as Character[];
					float targetDistance = float.MaxValue;
					target = null;
					for (int i = 0; i < chars.Length; ++i) {
						float tmpDistance = Vector3.Distance (transform.position, chars [i].transform.position);
						if (chars[i].isEnemy != isEnemy && tmpDistance < targetDistance && tmpDistance < behaviourArgs[0]) {
							target = chars [i].gameObject;
						}
					}
					if (target != null) {
						GameObject pro = (GameObject) Instantiate (projectile, gameObject.transform.position, Quaternion.identity);
						pro.gameObject.GetComponent<Projectile>().isEnemy = isEnemy;
						pro.gameObject.GetComponent<Projectile> ().SetTarget (target);
						nextShot = IngameTime.time + firingDelay;
					}
					break;
				case EmitterBehaviour.Static:
					{
						GameObject pro = (GameObject) Instantiate (projectile, gameObject.transform.position, Quaternion.identity);
						pro.gameObject.GetComponent<Projectile>().isEnemy = isEnemy;
						pro.gameObject.GetComponent<Projectile> ().SetRotation (isEnemy ? transform.rotation * Quaternion.Euler (Vector3.forward * 180f) : transform.rotation);
						nextShot = IngameTime.time + firingDelay;
					}
					break;
				case EmitterBehaviour.Star:
					{
						for (float i = 0; i < behaviourArgs [0]; ++i) {
							GameObject pro = (GameObject) Instantiate (projectile, gameObject.transform.position, Quaternion.identity);
							pro.gameObject.GetComponent<Projectile>().isEnemy = isEnemy;
							pro.gameObject.GetComponent<Projectile> ().SetRotation (transform.rotation * Quaternion.Euler (Vector3.forward * ((360f / behaviourArgs [0]) * i + (isEnemy ? 180f : 0f))));
						}
						nextShot = IngameTime.time + firingDelay;
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
					}
					break;
				}
				//pro.gameObject.GetComponent<Projectile> ().curveAngle = 10;
			}
		}
	}
}