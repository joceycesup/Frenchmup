using UnityEngine;
using System.Collections;

public class Magnet : MonoBehaviour {
	public float magnetSpeed;

	void OnTriggerStay2D (Collider2D other) {
		//Debug.Log (gameObject + " : " + other.gameObject);
		if (other.gameObject == transform.parent.gameObject)
			return;
		//Debug.Log (transform.parent.gameObject.ToString () + " got triggered by " + other.gameObject.ToString ());
		if (other.gameObject.tag == "Projectile") {
			float factor = 1.0f - Vector3.Distance (gameObject.transform.position, other.gameObject.transform.position) / gameObject.GetComponent<CircleCollider2D> ().radius;
			factor *= magnetSpeed * IngameTime.deltaTime;
			other.gameObject.transform.Translate (Vector3.Normalize (other.gameObject.transform.position - gameObject.transform.position) * factor);
		}
	}
}
