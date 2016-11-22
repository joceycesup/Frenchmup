﻿using UnityEngine;
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
	public float firingRate = 1;
	public GameObject projectile;

	private GameObject magnet;

	void Awake () {
		//gameObject.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Sprites/Rocket"+playerNumber);
		_isEnemy = false;
		speed = maxSpeed;
	}

	protected override void UpdateCharacter () {
		float dx = Input.GetAxis ("Horizontal_P" + playerNumber);
		float dy = Input.GetAxis ("Vertical_P" + playerNumber);

		if (dx != 0 || dy != 0) {
			Vector3 deltaPos = Vector3.Normalize (new Vector3 (Mathf.Abs (dx), Mathf.Abs (dy), 0));

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

			gameObject.transform.Translate (deltaPos * IngameTime.deltaTime * speed);
		} else {
			gameObject.GetComponent<Animator> ().Play ("idle");
		}

		if (Input.GetButton ("Fire1_P"+playerNumber)) {
			SetCanShoot (true);
		}
		if (Input.GetButtonUp ("Fire1_P"+playerNumber)) {
			SetCanShoot (false);
		}
		if (Input.GetButtonDown ("Fire2_P"+playerNumber)) {
			if (magnet == null) {
				magnet = (GameObject)Instantiate (Resources.Load<GameObject> ("Prefabs/Magnet"));
				magnet.transform.parent = transform;
				magnet.transform.position = transform.position;
			}
		}
		if (Input.GetButtonUp ("Fire2_P"+playerNumber)) {
			if (magnet != null) {
				Destroy (magnet);
			}
		}
		if (Input.GetButtonDown ("Fire3_P"+playerNumber)) {
			GameObject smartBomb = (GameObject)Instantiate (Resources.Load<GameObject> ("Prefabs/SmartBomb"));
			smartBomb.transform.parent = transform;
			smartBomb.transform.position = transform.position;
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
