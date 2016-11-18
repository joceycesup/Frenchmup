using UnityEngine;
using System.Collections;

public class Player : Character {
	private static Vector3 topRight = Vector3.Normalize (new Vector3 (1, 1, 0));

	public int playerNumber;
	public float firingRate = 1;
	public GameObject projectile;

	private float firingDelay;
	private float nextShot = 0;

	void Awake () {
		//gameObject.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Sprites/Rocket"+playerNumber);
		firingDelay = 1.0f / firingRate;
		_isEnemy = false;
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

		if (Input.GetAxis ("Fire1_P"+playerNumber) > .5f) {
			if (IngameTime.time > nextShot) {
				nextShot = IngameTime.time + firingDelay;
				GameObject pro = (GameObject) Instantiate (projectile, gameObject.transform.position, Quaternion.identity);
				pro.gameObject.GetComponent<Projectile>().isEnemy = _isEnemy;
			}
		}
		if (Input.GetButtonDown("Fire2_P"+playerNumber)) {
			//NotSoSmartBomb
			if (IngameTime.time > nextShot) {
				nextShot = IngameTime.time + firingDelay;
				GameObject pro = (GameObject) Instantiate (projectile, gameObject.transform.position, Quaternion.identity);
				pro.gameObject.GetComponent<Projectile>().isEnemy = _isEnemy;
			}
		}
	}

	public override string ToString() {
		return "Player_" + playerNumber;
	}
}
