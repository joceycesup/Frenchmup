using UnityEngine;
using System.Collections;

public class Enemy : Character {
	public bool isStatic = false;

	private EnemyGroup m_group;

	void Awake () {
		_isEnemy = true;
		m_group = transform.parent.gameObject.GetComponent<EnemyGroup> ();
	}

	void OnDestroy () {
		m_group.RemoveEnemy ();
	}

	public override string ToString() {
		return "Enemy";
	}

	void OnBecameInvisible () {
		//Debug.Log ("bye");
		Destroy (gameObject);
	}
}
