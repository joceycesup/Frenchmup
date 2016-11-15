using UnityEngine;
using System.Collections;

public class CurvedProjectile : LinearProjectile {
	public float curveAngle = 0.0f;

	protected override void UpdateProjectile () {
		transform.Rotate (new Vector3 (0, 0, curveAngle*Time.deltaTime));
		base.UpdateProjectile ();
	}
}
