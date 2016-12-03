using UnityEngine;
using System.Collections;

public class CharHitbox : MonoBehaviour {
	private Character character;

	void Awake () {
		character = transform.parent.gameObject.GetComponent<Character> ();
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject == transform.parent.gameObject)
			return;
		if (!character.isActiveAndEnabled)
			return;
		if (other.gameObject.tag == "Projectile" && other.gameObject.GetComponent<Projectile> () != null && other.gameObject.GetComponent<Projectile> ().isEnemy != character.isEnemy) {
			if (character.gameObject.GetComponent<Player> () != null) {
				character.gameObject.GetComponent<Player> ().EatBullet (other.transform.position);
			}
			character.TakeDamage (1f, !character.isEnemy);
			other.gameObject.GetComponent<Projectile> ().Remove ();
		}
	}

	void OnTriggerStay2D (Collider2D other) {
		if (other.gameObject == transform.parent.gameObject)
			return;
		if (!character.isActiveAndEnabled)
			return;
		//Debug.Log (other.gameObject);
		if (other.gameObject.tag == "Projectile" && character.isEnemy && other.transform.parent != null && other.transform.parent.gameObject.GetComponent<Chain> () != null) {
			character.TakeDamage (other.transform.parent.gameObject.GetComponent<Chain> ().damage * IngameTime.deltaTime, false);
		}
	}
}
