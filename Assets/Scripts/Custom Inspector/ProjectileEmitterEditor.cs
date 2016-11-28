using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(ProjectileEmitter))]
[CanEditMultipleObjects]
public class ProjectileEmitterEditor : Editor {

	// Use this for initialization
	void OnEnable() {
	
	}
	
	public override void OnInspectorGUI(){
		if (Application.isPlaying)
			return;

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
		//*
		if (myTarget.gameObject.GetComponent<Character> () != null) {
			GameObject child = new GameObject ();
			child.name = "ProjectileEmitter";
			child.transform.parent = myTarget.transform;
			child.transform.localPosition = Vector3.zero;
			ProjectileEmitter pe = child.AddComponent<ProjectileEmitter> ();
			pe.SetProperties (myTarget);
			DestroyImmediate (myTarget);
		} else {
			myTarget.enabled = false;
		}//*/

		EditorGUILayout.HelpBox ("Fire Rate in frames " + Mathf.RoundToInt( 60f / myTarget.firingRate) + " frames", MessageType.Info);

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
