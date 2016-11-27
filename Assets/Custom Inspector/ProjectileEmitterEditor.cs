using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(ProjectileEmitter))]
[CanEditMultipleObjects]
public class ProjectileEmitterEditor : Editor {

	// Use this for initialization
	void OnEnable() {
	
	}
	
	public override void OnInspectorGUI(){

		//Voilà pour afficher l'inspecteur par défaut
		DrawDefaultInspector ();

		ProjectileEmitter myTarget = (ProjectileEmitter)target; 

		/*Ici pour mettre les infos d'utilisation
		 * Et quand on est carrement fout on peut overide l'inspecteur de base pour
		 * changer le nom qui est affiché pour les behaviour.
		 */
		/*
		if (myTarget.enabled)
			myTarget.enabled = false;
		//*/
		/*
		if (myTarget.gameObject.GetComponent<Character> () != null) {
			ProjectileEmitter pe = new ProjectileEmitter (myTarget);
			pe.gameObject.name = "ProjectileEmitter";
			pe.transform.parent = myTarget.transform;
			DestroyImmediate (myTarget);
		}//*/

		switch (myTarget.behaviour) {
		case ProjectileEmitter.EmitterBehaviour.Static:
			EditorGUILayout.HelpBox ("Static", MessageType.Info);
			break;
		case ProjectileEmitter.EmitterBehaviour.TargetAdversary:
			EditorGUILayout.HelpBox ("Target" +
				"\nElement 0 : Max adversary distance", MessageType.Info);
			break;
		case ProjectileEmitter.EmitterBehaviour.Star:
			EditorGUILayout.HelpBox ("Star" +
				"\nElement 0 : Number", MessageType.Info);
			break;
		case ProjectileEmitter.EmitterBehaviour.Shotgun:
			EditorGUILayout.HelpBox ("Shotgun" +
				"\nElement 0 : Number" +
				"\nElement 1 : Spread_Angle" +
				"\nElement 2 : minSpeedFactor", MessageType.Info);
			break;
		}
	}
}
