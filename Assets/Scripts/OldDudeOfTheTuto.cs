using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class OldDudeOfTheTuto : MonoBehaviour {

	public enum State {
		S_01_Deplacement = 0,
		S_02_JaugesVie = 1,
		S_03_Roles = 2,
		S_04_BasesDPS = 3,
		S_05_TueMouches = 4,
		S_06_BasesSupport = 5,
		S_07_PassageSupport = 6,
		S_08_UtiliseDash = 7,
		S_09_PresSupport = 8,
		S_10_AbsorbeProjectiles = 9,
		S_11_PresMagnet = 10,
		S_12_UtiliseMagnet = 11,
		S_13_TransitionLaser = 12,
		S_14_JaugesLaser = 13,
		S_15_PresLaser = 14,
		S_16_RemplitLaser = 15,
		S_17_LaserPasseDPS = 16,
		S_18_UtiliseLaser = 17,
		S_19_WarningLaser = 18,
		S_20_Complementaires = 19,
		S_21_J2PasseSupport = 20,
		S_22_UtiliseChaine = 21,
		S_23_JaugesSpecial = 22,
		S_24_PresSpecial = 23,
		S_25_UtiliseSpecial = 24,
		S_26_FinTuto = 25
	}

	public GameObject player1;
	public GameObject player2;
	public GameObject grayZone;
	public State currentState;
	public string[] sentences = {
		"Pour commencer, on va apprendre les bases",
		"Déplacez votre destrier en utilisant le stick directionnel gauche",
		"Ça vous permettra de ne pas tomber au combat tout de suite...",
		"Ça, c’est votre jauge de vie",
		"Pas besoin de vous dire ce qui arrivera si elle se vide...",
		"Si vous voulez gagner cette bataille,",
		"Il est indispensable de se battre,",
		"Mais aussi de se défendre !",
		"Je vais vous apprendre deux rôles : Assaillant et Protecteur.",
		"Pour l’instant, vous êtes en rôle d’Assaillant.",
		"Dans l’idéal, il faut que chacun d’entre vous ait un rôle différent.",
		"Vous êtes libres de choisir",
		"Mais ça vous aiderait à survivre plus de quelques secondes...",
		"En tant qu’Assaillant, vous pouvez tirer des lances avec (A)",
		"Voici quelques ennemis que nos gardes ont réussi à capturer.",
		"Essayez de les anéantir !",
		"Vous avez bien compris comment utiliser les bases du rôle d’Assaillant.",
		"Maintenant, passez au rôle Protecteur avec (L1)",
		"Très bien ! Votre rôle de Protecteur est d’empêcher l’Assaillant de mourir",
		"Son armure est beaucoup moins résistante que la vôtre,",
		"Alors il faut l’aider un peu.",
		"En tant que Protecteur, vous ne pouvez pas tirer,",
		"Mais vous pouvez faire avancer votre destrier plus rapidement \navec (A)",
		"Et en vous déplaçant avec le stick directionnel gauche",
		"En chargeant sur les projectiles ennemis",
		"Vous pouvez les absorber",
		"Vous commencez à comprendre l’importance du Protecteur, c’est bien !",
		"L’armure du Protecteur a une particularité,",
		"Elle peut attirer les projectiles à elle en maintenant (X)",
		"Bien ! Vous connaissez toutes les techniques du Protecteur.",
		"Je pense qu’il y a un moyen de les combiner...",
		"Ceci est la jauge de rayon magique de votre destrier.",
		"Quand vous êtes le Protecteur, vous pouvez la remplir",
		"En absorbant les projectiles ennemis avec (A)",
		"N'oubliez pas qu'il mettra plus de temps à charger sur le champ de bataille !",
		"Voilà ! Maintenant vous pouvez passer en rôle d’Assaillant avec (R1)",
		"Maintenez (X) et anéantissez vos ennemis !",
		"Parfait ! Attention, quand cette jauge sera vide,",
		"Vous devrez repasser en rôle de Protecteur pour la remplir de nouveau.",
		"Il y a un avantage à avoir chacun un rôle différent.",
		"J2, passe en rôle de Protecteur !",
		"Voilà ! Vous êtes maintenant liés.",
		"Cette chaîne va blesser les ennemis qu’elle touche,",
		"Ce qui sera utile pour les moments difficiles...",
		"Pour finir, cette jauge représente le chargement de votre coup spécial",
		"Quand elle est remplie, appuyez sur (B) pour l’utiliser.",
		"Le Protecteur pourra ralentir le temps,",
		"L’Assaillant pourra faire disparaître les projectiles ennemis",
		"Et faire des dégâts dans une zone limitée.",
		"Essentiel en cas de danger !",
		"Rappelez-vous, (B) !",
		"Voilà, votre entraînement est terminé.",
		"Vous êtes prêts pour le combat (je pense...)",
		"Bonne chance !",
	};
	public int[] sentencesPerState = {
		3,//0
		2,//1
		8,//2
		2,//3
		1,//4
		1,//5
		1,//6
		6,//7
		1,//8
		1,//9
		2,//10
		1,//11
		2,//12
		1,//13
		1,//14
		1,//15
		2,//16
		1,//17
		2,//18
		1,//19
		1,//20
		3,//21
		1,//22
		5,//23
		1,//24
		3 //25
	};
	public GameObject[] tutoObjects;
	private float[] tmpValues = { 0f, 0f, 0f };
	private int currentStateSentence = 0;
	private int currentSentence = 0;
	private int currentSentenceLength = 0;
	private int conditions = 0;
	private bool awaitingAction = false;
	private string sentence = "";

	void Start () {
		for (int i = 0; i < (int)currentState; ++i) {
			currentSentence += sentencesPerState [i];
		}
		SetState ();
		GetComponent<Animator> ().Play ("vieux_parle");
		grayZone.transform.parent = ViewportHandler.viewport.transform;
		grayZone.transform.localPosition = Vector3.zero;
	}

	void Update () {
		if (currentSentence >= sentences.Length) {
			Destroy (grayZone);
			Destroy (gameObject);
			return;
		}

		if (currentSentenceLength <= sentences [currentSentence].Length) {
			sentence = sentences [currentSentence].Substring (0, currentSentenceLength);
			if (++currentSentenceLength > sentences [currentSentence].Length) {
				#if UNITY_EDITOR
				//Debug.Log (currentState + " reached end of sentence, awaiting = "+awaitingAction);
				//EditorApplication.isPaused = true;
				#endif
				if (++currentStateSentence >= sentencesPerState [(int)currentState]) {
					SetWaitAction ();
				}
				GetComponent<Animator> ().Play ("vieux_idle");
			}
		} else {
			//Debug.Log (currentState + " : "+awaitingAction+":"+conditions.ToString("X")+":"+ currentSentenceLength);
			if (WaitAction () || (awaitingAction ? false : Input.GetKeyDown (KeyCode.Return))) {
				if (currentStateSentence >= sentencesPerState [(int)currentState]) {
					ResetState ();
					awaitingAction = false;
					currentStateSentence = 0;
					currentState++;
					SetState ();
				}
				currentSentenceLength = 0;
				currentSentence++;
				GetComponent<Animator> ().Play ("vieux_parle");
			}/*
			if (Input.GetKeyDown (KeyCode.Backspace)) {
				currentSentence = 0;
				for (int i = 0; i < (int)currentState; ++i) {
					currentSentence += sentencesPerState [i];
				}
				currentSentenceLength = 0;
				GetComponent<Animator> ().Play ("vieux_parle");
			}//*/
		}
	}

	bool SetWaitAction () {
		switch (currentState) {
		case State.S_01_Deplacement:
			grayZone.GetComponent<SpriteRenderer> ().enabled = false;
			player1.GetComponent<Player> ().SetAbilities (Player.Ability.Move, true);
			player2.GetComponent<Player> ().SetAbilities (Player.Ability.Move, true);
			conditions = 0x03;
			break;
		case State.S_05_TueMouches:
			conditions = 0x01;
			break;
		case State.S_07_PassageSupport:
			conditions = 0x03;
			break;
		case State.S_08_UtiliseDash:
			grayZone.GetComponent<SpriteRenderer> ().enabled = false;
			player1.GetComponent<Player> ().SetAbilities (Player.Ability.Move | Player.Ability.Dash, true);
			player2.GetComponent<Player> ().SetAbilities (Player.Ability.Move | Player.Ability.Dash, true);
			conditions = 0x03;
			break;
		case State.S_10_AbsorbeProjectiles:
			conditions = 0x03;
			break;
		case State.S_12_UtiliseMagnet:
			conditions = 0x03;
			break;
		case State.S_16_RemplitLaser:
			conditions = 0x03;
			break;
		case State.S_18_UtiliseLaser:
			conditions = 0x01;
			break;
		case State.S_21_J2PasseSupport:
			conditions = 0x01;
			break;
		case State.S_25_UtiliseSpecial:
			conditions = 0x01;
			break;
		default:
			break;
		}
		return awaitingAction = (conditions != 0);
	}

	bool WaitAction () {
		if (!awaitingAction)
			return false;
		//Debug.Log ("wait action");
		switch (currentState) {
		case State.S_01_Deplacement:
			if (Input.GetAxis ("Horizontal_P1") != 0f || Input.GetAxis ("Vertical_P1") != 0f) {
				conditions &= ~0x01;
			}
			if (Input.GetAxis ("Horizontal_P2") != 0f || Input.GetAxis ("Vertical_P2") != 0f) {
				conditions &= ~0x02;
			}
			break;
		case State.S_05_TueMouches:
			if (tutoObjects [0] == null) {
				conditions &= ~0x01;
			}
			break;
		case State.S_07_PassageSupport:
			if (player1.GetComponent<Player> ().state == Player.PlayerState.Support) {
				conditions &= ~0x01;
			}
			if (player2.GetComponent<Player> ().state == Player.PlayerState.Support) {
				conditions &= ~0x02;
			}
			break;
		case State.S_08_UtiliseDash:
			if (player1.GetComponent<Player> ().dash) {
				conditions &= ~0x01;
			}
			if (player2.GetComponent<Player> ().dash) {
				conditions &= ~0x02;
			}
			break;
		case State.S_10_AbsorbeProjectiles:
			if (player1.GetComponent<Player> ().projectilesAbsorbed >= 3) {
				conditions &= ~0x01;
			}
			if (player2.GetComponent<Player> ().projectilesAbsorbed >= 3) {
				conditions &= ~0x02;
			}
			break;
		case State.S_12_UtiliseMagnet:
			if (player1.GetComponent<Player> ().magnet.GetComponent<Magnet> ().projectilesAttracted >= 3) {
				conditions &= ~0x01;
			}
			if (player2.GetComponent<Player> ().magnet.GetComponent<Magnet> ().projectilesAttracted >= 3) {
				conditions &= ~0x02;
			}
			break;
		case State.S_16_RemplitLaser:
			if (player1.GetComponent<Player> ().LaserLoad () >= 1f) {
				conditions &= ~0x01;
			}
			if (player2.GetComponent<Player> ().LaserLoad () >= 1f) {
				conditions &= ~0x02;
			}
			break;
		case State.S_18_UtiliseLaser:
			if (tutoObjects [2] == null) {
				conditions &= ~0x01;
			}
			break;
		case State.S_21_J2PasseSupport:
			if (player2.GetComponent<Player> ().state == Player.PlayerState.Support) {
				conditions &= ~0x01;
			}
			break;
		case State.S_25_UtiliseSpecial:
			if (tutoObjects [3] == null) {
				conditions &= ~0x01;
			}
			break;
		default:
			break;
		}
		return conditions == 0;
	}

	void SetState () {
		//Debug.Log ("current state : " + currentState);
		switch (currentState) {
		case State.S_01_Deplacement:
			grayZone.GetComponent<SpriteRenderer> ().enabled = true;
			player1.GetComponent<Player> ().SetAbilities (Player.Ability.All, false);
			player2.GetComponent<Player> ().SetAbilities (Player.Ability.All, false);
			break;
		case State.S_02_JaugesVie:
			{
				grayZone.GetComponent<SpriteRenderer> ().enabled = true;
				Transform hg1 = player1.GetComponent<Player> ().healthGauge.transform.parent;
				Transform hg2 = player2.GetComponent<Player> ().healthGauge.transform.parent;
				for (int i = 0; i < hg1.childCount; ++i) {
					hg1.GetChild (i).gameObject.GetComponent<SpriteRenderer> ().sortingOrder += 4;
					hg2.GetChild (i).gameObject.GetComponent<SpriteRenderer> ().sortingOrder += 4;
				}
			}
			break;
		case State.S_03_Roles:
			grayZone.GetComponent<SpriteRenderer> ().enabled = true;
			break;
		case State.S_04_BasesDPS:
			grayZone.GetComponent<SpriteRenderer> ().enabled = true;
			break;
		case State.S_05_TueMouches:
			grayZone.GetComponent<SpriteRenderer> ().enabled = false;
			player1.GetComponent<Player> ().SetAbilities (Player.Ability.Shotgun | Player.Ability.Move, true);
			player2.GetComponent<Player> ().SetAbilities (Player.Ability.Shotgun | Player.Ability.Move, true);
			tutoObjects [0].SetActive (true);
			tutoObjects [0].GetComponent<EnemyGroup> ().Unleash ();
			break;
		case State.S_06_BasesSupport:
			grayZone.GetComponent<SpriteRenderer> ().enabled = true;
			break;
		case State.S_07_PassageSupport:
			grayZone.GetComponent<SpriteRenderer> ().enabled = false;
			player1.GetComponent<Player> ().SetAbilities (Player.Ability.GoSupport, true);
			player2.GetComponent<Player> ().SetAbilities (Player.Ability.GoSupport, true);
			break;
		case State.S_08_UtiliseDash:
			grayZone.GetComponent<SpriteRenderer> ().enabled = true;
			break;
		case State.S_09_PresSupport:
			grayZone.GetComponent<SpriteRenderer> ().enabled = true;
			break;
		case State.S_10_AbsorbeProjectiles:
			grayZone.GetComponent<SpriteRenderer> ().enabled = false;
			player1.GetComponent<Player> ().SetAbilities (Player.Ability.Move | Player.Ability.Dash, true);
			player2.GetComponent<Player> ().SetAbilities (Player.Ability.Move | Player.Ability.Dash, true);
			tutoObjects [1].SetActive (true);
			for (int i = 0; i < tutoObjects [1].transform.childCount; ++i) {
				tutoObjects [1].transform.GetChild (i).GetComponent<ProjectileEmitter> ().isEnemy = true;
				tutoObjects [1].transform.GetChild (i).GetComponent<ProjectileEmitter> ().enabled = true;
			}
			break;
		case State.S_11_PresMagnet:
			grayZone.GetComponent<SpriteRenderer> ().enabled = true;
			break;
		case State.S_12_UtiliseMagnet:
			grayZone.GetComponent<SpriteRenderer> ().enabled = false;
			player1.GetComponent<Player> ().SetAbilities (Player.Ability.Move | Player.Ability.Magnet, true);
			player2.GetComponent<Player> ().SetAbilities (Player.Ability.Move | Player.Ability.Magnet, true);
			tutoObjects [1].SetActive (true);
			for (int i = 0; i < tutoObjects [1].transform.childCount; ++i) {
				tutoObjects [1].transform.GetChild (i).GetComponent<ProjectileEmitter> ().isEnemy = true;
				tutoObjects [1].transform.GetChild (i).GetComponent<ProjectileEmitter> ().enabled = true;
			}
			break;
		case State.S_13_TransitionLaser:
			grayZone.GetComponent<SpriteRenderer> ().enabled = true;
			break;
		case State.S_14_JaugesLaser:
			{
				grayZone.GetComponent<SpriteRenderer> ().enabled = true;
				Transform lg1 = player1.GetComponent<Player> ().laserGauge.transform.parent;
				Transform lg2 = player2.GetComponent<Player> ().laserGauge.transform.parent;
				for (int i = 0; i < lg1.childCount; ++i) {
					lg1.GetChild (i).gameObject.GetComponent<SpriteRenderer> ().sortingOrder += 4;
					lg2.GetChild (i).gameObject.GetComponent<SpriteRenderer> ().sortingOrder += 4;
				}
			}
			break;
		case State.S_15_PresLaser:
			grayZone.GetComponent<SpriteRenderer> ().enabled = true;
			break;
		case State.S_16_RemplitLaser:
			grayZone.GetComponent<SpriteRenderer> ().enabled = false;
			player1.GetComponent<Player> ().SetAbilities (Player.Ability.Move | Player.Ability.LoadLaser, true);
			player2.GetComponent<Player> ().SetAbilities (Player.Ability.Move | Player.Ability.LoadLaser, true);
			tmpValues [0] = tutoObjects [2].transform.childCount;
			tmpValues [1] = tutoObjects.Length;
			tmpValues [2] = player1.GetComponent<Player> ().maxLaserLoad;
			player1.GetComponent<Player> ().SetLaserMaxLoad (3f);
			player2.GetComponent<Player> ().SetLaserMaxLoad (3f);
			System.Array.Resize (ref tutoObjects, tutoObjects.Length + tutoObjects [2].transform.childCount);
			for (int i = 0; i < tmpValues [0]; ++i) {
				tutoObjects [i + (int)tmpValues [1]] = tutoObjects [2].transform.GetChild (i).gameObject;
			}
			tutoObjects [2].SetActive (true);
			tutoObjects [2].GetComponent<EnemyGroup> ().Unleash ();
			break;
		case State.S_17_LaserPasseDPS:
			grayZone.GetComponent<SpriteRenderer> ().enabled = true;
			break;
		case State.S_18_UtiliseLaser:
			grayZone.GetComponent<SpriteRenderer> ().enabled = false;
			player1.GetComponent<Player> ().SetAbilities (Player.Ability.Move | Player.Ability.GoSupport | Player.Ability.GoDPS | Player.Ability.Laser | Player.Ability.LoadLaser, true);
			player2.GetComponent<Player> ().SetAbilities (Player.Ability.Move | Player.Ability.GoSupport | Player.Ability.GoDPS | Player.Ability.Laser | Player.Ability.LoadLaser, true);
			for (int i = 0; i < tmpValues [0]; ++i) {
				tutoObjects [i + (int)tmpValues [1]].GetComponent<Enemy> ().enabled = true;
			}
			break;
		case State.S_19_WarningLaser:
			grayZone.GetComponent<SpriteRenderer> ().enabled = true;
			break;
		case State.S_20_Complementaires:
			grayZone.GetComponent<SpriteRenderer> ().enabled = true;
			break;
		case State.S_21_J2PasseSupport:
			grayZone.GetComponent<SpriteRenderer> ().enabled = false;
			player2.GetComponent<Player> ().SetAbilities (Player.Ability.GoSupport | Player.Ability.Chain, true);
			player1.GetComponent<Player> ().SetAbilities (Player.Ability.GoDPS | Player.Ability.Chain, true);
			break;
		case State.S_22_UtiliseChaine:
			grayZone.GetComponent<SpriteRenderer> ().enabled = true;
			break;
		case State.S_23_JaugesSpecial:
			{
				grayZone.GetComponent<SpriteRenderer> ().enabled = true;
				Transform sg1 = player1.GetComponent<Player> ().specialGauge.transform.parent;
				Transform sg2 = player2.GetComponent<Player> ().specialGauge.transform.parent;
				for (int i = 0; i < sg1.childCount; ++i) {
					sg1.GetChild (i).gameObject.GetComponent<SpriteRenderer> ().sortingOrder += 4;
					sg2.GetChild (i).gameObject.GetComponent<SpriteRenderer> ().sortingOrder += 4;
				}
			}
			break;
		case State.S_24_PresSpecial:
			grayZone.GetComponent<SpriteRenderer> ().enabled = true;
			break;
		case State.S_25_UtiliseSpecial:
			grayZone.GetComponent<SpriteRenderer> ().enabled = false;
			player1.GetComponent<Player> ().SetAbilities (Player.Ability.Bomb | Player.Ability.Move, true);
			player2.GetComponent<Player> ().SetAbilities (Player.Ability.BulletTime | Player.Ability.Move, true);
			tutoObjects [3].SetActive (true);
			tutoObjects [3].GetComponent<EnemyGroup> ().Unleash ();
			break;
		case State.S_26_FinTuto:
			grayZone.GetComponent<SpriteRenderer> ().enabled = true;
			break;
		default:
			break;
		}
	}

	void ResetState () {
		switch (currentState) {
		case State.S_01_Deplacement:
			player1.GetComponent<Player> ().SetAbilities (Player.Ability.All, false);
			player2.GetComponent<Player> ().SetAbilities (Player.Ability.All, false);
			break;
		case State.S_02_JaugesVie:
			{
				Transform hg1 = player1.GetComponent<Player> ().healthGauge.transform.parent;
				Transform hg2 = player2.GetComponent<Player> ().healthGauge.transform.parent;
				for (int i = 0; i < hg1.childCount; ++i) {
					hg1.GetChild (i).gameObject.GetComponent<SpriteRenderer> ().sortingOrder -= 4;
					hg2.GetChild (i).gameObject.GetComponent<SpriteRenderer> ().sortingOrder -= 4;
				}
			}
			break;
		case State.S_05_TueMouches:
			player1.GetComponent<Player> ().SetAbilities (Player.Ability.All, false);
			player2.GetComponent<Player> ().SetAbilities (Player.Ability.All, false);
			break;
		case State.S_07_PassageSupport:
			player1.GetComponent<Player> ().SetAbilities (Player.Ability.All, false);
			player2.GetComponent<Player> ().SetAbilities (Player.Ability.All, false);
			break;
		case State.S_08_UtiliseDash:
			player1.GetComponent<Player> ().SetAbilities (Player.Ability.All, false);
			player2.GetComponent<Player> ().SetAbilities (Player.Ability.All, false);
			break;
		case State.S_10_AbsorbeProjectiles:
			player1.GetComponent<Player> ().SetAbilities (Player.Ability.All, false);
			player2.GetComponent<Player> ().SetAbilities (Player.Ability.All, false);
			tutoObjects [1].SetActive (false);
			break;
		case State.S_12_UtiliseMagnet:
			player1.GetComponent<Player> ().SetAbilities (Player.Ability.All, false);
			player2.GetComponent<Player> ().SetAbilities (Player.Ability.All, false);
			tutoObjects [1].SetActive (false);
			break;
		case State.S_14_JaugesLaser:
			{
				Transform lg1 = player1.GetComponent<Player> ().laserGauge.transform.parent;
				Transform lg2 = player2.GetComponent<Player> ().laserGauge.transform.parent;
				for (int i = 0; i < lg1.childCount; ++i) {
					lg1.GetChild (i).gameObject.GetComponent<SpriteRenderer> ().sortingOrder -= 4;
					lg2.GetChild (i).gameObject.GetComponent<SpriteRenderer> ().sortingOrder -= 4;
				}
			}
			break;
		case State.S_16_RemplitLaser:
			player1.GetComponent<Player> ().SetAbilities (Player.Ability.All, false);
			player2.GetComponent<Player> ().SetAbilities (Player.Ability.All, false);
			for (int i = 0; i < tmpValues [0]; ++i) {
				tutoObjects [i + (int)tmpValues [1]].GetComponent<Enemy> ().enabled = false;
			}
			break;
		case State.S_18_UtiliseLaser:
			player1.GetComponent<Player> ().SetAbilities (Player.Ability.All, false);
			player2.GetComponent<Player> ().SetAbilities (Player.Ability.All, false);
			break;
		case State.S_21_J2PasseSupport:
			player1.GetComponent<Player> ().SetAbilities (Player.Ability.GoDPS, false);
			player2.GetComponent<Player> ().SetAbilities (Player.Ability.GoSupport, false);
			player1.GetComponent<Player> ().SetAbilities (Player.Ability.Chain, true);
			player2.GetComponent<Player> ().SetAbilities (Player.Ability.Chain, true);
			break;
		case State.S_23_JaugesSpecial:
			{
				Transform sg1 = player1.GetComponent<Player> ().specialGauge.transform.parent;
				Transform sg2 = player2.GetComponent<Player> ().specialGauge.transform.parent;
				for (int i = 0; i < sg1.childCount; ++i) {
					sg1.GetChild (i).gameObject.GetComponent<SpriteRenderer> ().sortingOrder -= 4;
					sg2.GetChild (i).gameObject.GetComponent<SpriteRenderer> ().sortingOrder -= 4;
				}
			}
			break;
		case State.S_25_UtiliseSpecial:
			player1.GetComponent<Player> ().SetAbilities (Player.Ability.All, false);
			player2.GetComponent<Player> ().SetAbilities (Player.Ability.All, false);
			break;
		case State.S_26_FinTuto:
			player1.GetComponent<Player> ().SetAbilities (Player.Ability.All, true);
			player2.GetComponent<Player> ().SetAbilities (Player.Ability.All, true);
			player1.GetComponent<Player> ().SetLaserMaxLoad (tmpValues [2]);
			player2.GetComponent<Player> ().SetLaserMaxLoad (tmpValues [2]);
			player1.GetComponent<Player> ().Reset ();
			player2.GetComponent<Player> ().Reset ();
			break;
		default:
			break;
		}
	}

	void OnGUI () {
		if (currentSentence >= sentences.Length)
			return;
		GUIStyle style = new GUIStyle ();
		style.normal.textColor = Color.white;
		style.fontSize = 15;
		GUI.TextArea (new Rect (Camera.main.WorldToScreenPoint( transform.position - GetComponent<SpriteRenderer> ().bounds.extents).x, Screen.height - Camera.main.WorldToScreenPoint( transform.position + GetComponent<SpriteRenderer> ().bounds.extents).y - 60, 150, 60), sentence, style);
	}
}
