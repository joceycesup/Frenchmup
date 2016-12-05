using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Spine.Unity;

public class Boss : Enemy {
	bool phase2 = false;

	public GameObject Phase1 ;
	public GameObject Phase2 ;

	public Image jauge;

	SkeletonAnimation skel;
	public SkeletonAnimation trompe2;

	[SpineAttachment (currentSkinOnly: true, slotField: "Paupiere")]
	public string eyesOpen;

	[SpineAttachment (currentSkinOnly: true, slotField: "Paupiere")]
	public string eyesClosed;


	protected override void StartCharacter () {
		base.StartCharacter ();
		skel = GetComponent<SkeletonAnimation> ();
		if (skel != null) {
			skel.skeleton.SetAttachment ("Paupiere", eyesClosed);

			skel.AnimationState.TimeScale = 0.75f;
			//On met une animation sur un track, 0 prenant la priorité sur toutes les autres animations, le nom de l'animation puis si ça loop ou pas
			skel.AnimationState.SetAnimation (1, "FlapAiles", true);
			skel.AnimationState.SetAnimation (0, "Testing", true);
		}
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
			AkSoundEngine.PostEvent ("boss_open_eyes", GameSettings.game_settings);

			skel.skeleton.SetAttachment ("Paupiere", eyesOpen);

			trompe2.gameObject.SetActive (true);
			skel.AnimationState.SetAnimation (0, "Testing", true);
			trompe2.AnimationState.SetAnimation (0, "Starting", false);
			trompe2.timeScale = 0.75f;
			trompe2.AnimationState.AddAnimation (0, "Testing", true, 0);

			GameObject smartBomb = (GameObject)Instantiate(Resources.Load<GameObject>("Prefabs/SmartBombEnemy"));
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
			GameObject smartBomb = (GameObject)Instantiate(Resources.Load<GameObject>("Prefabs/SmartBombEnemy"), ViewportHandler.viewport.transform);
            smartBomb.transform.position = transform.position;
			smartBomb.SetActive (true);
			//Debug.Break ();
        }
	}

	public override string ToString() {
		return "Boss";
	}
}
