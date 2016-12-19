using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {
	protected float speed;
	public float maxHealth;
	public float health
    {
        get;
        protected set;
	}
	public float damageTakenOnCollision = 0f;
	public float invincibilityTime = float.MaxValue;
	private float invincibilityStartTime = 0f;
	public bool invincible = false;

	public GameObject blood;

	protected bool _isEnemy;

	public bool isEnemy {
		get { return _isEnemy; }
	}

	void Awake () {
		health = maxHealth;
		if (invincible) {
			invincibilityTime = float.MaxValue;
		}
		AwakeCharacter ();
	}

	protected virtual void AwakeCharacter () {
	}

	void Start () {
		StartCharacter ();
	}

	protected virtual void StartCharacter () {
	}

	void Update () {
		if (IngameTime.pause)
			return;
		if (invincible) {
			if (IngameTime.time >= invincibilityStartTime + invincibilityTime) {
				invincible = false;//*
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
	//*
	void OnCollisionEnter2D (Collision2D other) {
		if (IngameTime.pause)
			return;
		if (damageTakenOnCollision > 0f) {
			if (other.gameObject.GetComponent<Character> () != null && other.gameObject.GetComponent<Character> ().isEnemy != _isEnemy) {
				TakeDamage (damageTakenOnCollision);
			}
		}
	}//*/

	protected virtual void Death () {
		Destroy (gameObject);
	}

	public virtual bool TakeDamage (float value, bool activeInvincibility = true) {
		if (invincible)
			return false;
		if (activeInvincibility) {
			SetInvincible ();
		}
		//Debug.Log ("Ouch");
		if (blood != null) {
			//if (GetComponent<Boss> () == null) {
				Instantiate (blood, transform.position - Vector3.forward, Quaternion.identity);
		//	}
		}
		if ((health -= value) <= 0) {
			health = 0;
			Death ();
		}
		return true;
	}

	protected void SetInvincible () {
		invincibilityStartTime = IngameTime.time;
		invincible = true;
	}
}
