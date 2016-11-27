using UnityEngine;
using System.Collections;

public class Player : Character {
	private static int alivePlayer = 0;

	public enum PlayerState {
		DPS,
		Support
	}

	private static Vector3 topRight = Vector3.Normalize (new Vector3 (1, 1, 0));

	public int playerNumber;
	public PlayerState state = PlayerState.DPS;
	public float switchStateCooldown = 0f;
	private float nextSwitchStateTime = 0f;

	public float respawnDelay = 10f;

	public float bulletTimeDuration = 2f;
	public float bulletTimeFactor = 0.5f;
	public float bulletTimeCooldown = 1f;
	private bool bulletTime = false;
	private float bulletTimeEndTime;

	public float dashDuration = 0.5f;
	public float dashSpeed = 16f;
	public float dashCooldown = 0.5f;
	private bool dash = false;
	private float dashEndTime;
	private Vector3 dashVector;

	public float supportDamageReduction = 0.25f;
	public float supportSpeed = 8f;
	public float dpsSpeed = 4f;

	public float maxLaserLoad = 30f;
	private float laserLoad = 0f;
	public float laserSpeed = 2f;
	public float laserUnloadTime = 5f;

	private GameObject magnet;
	private GameObject smartBomb;
	private GameObject laser;

	protected override void AwakeCharacter () {
		//gameObject.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Sprites/Rocket"+playerNumber);
		_isEnemy = false;
	}

	void Start () {
		alivePlayer++;
		if (!GameSettings.tutorial) {
			laserLoad = maxLaserLoad;
		}
	}

	private void Reset () {
		maxHealth = maxHealth;
		laserLoad = maxLaserLoad;
	}

