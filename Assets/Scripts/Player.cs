using UnityEngine;
using System.Collections;

public class Player : Character {

	public enum PlayerState {
		DPS,
		Neutral,
		Support
	}

	private static Vector3 topRight = Vector3.Normalize (new Vector3 (1, 1, 0));

	public int playerNumber;
	public PlayerState state = PlayerState.Neutral;

	public float bulletTimeDuration = 2f;
	public float bulletTimeFactor = 0.5f;
	public float bulletTimeCooldown = 1f;
	private bool bulletTime = false;
	private float bulletTimeEndTime;

	public float dashDuration = 0.5f;
	public float dashSpeedFactor = 4f;
	public float dashCooldown = 0.5f;
	private bool dash = false;
	private float dashEndTime;
	private Vector3 dashVector;

	private GameObject magnet;
	private GameObject smartBomb;
	private GameObject laser;



	protected override void AwakeCharacter () {
		//gameObject.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Sprites/Rocket"+playerNumber);
		_isEnemy = false;
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
			if (IngameTime.time >= bulletTimeEndTime) {
				bulletTime = false;
				speed /= dashSpeedFactor;
			}
		}
		if (dash) {
			if (IngameTime.time >= dashEndTime) {
				dash = false;
				IngameTime.MultiplyFactor (1f / dashSpeedFactor);
			}
		}

		if (Input.GetButtonDown ("Dash_P"+playerNumber)) {
			if (!dash) {
				if (IngameTime.time > dashEndTime + dashCooldown) {
					dash = true;
					dashEndTime = IngameTime.time + dashDuration;
					speed *= dashSpeedFactor;
					dashVector = deltaPos;
				}
			}
		}
		switch (state) {
		case PlayerState.Neutral:
			gameObject.GetComponent<SpriteRenderer> ().color = Color.white;
			break;
		case PlayerState.DPS:
			if (Input.GetButton ("Fire1_P"+playerNumber)) {
				SetCanShoot (true);
			}
			if (Input.GetButtonUp ("Fire1_P"+playerNumber)) {
				SetCanShoot (false);
			}
			if (Input.GetButtonDown ("Fire2_P"+playerNumber)) {
				if (laser == null) {
					laser = (GameObject)Instantiate (Resources.Load<GameObject> ("Prefabs/Laser"));
					laser.transform.parent = transform;
					laser.transform.position = transform.position;
				}
				laser.SetActive (true);
				laser.GetComponent<Laser> ().Shoot ();
			}
			if (Input.GetButtonUp ("Fire2_P"+playerNumber)) {
				laser.GetComponent<Laser> ().Stop ();
				laser.SetActive (false);
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
					if (IngameTime.time > bulletTimeEndTime + bulletTimeCooldown) {
						bulletTime = true;
						bulletTimeEndTime = IngameTime.time + bulletTimeDuration;
						IngameTime.MultiplyFactor (bulletTimeFactor);
					}
				}
			}
			break;
		}
		if (Input.GetButtonDown ("DPS_P" + playerNumber)) {
			ChangeState (PlayerState.DPS);
		} else if (Input.GetButtonDown ("Support_P" + playerNumber)) {
			ChangeState (PlayerState.Support);
		}
	}

	private void ChangeState (PlayerState newState) {
		if (state == newState) {
			state = PlayerState.Neutral;
		} else {
			state = newState;
		}
		switch (state) {
		case PlayerState.Neutral:
			gameObject.GetComponent<SpriteRenderer> ().color = Color.white;
			break;
		case PlayerState.DPS:
			gameObject.GetComponent<SpriteRenderer> ().color = Color.magenta;
			break;
		case PlayerState.Support:
			gameObject.GetComponent<SpriteRenderer> ().color = Color.cyan;
			break;
		}
	}

	public override string ToString() {
		return "Player_" + playerNumber;
	}
}
