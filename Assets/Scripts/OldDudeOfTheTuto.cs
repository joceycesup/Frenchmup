using UnityEngine;
using System.Collections.Generic;

public class OldDudeOfTheTuto : MonoBehaviour {

	public enum State {
		S_01_Deplacement = 0,
		S_02_JaugesVie = 1,
		S_03_Roles = 2,
		S_04_BasesDPS = 3,
		S_05_TueMouches = 4,
		S_06_ = 5,
		S_07_ = 6,
		S_08_ = 7,
		S_09_ = 8,
		S_10_ = 9,
		S_11_ = 10,
		S_12_ = 11,
		S_13_ = 12,
		S_14_ = 13,
		S_15_ = 14,
		S_16_ = 15,
		S_17_ = 16,
		S_18_ = 17,
		S_19_ = 18,
		S_20_ = 19,
		S_21_ = 20,
		S_22_ = 21,
		S_23_ = 22,
		S_24_ = 23,
		S_25_ = 24,
		S_26_FinTuto = 25
	}

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
	}

	void Update () {
		if (currentSentence >= sentences.Length) {
			Destroy (gameObject);
			return;
		}
		if (conditions != 0) {
			switch (currentState) {
			case State.S_01_Deplacement:
				break;
			case State.S_02_JaugesVie:
				break;
			case State.S_03_Roles:
				break;
			case State.S_04_BasesDPS:
				break;
			case State.S_05_TueMouches:
				break;
			}
		}
		if (currentSentenceLength <= sentences [currentSentence].Length) {
			Debug.ClearDeveloperConsole ();
			sentence = sentences [currentSentence].Substring (0, currentSentenceLength);
			if (++currentSentenceLength > sentences [currentSentence].Length) {
				GetComponent<Animator> ().Play ("vieux_idle");
			}
		} else {
			if (awaitingAction) {
				WaitAction ();
				if (conditions == 0) {
					awaitingAction = false;
					currentStateSentence = 0;
					currentState++;
					currentSentenceLength = 0;
					currentSentence++;
					GetComponent<Animator> ().Play ("vieux_parle");
					SetState ();
				}
			} else if (Input.GetKeyDown (KeyCode.Return)) {
				if (++currentStateSentence >= sentencesPerState [(int)currentState]) {
					awaitingAction = true;
				} else {
					currentSentenceLength = 0;
					currentSentence++;
					GetComponent<Animator> ().Play ("vieux_parle");
				}
			}
		}
	}

	void WaitAction () {
		switch (currentState) {
		case State.S_01_Deplacement:
			if (Input.GetAxis ("Horizontal_P1") != 0f || Input.GetAxis ("Vertical_P1") != 0f) {
				conditions &= ~0x01;
			}
			if (Input.GetAxis ("Horizontal_P2") != 0f || Input.GetAxis ("Vertical_P2") != 0f) {
				conditions &= ~0x02;
			}
			break;
		case State.S_02_JaugesVie:
			break;
		case State.S_03_Roles:
			break;
		case State.S_04_BasesDPS:
			break;
		case State.S_05_TueMouches:
			break;
		}
	}

	void SetState () {
		switch (currentState) {
		case State.S_01_Deplacement:
			conditions = 0x03;
			break;
		case State.S_02_JaugesVie:
			break;
		case State.S_03_Roles:
			break;
		case State.S_04_BasesDPS:
			break;
		case State.S_05_TueMouches:
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
		GUI.TextArea (new Rect (10, Screen.height - Camera.main.WorldToScreenPoint( transform.position + GetComponent<SpriteRenderer> ().bounds.extents).y - 60, 150, 60), sentence, style);
	}
}
