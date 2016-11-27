using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour {
	public GameObject[] parts;
	public float beamSpeed = 1f;
	public float dps = 2f;
	private float beamHeight;
	private BoxCollider2D b2d;

	void Awake () {
		b2d = GetComponent<BoxCollider2D> ();
		beamHeight = parts [1].GetComponent<SpriteRenderer> ().bounds.size.y;
	}

	void Update () {//*
		float boxHeight = ViewportHandler.viewport.transform.position.y + ViewportHandler.viewport.GetComponent<BoxCollider2D> ().bounds.extents.y - transform.position.y;
		parts [0].GetComponent<SpriteRenderer> ().enabled = (boxHeight != (boxHeight = Mathf.Min (b2d.size.y+beamSpeed*IngameTime.deltaTime, boxHeight)));
		SetHeight (boxHeight);
		RaycastHit2D[] results = new RaycastHit2D[10];
		float minEnemyY = float.MaxValue;
		Collider2D closestEnemy = null;
		int n = b2d.Cast(Vector2.up, results, 0f);
		if (n > 0) {
			for (int i = 0; i < n; ++i) {
				if (results [i].collider.gameObject.tag == "Enemy") {
					if (results [i].transform.position.y < minEnemyY) {
						minEnemyY = results [i].transform.position.y;
						closestEnemy = results [i].collider;
					}
				}	
			}
		}
		if (closestEnemy != null) {
			parts [0].GetComponent<SpriteRenderer> ().enabled = true;
			parts [0].transform.localPosition = new Vector3 (0f, closestEnemy.transform.position.y - transform.position.y);
			parts [1].transform.localScale = new Vector3 (1f, (parts [0].transform.localPosition.y - parts [0].GetComponent<SpriteRenderer> ().bounds.extents.y - parts [1].transform.localPosition.y) / beamHeight);
			SetHeight (closestEnemy.transform.position.y - transform.position.y);
			closestEnemy.transform.parent.gameObject.GetComponent<Character> ().TakeDamage (dps*IngameTime.deltaTime, false);
		} else {
			if (parts [0].GetComponent<SpriteRenderer> ().enabled) {
				parts [0].transform.localPosition = new Vector3 (0f, boxHeight);
				parts [1].transform.localScale = new Vector3 (1f, (parts [0].transform.localPosition.y - parts [0].GetComponent<SpriteRenderer> ().bounds.extents.y - parts [1].transform.localPosition.y) / beamHeight);
			} else {
				parts [1].transform.localScale = new Vector3 (1f, (boxHeight - parts [1].transform.localPosition.y) / beamHeight);
			}
		}//*/
	}

	void SetHeight (float height) {
		b2d.offset = new Vector2 (0f, height / 2f);
		Vector3 tmpSize = b2d.size;
		tmpSize.y = height;
		b2d.size = tmpSize;
	}

	public void Shoot () {
		float height = parts [0].GetComponent<SpriteRenderer> ().bounds.extents.y + parts [2].GetComponent<SpriteRenderer> ().bounds.extents.y + parts [2].transform.localPosition.y;
		SetHeight (height);
		parts [0].transform.localPosition = new Vector3 (0f, height);
		parts [1].transform.localScale = Vector3.zero;
		parts [0].GetComponent<SpriteRenderer> ().enabled = true;
		parts [1].GetComponent<SpriteRenderer> ().enabled = true;
		parts [2].GetComponent<SpriteRenderer> ().enabled = true;
	}

	public void Stop () {
		parts [0].GetComponent<SpriteRenderer> ().enabled = false;
		parts [1].GetComponent<SpriteRenderer> ().enabled = false;
		parts [2].GetComponent<SpriteRenderer> ().enabled = false;
		gameObject.SetActive (false);
	}
}
