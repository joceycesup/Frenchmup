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
			EditorGUILayout.HelpBox ("Static on Section" +
			"\nInfo", MessageType.Info);
			break;
		case Projectile.BehaviourOverTime.Leaf:
			EditorGUILayout.HelpBox ("Static on Section" +
				"\nInfo", MessageType.Info);
			break;
		case Projectile.BehaviourOverTime.Rotate:
			EditorGUILayout.HelpBox ("Static on Section" +
				"\nInfo", MessageType.Info);
			break;
		case Projectile.BehaviourOverTime.Split:
			EditorGUILayout.HelpBox ("Split" +
				"\nA behaviourTime, le projectile se split en NextProjectile", MessageType.Info);
			break;
		}
	}
}
