using UnityEngine;
using System.Collections;

public class StraightProjectile : Projectile {

	protected override Vector3 getPosition () {
		Vector3 res = gameObject.transform.position;
		res.y += speed * Time.deltaTime;
		return res;
	}
}
