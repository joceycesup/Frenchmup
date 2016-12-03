using UnityEngine;
using System.Collections;

public class EnemyEmitter : MonoBehaviour {


	[Header("Meta-Cycle")]
	public int parCycle = 3;
	public float Cooldown = 3f;
	public float StartTime;
	private int compteur = 0;

	[Header("Cycle")]
	public GameObject enemy;
	private float emittingDelay;
	private float nextEnemyTime = 0;
	public float emittingRate = 1;

	void Start () {
		emittingDelay = 1.0f / emittingRate;
		nextEnemyTime = IngameTime.time + StartTime;
	}

	void Update () {
		if (compteur < parCycle) {
			if (IngameTime.time > nextEnemyTime) {
				if (enemy != null) {
					GameObject next = (GameObject)Instantiate (enemy, gameObject.transform.position, Quaternion.identity);
					Enemy nextEnemy = next.gameObject.GetComponent<Enemy> ();
					if (nextEnemy != null) {
						if (nextEnemy.pattern == Enemy.PatternType.StaticOnSection) {
							next.transform.parent = ViewportHandler.viewport.GetComponent<ViewportHandler> ().currentSection.transform;
						} else {
							next.transform.parent = ViewportHandler.viewport.transform;
						}
						nextEnemy.enabled = true;
					}
					nextEnemyTime = IngameTime.time + emittingDelay;
				}
				compteur++;
			}
		} else {
			compteur = 0;
			nextEnemyTime = IngameTime.time + Cooldown;
		}
	}
}
