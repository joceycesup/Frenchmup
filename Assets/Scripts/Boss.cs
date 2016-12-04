﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Boss : Enemy {
	bool phase2 = false;

	public GameObject Phase1 ;
	public GameObject Phase2 ;

	public Image jauge;

	protected override void StartCharacter () {
		base.StartCharacter ();
		// Mets ce que tu veux ici pour l'initialisation
		if (Phase1 != null)
			Phase1.SetActive (true);
		//---------------------
	}

	protected override void UpdateCharacter () {
		base.UpdateCharacter ();
		if(jauge!=null)
			jauge.fillAmount = health/maxHealth;


		if (Input.GetKeyDown (KeyCode.K)) {
			Debug.Log ("Kill");
			TakeDamage (75, false);
		}

		// Mets ce que tu veux ici
		if (Phase1 != null && !phase2 && health < maxHealth / 2f) {
			phase2 = true;
			// Mise en place de la phase 2
			Debug.Log("Phase2");
			GameObject smartBomb = (GameObject)Instantiate(Resources.Load<GameObject>("Prefabs/SmartBomb"));
			smartBomb.transform.parent = ViewportHandler.viewport.transform;
			smartBomb.transform.position = transform.position;
			smartBomb.SetActive (true);

			Destroy (Phase1);
			Phase2.SetActive (true);

			//---------------------
		}
	}

	void OnDestroy () {
        if (Phase1 != null)
        {
            AkSoundEngine.SetState("current_zone", "post_boss");
        }
		if (m_group != null) {
			m_group.RemoveEnemy ();
		} else {
			Debug.LogWarningFormat("{0} was destroyed but has no group", gameObject);
		}
		if (pattern == PatternType.Path || pattern == PatternType.Bezier) {
			if (patternArgsO.Length > 0 && patternArgsO [0] != null) {
				Destroy (patternArgsO [0]);
			}
		}
        if (health <= 0f)
        {
			Debug.Log("Say hello to my little friend");
			GameObject smartBomb = (GameObject)Instantiate(Resources.Load<GameObject>("Prefabs/SmartBomb"), ViewportHandler.viewport.transform);
            smartBomb.transform.position = transform.position;
			smartBomb.SetActive (true);
			//Debug.Break ();
        }
	}

	public override string ToString() {
		return "Boss";
	}
}
