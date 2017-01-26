using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
#if UNITY_EDITOR
#endif

public class NewDudeOfTheTuto : MonoBehaviour {
	public Text text;
	public GameObject grayZone;
	public GameObject bulle;
	public delegate void testDelegate ();

	//public GameObject grayZone;
	private GameObject[] bars = { null, null };
	public string[] sentences = {
/*S_01*/"Pour commencer, on va apprendre les bases",
		"Déplacez votre destrier en utilisant le stick directionnel gauche",
		"Ça vous permettra de ne pas tomber au combat tout de suite...",
/*S_02*/"Ça, c’est votre jauge de vie",
		"Pas besoin de vous dire ce qui arrivera si elle se vide...",
/*S_03*/"Si vous voulez gagner cette bataille,",
		"Il est indispensable de se battre,",
		"Mais aussi de se défendre !",
		"Je vais vous apprendre deux rôles : Assaillant et Protecteur.",
		"Pour l’instant, vous êtes en rôle d’Assaillant.",
		"Dans l’idéal, il faut que chacun d’entre vous ait un rôle différent.",
		"Vous êtes libres de choisir",
		"Mais ça vous aiderait à survivre plus de quelques secondes...",
/*S_04*/"En tant qu’Assaillant, vous pouvez tirer des lances avec (A)",
		"Voici quelques ennemis que nos gardes ont réussi à capturer.",
/*S_05*/"Essayez de les anéantir !",
/*S_06*/"Vous avez bien compris comment utiliser les bases du rôle d’Assaillant.",
/*S_07*/"Maintenant, passez au rôle Protecteur avec (L1)",
/*S_08*/"Très bien ! Votre rôle de Protecteur est d’empêcher l’Assaillant de mourir",
		"Son armure est beaucoup moins résistante que la vôtre,",
		"Alors il faut l’aider un peu.",
		"En tant que Protecteur, vous ne pouvez pas tirer,",
		"Mais vous pouvez faire avancer votre destrier plus rapidement \navec (A)",
		"Et en vous déplaçant avec le stick directionnel gauche",
/*S_09*/"En chargeant sur les projectiles ennemis",
/*S_10*/"Vous pouvez les absorber",
/*S_10*/"Vous commencez à comprendre l’importance du Protecteur, c’est bien !",
		"L’armure du Protecteur a une particularité,",
/*S_10*/"Elle peut attirer les projectiles à elle en maintenant (X)",
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

	public List<TutoPhase> tutoPhases;
	public float actionDelay = 0.5f;
	private float[] tmpValues = { 0f, 0f, 0f };
	private int currentStateSentence = 0;
	private int currentSentence = 0;
	private int currentSentenceLength = 0;
	private int conditions = 0;
	private bool awaitingAction = false;
	private bool awaitingEnter = false;
	private bool skipUpdate = false;
	private string sentence = "";

	void Awake () {
		tutoPhases = new List<TutoPhase> ();
	}

	void Start () {
		if (!((GameObject.FindObjectOfType<GameSettings> () != null) ? GameSettings.tutorial : false)) {
			Destroy (grayZone.transform.parent.gameObject);
			Destroy (gameObject);
		}
	}

	void Update () {
	}

	void UpdateText () {
		text.text = sentence;
	}
	/*
	void OnGUI () {
		if (currentSentence >= sentences.Length)
			return;
		GUIStyle style = new GUIStyle ();
		style.normal.textColor = Color.white;
		style.fontSize = 15;
		GUI.TextArea (new Rect (Camera.main.WorldToScreenPoint (transform.position - GetComponent<SpriteRenderer> ().bounds.extents).x, Screen.height - Camera.main.WorldToScreenPoint (transform.position + GetComponent<SpriteRenderer> ().bounds.extents).y - 60, 150, 60), sentence, style);
	}//*/
}