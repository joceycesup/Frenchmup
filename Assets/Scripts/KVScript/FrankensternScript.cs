using UnityEngine;
using System.Collections;

public class FrankensternScript : MonoBehaviour {

	void OnTriggerEnter2D(Collider2D other){
		if (other.tag == "Projectile") {
			if (!other.GetComponent<Projectile> ().isEnemy) {
				Destroy (other.gameObject);
				Instantiate (Resources.Load ("Particules/Cancel"), transform.position-Vector3.forward, Quaternion.identity, ViewportHandler.viewport.transform);
			}
		}
	}
}
