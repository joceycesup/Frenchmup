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
				enemy.enabled = true;
			}
			if (enemy.pattern == Enemy.PatternType.StaticOnSection) {
				++i;
			} else {
				transform.GetChild (i).parent = ViewportHandler.viewport.transform;
			}
		}
	}

	public void RemoveEnemy () {
		if (--enemyCount <= 0) {
			Destroy (gameObject);
		}
	}

	void OnDestroy () {
		enemyCount += 1337; // prevents this object from getting destroyed multiple times because of RemoveEnemy
	}
}
