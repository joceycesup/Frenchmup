﻿using UnityEngine;
using System.Collections;

public class Magnet : MonoBehaviour {
	public float magnetSpeed;

	public int projectilesAttracted {
		get;
		private set;
	}

	void Start () {
		projectilesAttracted = 0;
	}

	void OnTriggerStay2D (Collider2D other) {
		//Debug.Log (gameObject + " : " + other.gameObject);
		if (other.gameObject == transform.parent.gameObject)
			return;
		//Debug.Log (transform.parent.gameObject.ToString () + " got triggered by " + other.gameObject.ToString ());
		if (other.gameObject.tag == "Projectile" && other.gameObject.GetComponent<Projectile> ().isEnemy) {
			float factor = 1.0f - Vector3.Distance (gameObject.transform.position, other.gameObject.transform.position) / gameObject.GetComponent<CircleCollider2D> ().radius;
			if (factor > 0) {
				if (other.gameObject.name != "MProjectile") {
					other.gameObject.name = "MProjectile";
					projectilesAttracted++;
				}
				float angle = ((gameObject.transform.position - other.gameObject.transform.position).x < 0f ? 1f : -1f) * Vector3.Angle (Vector3.up, gameObject.transform.position - other.gameObject.transform.position);/*
				Debug.Log (angle);
				Debug.DrawLine (other.gameObject.transform.position, other.gameObject.transform.position + Quaternion.Euler (0f, 0f, angle) * Vector3.up * Vector3.Distance(gameObject.transform.position, other.gameObject.transform.position), Color.red);/*/
				factor *= magnetSpeed * IngameTime.deltaTime;
				other.gameObject.transform.Translate (Vector3.Normalize (other.gameObject.transform.position - gameObject.transform.position) * factor);
				other.gameObject.transform.rotation = Quaternion.Euler (new Vector3 (0f, 0f, Mathf.LerpAngle(other.gameObject.transform.rotation.eulerAngles.z, angle, factor)));//*/
			}
		}
	}
}
