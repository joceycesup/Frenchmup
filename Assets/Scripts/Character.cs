using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour {
	public float speed;
	public int health;

	protected bool isEnemy;
	
	// Update is called once per frame
	void Update () {
	}

	void OnTriggerEnter2D (Collider2D other) {
		if (other.gameObject.tag == "Projectile" && other.gameObject.GetComponent<Projectile> ().isEnemy != isEnemy) {
			Debug.Log (this.ToString() + " took damage");
			Destroy (other.gameObject);
			TakeDamage ();
		}
	}

	void TakeDamage() {
		if (--health <= 0) {
			Destroy (gameObject);
		}
	}
}
