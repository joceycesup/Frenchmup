using UnityEngine;
using System.Collections;

public class CharHitbox : MonoBehaviour {
	private Character character;

	void Start () {
		character = transform.parent.gameObject.GetComponent<Character> ();
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject == transform.parent.gameObject)
			return;
		if (other.gameObject.tag == "Projectile" && other.gameObject.GetComponent<Projectile> ().isEnemy != character.isEnemy) {
			if (character.gameObject.GetComponent<Player> () != null) {
				character.gameObject.GetComponent<Player> ().EatBullet ();
			}
			character.TakeDamage (1f, !character.isEnemy);
			other.gameObject.GetComponent<Projectile> ().Remove ();
		}
	}
}
