using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {
	public float maxSpeed;
	protected float speed;
	public float health;
	public float invincibilityTime = float.MaxValue;
	private float invincibilityStartTime = 0f;
	public bool invincible = false;

	protected bool _isEnemy;

	public bool isEnemy {
		get { return _isEnemy; }
	}

	void Awake () {
		speed = maxSpeed;
		if (invincible) {
			invincibilityTime = float.MaxValue;
		}
		AwakeCharacter ();
	}

	protected virtual void AwakeCharacter () {
	}

	void Update () {
		if (invincible) {
			if (IngameTime.time >= invincibilityStartTime + invincibilityTime) {
				invincible = false;/*
				gameObject.GetComponent<SpriteRenderer> ().color = new Color (1f, 1f, 1f);
			} else {
				float f = Mathf.Sin ((IngameTime.time - invincibilityStartTime) * 10.0f) / 2.0f + 0.5f;
				gameObject.GetComponent<SpriteRenderer> ().color = new Color (1f, f, f);//*/
			}
		}
		UpdateCharacter ();
	}

	protected virtual void UpdateCharacter () {
	}

	protected void SetCanShoot (bool b) {
		for (int i = 0; i < transform.childCount; ++i) {
			if (transform.GetChild (i).gameObject.GetComponent<ProjectileEmitter> () != null){
				transform.GetChild (i).gameObject.GetComponent<ProjectileEmitter> ().enabled = b;
				transform.GetChild (i).gameObject.GetComponent<ProjectileEmitter> ().isEnemy = _isEnemy;
			}
		}
	}
	/*
	void OnCollisionEnter2D (Collision2D other) {
		if (other.gameObject.tag == "CharHitbox")
			return;
	}//*/

	public virtual bool TakeDamage (float value, bool activeInvincibility = true) {
		if (invincible)
			return false;
		if (activeInvincibility) {
			invincibilityStartTime = IngameTime.time;
			invincible = true;
		}
		if ((health -= value) <= 0) {
			Destroy (gameObject);
		}
		return true;
	}
}
