using UnityEngine;
using System.Collections;

public class CharHitbox : MonoBehaviour {
	private Character character;

	void Awake () {
		character = transform.parent.gameObject.GetComponent<Character> ();
	}

	void OnTriggerEnter2D (Collider2D other) {
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
		if (gameObject.name == "TestHitbox") {
			Debug.Log ("stay  " + other.gameObject + " ; " + (other.gameObject.tag == "Projectile") +" ; " +(character.isEnemy) +" ; " + (other.transform.parent.gameObject.name));
		}
		if (other.gameObject == transform.parent.gameObject)
			return;
		if (!character.isActiveAndEnabled)
			return;
		//Debug.Log (other.gameObject);
		if (other.gameObject.tag == "Projectile" && character.isEnemy && other.transform.parent != null && other.transform.parent.gameObject.name == "Links") {
		//if (other.gameObject.tag == "Projectile" && character.isEnemy && other.transform.parent != null && other.transform.parent.gameObject.GetComponent<Chain> () != null) {
				Debug.Log ("ELLE MARCHE CETTE FOUTUE CHAINE");
			character.TakeDamage (other.transform.parent.parent.FindChild("Path").gameObject.GetComponent<Chain> ().damage * IngameTime.deltaTime, false);
		}
	}
}
