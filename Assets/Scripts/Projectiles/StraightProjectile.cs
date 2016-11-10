using UnityEngine;
using System.Collections;

public class StraightProjectile : Projectile {

	protected override Vector3 getDeltaPosition () {
		return new Vector3 (0, speed * Time.deltaTime, 0);
	}
}
