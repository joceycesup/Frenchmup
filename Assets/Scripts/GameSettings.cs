using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameSettings : MonoBehaviour {
	
	public static bool tutorial {
		get;
		private set;
	}

	void Awake () {
		DontDestroyOnLoad (gameObject);
		tutorial = true;
	}

	public void LoadGame (bool tuto) {
		tutorial = tuto;
		Destroy (gameObject.GetComponent<Menu> ());
		SceneManager.LoadScene (1);
		//SceneManager.LoadScene ("Game");
	}
}
