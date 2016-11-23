using UnityEngine;
using System.Collections;

public class Laser : MonoBehaviour {
	public GameObject[] parts;
	private BoxCollider2D b2d;

	void Awake () {
		b2d = GetComponent<BoxCollider2D> ();
	}
	
	// Update is called once per frame
	void Update () {//*
		Bounds bounds = b2d.bounds;
		float boxHeight = ViewportHandler.viewport.transform.position.y + ViewportHandler.viewport.GetComponent<BoxCollider2D> ().bounds.extents.y - transform.position.y;
		b2d.offset = new Vector2 (0f, boxHeight / 2f);
		b2d.size = new Vector3 (bounds.extents.x * 2f, boxHeight);//*/
		RaycastHit2D[] results = new RaycastHit2D[10];
		int n = b2d.Cast(Vector2.up, results, 0f);
		if (n > 0) {
			for (int i = 0; i < n; ++i) {
				if (results[i].collider.gameObject.tag == "Enemy")
					Debug.Log ("hit " + results[i].collider.gameObject.transform.parent.gameObject);
					
			}
		}
	}
}
