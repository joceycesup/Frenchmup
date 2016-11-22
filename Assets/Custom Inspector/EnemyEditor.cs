using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Enemy))]
[CanEditMultipleObjects]
public class EnemyEditor : Editor {

	public override void OnInspectorGUI(){
		DrawDefaultInspector ();

		Enemy myTarget = (Enemy)target; 

		switch (myTarget.pattern) {
		case Enemy.PatternType.StaticOnSection:
			EditorGUILayout.HelpBox ("Static on Section" +
				"\nInfo", MessageType.Info);
			break;
		case Enemy.PatternType.Static:
			EditorGUILayout.HelpBox ("Static" +
				"\nInfo", MessageType.Info);
			break;
		case Enemy.PatternType.Circle:
			EditorGUILayout.HelpBox ("Circle" +
				"\nInfo", MessageType.Info);
			break;
		case Enemy.PatternType.Path:
			EditorGUILayout.HelpBox ("Path" +
				"\nInfo", MessageType.Info);
			break;
		}
	}
}
