using UnityEngine;
using System.Collections;

public class CharHitbox : MonoBehaviour {
	private Character character;

	void Start () {
		character = transform.parent.gameObject.GetComponent<Character> ();
		//Debug.Log (transform.parent.gameObject.ToString () + " collider : " + gameObject.GetComponent<Collider2D> ());
		//Debug.Log ("parent is " + transform.parent.gameObject.ToString ());
	}

	void OnTriggerEnter2D (Collider2D other) {
		//Debug.Log (gameObject + " : " + other.gameObject);
		if (other.gameObject == transform.parent.gameObject)
			return;
		//Debug.Log (transform.parent.gameObject.ToString () + " got triggered by " + other.gameObject.ToString ());
		if (other.gameObject.tag == "Projectile" && other.gameObject.GetComponent<Projectile> ().isEnemy != character.isEnemy) {
			if (character.TakeDamage (1)) {
				other.gameObject.GetComponent<Projectile> ().Remove ();
			}
		}
	}
}
