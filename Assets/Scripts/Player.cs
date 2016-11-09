using UnityEngine;
using System.Collections;

public class Player : Character {
	private static Vector3 topRight = Vector3.Normalize (new Vector3 (1, 1, 0));

	public int playerNumber;
	public float firingRate = 0;
	public GameObject projectile;

	private float firingDelay;
	private float nextShot = 0;

	void Start () {
		gameObject.GetComponent<SpriteRenderer> ().sprite = Resources.Load<Sprite> ("Sprites/Rocket"+playerNumber);
		firingDelay = 1.0f / firingRate;
		isEnemy = false;
	}

	void Update () {
		float dx = Input.GetAxis ("Horizontal_P" + playerNumber);
		float dy = Input.GetAxis ("Vertical_P" + playerNumber);

		if (dx != 0 || dy != 0) {
			Vector3 deltaPos = Vector3.Normalize(new Vector3(Mathf.Abs(dx), Mathf.Abs(dy), 0));

			if (Vector3.Dot (Vector3.up, deltaPos) > Vector3.Dot (topRight, deltaPos)) {
				deltaPos = new Vector3 (0, Mathf.Sign (dy), 0);
			} else if (Vector3.Dot (Vector3.right, deltaPos) > Vector3.Dot (topRight, deltaPos)) {
				deltaPos = new Vector3 (Mathf.Sign (dx), 0, 0);
			} else {
				deltaPos = Vector3.Normalize (new Vector3 (Mathf.Sign (dx), Mathf.Sign (dy), 0));
			}

			gameObject.transform.position += (deltaPos * Time.deltaTime * speed);
		}

		if (Input.GetAxis ("Fire1_P"+playerNumber) > .5f) {
			if (Time.time > nextShot) {
				nextShot = Time.time + firingDelay;
				GameObject pro = (GameObject) Instantiate (projectile, gameObject.transform.position, Quaternion.identity);
				pro.gameObject.GetComponent<Projectile>().isEnemy = isEnemy;
			}
		}
	}

	public string ToString() {
		return "Player_" + playerNumber;
	}
	/*
	void OnCollisionEnter2D (Collision2D coll) {
		Debug.Log ("Hit");
		if (coll.gameObject.tag == "Projectile")
			Debug.Log ("Damage");
	}//*/
}
