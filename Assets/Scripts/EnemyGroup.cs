using UnityEngine;
using System.Collections;

public class EnemyGroup : MonoBehaviour {
	public int enemyCount = 0;

	void Awake () {
		Destroy (gameObject.GetComponent<SpriteRenderer> ());
		for (int i = 0; i < transform.childCount; ++i) {
			if (transform.GetChild (i).gameObject.GetComponent<Enemy> () != null) {
				++enemyCount;
			}
		}
	}

	public void Unleash () {
		Destroy (gameObject.GetComponent<BoxCollider2D> ());
		for (int i = 0; i < transform.childCount;) {
			Enemy enemy = transform.GetChild (i).gameObject.GetComponent<Enemy> ();
			if (enemy == null) {
				++i;
			} else {
				if (enemy.pattern == Enemy.PatternType.StaticOnSection) {
					transform.GetChild (i).parent = ViewportHandler.viewport.GetComponent<ViewportHandler> ().currentSection.transform;
				} else {
					transform.GetChild (i).parent = ViewportHandler.viewport.transform;
				}
				Debug.Log ("unleash " + enemy.gameObject);
				enemy.enabled = true;
			}
		}
		transform.parent = null;
	}

	public void RemoveEnemy () {
		if (--enemyCount <= 0) {
			Destroy (gameObject);
		}
	}

	void OnDestroy () {
		enemyCount += 1337; // prevents this object from getting destroyed multiple times because of RemoveEnemy
	}

	void OnDrawGizmos () {
		if (gameObject.GetComponent<BoxCollider2D> () != null) {
			Gizmos.color = Color.red;
			Gizmos.DrawWireCube(transform.position - new Vector3 (0f, gameObject.GetComponent<BoxCollider2D>().bounds.extents.y+9f),
				new Vector3(32f, 18f));
		}
	}
}
