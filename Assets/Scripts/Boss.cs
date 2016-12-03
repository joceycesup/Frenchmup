using UnityEngine;
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
		// Mets ce que tu veux ici
		if (Phase1 != null && !phase2 && health < maxHealth / 2f) {
			phase2 = true;
			// Mise en place de la phase 2

			Destroy (Phase1);
			Phase2.SetActive (true);

			//---------------------
		}
	}

	void OnDestroy(){
		GameObject smartBomb = (GameObject)Instantiate (Resources.Load<GameObject> ("Prefabs/SmartBomb"));
		smartBomb.transform.parent = ViewportHandler.viewport.transform;
		smartBomb.transform.position = transform.position;
	}

	public override string ToString() {
		return "Boss";
	}
}
