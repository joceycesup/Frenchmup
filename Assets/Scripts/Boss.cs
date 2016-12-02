using UnityEngine;
using System.Collections;

public class Boss : Enemy {
	bool phase2 = false;

	protected override void StartCharacter () {
		base.StartCharacter ();
		// Mets ce que tu veux ici pour l'initialisation


		//---------------------
	}

	protected override void UpdateCharacter () {
		base.UpdateCharacter ();
		// Mets ce que tu veux ici
		if (!phase2 && health < maxHealth / 2f) {
			phase2 = true;
			// Mise en place de la phase 2


			//---------------------
		}
	}

	public override string ToString() {
		return "Boss";
	}
}
