/*
using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine.UI;

[CustomEditor(typeof(NewDudeOfTheTuto))]
public class TutoDudeEditor : Editor {
	private ReorderableList phasesList;

	void OnEnable () {
		phasesList = new ReorderableList (serializedObject,
			serializedObject.FindProperty ("tutoPhases"),
			true, true, true, true);
		phasesList.drawElementCallback = (Rect rect, int index, bool isActive, bool isFocused) => {
			SerializedProperty element = phasesList.serializedProperty.GetArrayElementAtIndex(index);
			rect.y += 2;
			EditorGUI.PropertyField(new Rect(rect.x, rect.y, rect.width, 60),
				element.FindPropertyRelative("initAction"), GUIContent.none);
		};
	}

	public override void OnInspectorGUI () {
		NewDudeOfTheTuto tuto = (NewDudeOfTheTuto)target;

		tuto.actionDelay = EditorGUILayout.FloatField ("Action Delay", tuto.actionDelay);
		tuto.text = (Text) EditorGUILayout.ObjectField ("Text", tuto.text, typeof(Text), true);

		serializedObject.Update ();
		phasesList.DoLayoutList ();
		serializedObject.ApplyModifiedProperties ();
	}
}
//*/