using UnityEngine;
using System.Collections;

public class Enemy : Character {
	public bool isStatic = false;
	public float firingRate = 1;
	public GameObject projectile;

	private float firingDelay;
	private float nextShot = 0;
	private EnemyGroup m_group;

	void Awake () {
		firingDelay = 1.0f / firingRate;
		_isEnemy = true;
		m_group = transform.parent.gameObject.GetComponent<EnemyGroup> ();
	}

	protected override void UpdateCharacter () {
		if (IngameTime.time > nextShot) {
			nextShot = IngameTime.time + firingDelay;
			if (projectile != null) {
				GameObject pro = (GameObject) Instantiate (projectile, gameObject.transform.position, Quaternion.identity);
				pro.gameObject.GetComponent<Projectile>().isEnemy = _isEnemy;
				//pro.gameObject.GetComponent<CurvedProjectile> ().SetDirection (1, -2);
				pro.gameObject.GetComponent<Projectile> ().SetDirection (0, -1);
				//pro.gameObject.GetComponent<Projectile> ().curveAngle = 10;
			}
		}
	}

	void OnDestroy () {
		m_group.RemoveEnemy ();
	}

	public override string ToString() {
		return "Enemy";
	}

	void OnBecameInvisible () {
		//Debug.Log ("bye");
		Destroy (gameObject);
	}
}
