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
		if (gameObject.transform.parent.gameObject.GetComponent<Character> () == null) {
			GetComponent<SpriteRenderer> ().enabled = false;
		}
	}

	void OnEnable () {
		startTime = IngameTime.time;
		desintegrateStartTime = startTime + maxLifeSpan - desintegrateTime;
		transform.localScale = new Vector3 ();
		gameObject.GetComponent<SpriteRenderer> ().color = Color.white;
	}

	void Update () {
		float scaleFactor = (IngameTime.time - startTime) / maxLifeSpan;
		/*
		if (gameObject.transform.parent.gameObject.GetComponent<Character> () == null) {
			gameObject.transform.localScale = new Vector3 (scaleFactor*maxLifeSpan, scaleFactor*maxLifeSpan, 0);
		}
		else*/
		gameObject.transform.localScale = new Vector3 (scaleFactor, scaleFactor, 0);
		if (IngameTime.time >= desintegrateStartTime) {
			if (IngameTime.time >= desintegrateStartTime + desintegrateTime) {
				if (gameObject.transform.parent.gameObject.GetComponent<Character> () == null)
					Destroy (gameObject);
				else
					gameObject.SetActive (false);
			} else {
				Color tmpColor = gameObject.GetComponent<SpriteRenderer> ().color;
				tmpColor.a = 1.0f - (IngameTime.time - desintegrateStartTime) / desintegrateTime;
				gameObject.GetComponent<SpriteRenderer> ().color = tmpColor;
			}
		}
	}

	public float GetLoad () {
		return Mathf.Clamp01 ((IngameTime.time - startTime) / maxLifeSpan);
	}

	void OnTriggerEnter2D (Collider2D other) {
		//Debug.Log (gameObject + " : " + other.gameObject);
		if (transform.parent == null)
			return;
		if (other.gameObject == transform.parent.gameObject)
			return;
		//Debug.Log (transform.parent.gameObject.ToString () + " got triggered by " + other.gameObject.ToString ());
		if (other.gameObject.tag == "Projectile") {
			Projectile pro = other.gameObject.GetComponent<Projectile> ();
			if (pro != null && pro.isEnemy) {
				pro.desintegrateTime = 0.1f;
				Instantiate (Resources.Load<GameObject>("Particules/Cancel"),pro.transform.position-Vector3.forward*0.1f,Quaternion.identity,ViewportHandler.viewport.transform);
				pro.Disintegrate ();
			}
		} else if (other.gameObject.tag == "Enemy") {
			if (other.gameObject.GetComponent<Character> () != null)
				other.gameObject.GetComponent<Character> ().TakeDamage (damage);
		}
	}
}
