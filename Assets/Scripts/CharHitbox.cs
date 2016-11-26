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
			if (character.TakeDamage (1)) {
				other.gameObject.GetComponent<Projectile> ().Remove ();
			}
		}
	}
}