	protected override void UpdateCharacter () {
		float dx = Input.GetAxis ("Horizontal_P" + playerNumber);
		float dy = Input.GetAxis ("Vertical_P" + playerNumber);

		Vector3 deltaPos = Vector3.zero;
		if (dx != 0 || dy != 0) {
			if (!dash) {
				deltaPos = Vector3.Normalize (new Vector3 (Mathf.Abs (dx), Mathf.Abs (dy), 0));

				if (Vector3.Dot (Vector3.up, deltaPos) > Vector3.Dot (topRight, deltaPos)) {
					deltaPos = new Vector3 (0, Mathf.Sign (dy), 0);
					gameObject.GetComponent<Animator> ().Play (Mathf.Sign (dy) < 0 ? "down" : "up");
				} else if (Vector3.Dot (Vector3.right, deltaPos) > Vector3.Dot (topRight, deltaPos)) {
					deltaPos = new Vector3 (Mathf.Sign (dx), 0, 0);
					gameObject.GetComponent<Animator> ().Play (Mathf.Sign (dx) < 0 ? "left" : "right");
				} else {
					deltaPos = Vector3.Normalize (new Vector3 (Mathf.Sign (dx), Mathf.Sign (dy), 0));
					gameObject.GetComponent<Animator> ().Play ((Mathf.Sign (dy) < 0 ? "down_" : "up_") + (Mathf.Sign (dx) < 0 ? "left" : "right"));
				}
			} else {
				deltaPos = dashVector;
			}

			gameObject.transform.Translate (deltaPos * IngameTime.deltaTime * speed);
		} else {
			gameObject.GetComponent<Animator> ().Play ("idle");
		}

		if (bulletTime) {
			if (IngameTime.globalTime >= bulletTimeEndTime) {
				bulletTime = false;
				IngameTime.MultiplyFactor (1f / bulletTimeFactor);
			}
		}
		if (dash) {
			speed = dashSpeed;
			if (IngameTime.time >= dashEndTime) {
				dash = false;
				speed = state == PlayerState.DPS ? dpsSpeed : supportSpeed;
			}
		}
		if (laser != null && laser.active) {
			speed = laserSpeed;
			if ((laserLoad -= maxLaserLoad * IngameTime.deltaTime / laserUnloadTime) <= 0f) {
				laserLoad = 0f;
				laser.GetComponent<Laser> ().Stop ();
				speed = state == PlayerState.DPS ? dpsSpeed : supportSpeed;
			}
		}
		switch (state) {
		case PlayerState.DPS:
			if (Input.GetButton ("Fire1_P"+playerNumber)) {
				SetCanShoot (true);
			}
			if (Input.GetButtonUp ("Fire1_P"+playerNumber)) {
				SetCanShoot (false);
			}
			if (Input.GetButtonDown ("Fire2_P"+playerNumber)) {
				if (laserLoad > 0f) {
					if (laser == null) {
						laser = (GameObject)Instantiate (Resources.Load<GameObject> ("Prefabs/Laser"));
						laser.transform.parent = transform;
						laser.transform.position = transform.position;
					}
					laser.SetActive (true);
					laser.GetComponent<Laser> ().Shoot ();
				}
			}
			if (Input.GetButtonUp ("Fire2_P"+playerNumber)) {
				if (laser != null) {
					laser.GetComponent<Laser> ().Stop ();
				}
				speed = dpsSpeed;
				//laser.SetActive (false);
			}
			if (Input.GetButtonDown ("Fire3_P"+playerNumber)) {
				if (smartBomb == null) {
					smartBomb = (GameObject)Instantiate (Resources.Load<GameObject> ("Prefabs/SmartBomb"));
					smartBomb.transform.parent = transform;
					smartBomb.transform.position = transform.position;
				}
				smartBomb.SetActive (true);
			}
			break;
		case PlayerState.Support:
			if (Input.GetButtonDown ("Fire1_P"+playerNumber)) {
				if (!dash) {
					if (IngameTime.time > dashEndTime + dashCooldown) {
						dash = true;
						dashEndTime = IngameTime.time + dashDuration;
						speed = dashSpeed;
						dashVector = deltaPos;
					}
				}
			}
			if (Input.GetButtonDown ("Fire2_P"+playerNumber)) {
				if (magnet == null) {
					magnet = (GameObject)Instantiate (Resources.Load<GameObject> ("Prefabs/Magnet"));
					magnet.transform.parent = transform;
					magnet.transform.position = transform.position;
				}
				magnet.SetActive (true);
			}
			if (Input.GetButtonUp ("Fire2_P"+playerNumber)) {
				magnet.SetActive (false);
			}
			if (Input.GetButtonDown ("Fire3_P"+playerNumber)) {
				if (!bulletTime) {
					if (Time.time > bulletTimeEndTime + bulletTimeCooldown) {
						bulletTime = true;
						bulletTimeEndTime = IngameTime.globalTime + bulletTimeDuration;
						IngameTime.MultiplyFactor (bulletTimeFactor);
					}
				}
			}
			break;
		}
		if (Input.GetButtonDown ("DPS_P" + playerNumber)) {
			if (IngameTime.globalTime > nextSwitchStateTime) {
				speed = dpsSpeed;
				state = PlayerState.DPS;
				gameObject.GetComponent<SpriteRenderer> ().color = Color.magenta;
				nextSwitchStateTime = IngameTime.globalTime + switchStateCooldown;
			}
		} else if (Input.GetButtonDown ("Support_P" + playerNumber)) {
			if (IngameTime.globalTime > nextSwitchStateTime) {
				if (!dash)
					speed = supportSpeed;
				if (laser != null)
					laser.GetComponent<Laser> ().Stop ();
				state = PlayerState.Support;
				gameObject.GetComponent<SpriteRenderer> ().color = Color.cyan;
				nextSwitchStateTime = IngameTime.globalTime + switchStateCooldown;
			}
		}
	}

	public void EatBullet () {
		if (state == PlayerState.Support) {
			if (++laserLoad > maxLaserLoad)
				laserLoad = maxLaserLoad;
		}
	}

	protected override void Death () {
		if (--alivePlayer <= 0) {
		} else {
			StartCoroutine ("RespawnPlayer", this);
		}
		gameObject.SetActive (false);
	}

	IEnumerator RespawnPlayer (Player player) {
		float respawnTime = IngameTime.globalTime + player.respawnDelay;
		while (IngameTime.globalTime < respawnTime) {
			if (alivePlayer <= 0) {
				respawnTime = 0f;
			} else {
				yield return null;
			}
		}
		if (alivePlayer <= 0) {
			Destroy (player.gameObject);
		} else {
			player.gameObject.SetActive (true);
			alivePlayer++;
		}
	}

	public override bool TakeDamage (float value, bool activeInvincibility = true) {
		return dash ? false : base.TakeDamage (value * (state == PlayerState.Support ? supportDamageReduction : 1f), activeInvincibility);
	}

	public override string ToString() {
		return "Player_" + playerNumber;
	}

	void OnGUI () {
		GUIStyle style = new GUIStyle ();
		style.normal.textColor = Color.white;
		style.fontSize = 30;
		GUI.Label (new Rect (10, 10, 200, 30), laserLoad + "/" + maxLaserLoad, style);
	}
}
