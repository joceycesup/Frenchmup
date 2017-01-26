using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class SelectButton : MonoBehaviour {
	private Button button;

	void Awake () {
		button = GetComponent<Button> ();
	}

	void OnEnable() {
		StartCoroutine ("SelectLater");
	}

	IEnumerator SelectLater () {
		yield return null;
		EventSystem.current.SetSelectedGameObject(null);
		EventSystem.current.SetSelectedGameObject(button.gameObject);
	}
}
