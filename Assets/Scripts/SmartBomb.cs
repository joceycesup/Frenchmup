using UnityEngine;
using System.Collections;

public class SmartBomb : MonoBehaviour {
	public int damage = 1;
	public float maxLifeSpan = 3.0f;
	public float desintegrateTime = 0.3f;
	private float startTime = 0.0f;
	private float desintegrateStartTime = 0.0f;

	void Awake () {
		gameObject.transform.localScale = new Vector3 (0, 0, 0);
	}

	void Start () {
		startTime = IngameTime.time;
		desintegrateStartTime = startTime + maxLifeSpan - desintegrateTime;
	}

	void Update () {
		float scaleFactor = (IngameTime.time - startTime) / maxLifeSpan;
		gameObject.transform.localScale = new Vector3 (scaleFactor, scaleFactor, 0);
		if (IngameTime.time >= desintegrateStartTime) {
			if (IngameTime.time >= desintegrateStartTime + desintegrateTime) {
				Destroy (gameObject);
			} else {
				Color tmpColor = gameObject.GetComponent<SpriteRenderer> ().color;
				tmpColor.a = 1.0f - (IngameTime.time - desintegrateStartTime) / desintegrateTime;
				gameObject.GetComponent<SpriteRenderer> ().color = tmpColor;
			}
		}
	}

	void OnTriggerEnter2D (Collider2D other) {
		//Debug.Log (gameObject + " : " + other.gameObject);
		if (other.gameObject == transform.parent.gameObject)
			return;
		//Debug.Log (transform.parent.gameObject.ToString () + " got triggered by " + other.gameObject.ToString ());
		if (other.gameObject.tag == "Projectile") {
			Projectile pro = other.gameObject.GetComponent<Projectile> ();
			if (pro.isEnemy) {
				pro.desintegrateTime = 0.1f;
				pro.Disintegrate ();
			}
		} else if (other.gameObject.tag == "Enemy") {
			other.gameObject.GetComponent<Character> ().TakeDamage (damage);
		}
	}
}
