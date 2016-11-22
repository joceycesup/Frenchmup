using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {
	public float maxSpeed;
	protected float speed;
	public int health;
	public float invincibilityTime = 1.0f;
	private float invincibilityStartTime = float.MaxValue;
	public bool invincible = false;

	protected bool _isEnemy;

	public bool isEnemy {
		get { return _isEnemy; }
	}

	void Update () {
		if (invincible) {
			if (IngameTime.time >= invincibilityStartTime + invincibilityTime) {
				invincible = false;
				gameObject.GetComponent<SpriteRenderer> ().color = new Color (1, 1, 1);
			} else {
				float f = Mathf.Sin ((IngameTime.time - invincibilityStartTime) * 10.0f) / 2.0f + 0.5f;
				gameObject.GetComponent<SpriteRenderer> ().color = new Color (1, f, f);
			}
		}
		UpdateCharacter ();
	}

	protected virtual void UpdateCharacter () {
	}

	protected void SetCanShoot (bool b) {
		if (gameObject.GetComponent<ProjectileEmitter> () != null)
			gameObject.GetComponent<ProjectileEmitter> ().enabled = b;
	}
	/*
	void OnCollisionEnter2D (Collision2D other) {
		if (other.gameObject.tag == "CharHitbox")
			return;
	}//*/

	public bool TakeDamage (int value) {
		if (invincible)
			return false;
		invincibilityStartTime = IngameTime.time;
		invincible = true;
		if ((health -= value) <= 0) {
			Destroy (gameObject);
		}
		return true;
	}
}
