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

		switch (myTarget.behaviour) {
		case ProjectileEmitter.EmitterBehaviour.Static:
			EditorGUILayout.HelpBox ("Static" +
				"\nElement 0 : MaxPlayerDistance", MessageType.Info);
			break;
		case ProjectileEmitter.EmitterBehaviour.TargetAdversary:
			EditorGUILayout.HelpBox ("Target" +
				"\nElement 0 :" +
				"\nElement 1 :" +
				"\nElement 2 :", MessageType.Info);
			break;
		case ProjectileEmitter.EmitterBehaviour.Star:
			EditorGUILayout.HelpBox ("Star" +
				"\nElement 0 :" +
				"\nElement 1 :" +
				"\nElement 2 :", MessageType.Info);
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
