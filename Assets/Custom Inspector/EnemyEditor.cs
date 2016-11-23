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
			EditorGUILayout.HelpBox ("Static on Section :\n" +
				"Pattern Args O : takes no argument\n" +
				"Pattern Args F : takes no argument", MessageType.Info);
			break;
		case Enemy.PatternType.Static:
			EditorGUILayout.HelpBox ("Static :\n" +
				"Pattern Args O : takes no argument\n" +
				"Pattern Args F : takes no argument", MessageType.Info);
			break;
		case Enemy.PatternType.Circle:
			if (myTarget.patternArgsF.Length < 2) {
				EditorGUILayout.HelpBox ("Too few arguments in pattern Args F", MessageType.Error);
			} else if (myTarget.patternArgsF[0] <= 0) {
				EditorGUILayout.HelpBox ("Radius argument must be strictly positive", MessageType.Error);
			}
			if (myTarget.patternArgsO.Length < 1) {
				EditorGUILayout.HelpBox ("Too few arguments in Pattern Args O", MessageType.Error);
			}
			EditorGUILayout.HelpBox ("Circle :\n" +
				"Pattern Args O :\n" +
				" [0] : Center object\n" +
				"Pattern Args F :\n" +
				" [0] : Radius\n" +
				" [1] : Positive value if clockwise, negative value if counterclockwise", MessageType.Info);
			break;
		case Enemy.PatternType.Path:
			if (myTarget.patternArgsF.Length < 1) {
				EditorGUILayout.HelpBox ("Too few arguments in pattern Args F", MessageType.Error);
			}
			if (myTarget.patternArgsO.Length < 1) {
				EditorGUILayout.HelpBox ("Too few arguments in Pattern Args O", MessageType.Error);
			}
			EditorGUILayout.HelpBox ("Path :\n" +
				"Pattern Args O :\n" +
				" [...] : Intermediate waypoints\n" +
				"Pattern Args F :\n" +
				" [0] : Amount of time spent on each waypoint", MessageType.Info);
			break;
		}
	}
}
