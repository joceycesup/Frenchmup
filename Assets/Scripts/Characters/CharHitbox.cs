using UnityEngine;
using System.Collections;

public class CharHitbox : MonoBehaviour {
	private Character character;

	void Awake () {
		character = transform.parent.gameObject.GetComponent<Character> ();
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (IngameTime.pause)
			return;
		if (gameObject.name == "TestHitbox")
			Debug.Log ("enter " + other.gameObject);
		if (other.gameObject == transform.parent.gameObject)
			return;
		if (!character.isActiveAndEnabled)
			return;
		if (other.gameObject.tag == "Projectile" && other.gameObject.GetComponent<Projectile> () != null && other.gameObject.GetComponent<Projectile> ().isEnemy != character.isEnemy) {
			if (character.gameObject.GetComponent<Player> () != null) {
				character.gameObject.GetComponent<Player> ().EatBullet (other.transform.position);
			}
			character.TakeDamage (1f, !character.isEnemy);
			if (character.GetComponent<Boss>()!=null) {
				Instantiate (Resources.Load ("Particules/MiniBlood"), other.transform.position - Vector3.forward, Quaternion.identity, ViewportHandler.viewport.transform);
			}
			other.gameObject.GetComponent<Projectile> ().Remove ();
		}
	}

	void OnTriggerStay2D (Collider2D other) {
		if (IngameTime.pause)
			return;
		if (other.gameObject == transform.parent.gameObject)
			return;
		if (!character.isActiveAndEnabled)
			return;
		if (other.gameObject.tag == "Projectile" && character.isEnemy && other.transform.parent != null && other.transform.parent.gameObject.name == "Links") {
			character.TakeDamage (other.transform.parent.parent.FindChild("Path").gameObject.GetComponent<Chain> ().damage * IngameTime.deltaTime, false);
		}
	}
}
