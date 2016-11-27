using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Projectile))]
[CanEditMultipleObjects]
public class ProjectileEditor : Editor {

	public override void OnInspectorGUI(){
		DrawDefaultInspector ();

		Projectile myTarget = (Projectile)target; 

		switch (myTarget.behaviour) {
		case Projectile.BehaviourOverTime.DoNothing:
			break;
		case Projectile.BehaviourOverTime.Leaf:
			EditorGUILayout.HelpBox ("Leaf" +
				"\nElement 0 : Amplitude" +
				"\nElement 1 : Frequency", MessageType.Info);
			break;
		case Projectile.BehaviourOverTime.Rotate:
			EditorGUILayout.HelpBox ("Rotate" +
				"\nElement 0 : Angle", MessageType.Info);
			break;
		}
		switch (myTarget.pattern) {
		case Projectile.Pattern.None:
			break;
		case Projectile.Pattern.Sinusoidal:
			EditorGUILayout.HelpBox ("Sinusoidal" +
				"\nElement 0 : Amplitude" +
				"\nElement 1 : Frequency", MessageType.Info);
			break;
		case Projectile.Pattern.Spiral:
			EditorGUILayout.HelpBox ("Spiral" +
				"\nElement 0 : Amplitude" +
				"\nElement 1 : Frequency", MessageType.Info);
			break;
		}
	}
}
