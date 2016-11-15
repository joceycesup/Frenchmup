using UnityEngine;
using System.Collections;

public class LinearProjectile : Projectile {
	/*
	public override void Update () {
		base.Update ();
		Debug.Log ("child class");
	}//*/

	public void SetDirection (float dx, float dy) {
		gameObject.transform.rotation = Quaternion.FromToRotation (Vector3.up, new Vector3 (dx, dy, 0));
	}
}
