using UnityEngine;
using System.Collections;

public class LinearProjectile : Projectile {

	protected override Vector3 getDeltaPosition () {
		return new Vector3 (0, speed * Time.deltaTime, 0);
	}

	public void SetDirection (float dx, float dy) {
		gameObject.transform.rotation = Quaternion.FromToRotation (Vector3.up, new Vector3 (dx, dy, 0));
	}
}
