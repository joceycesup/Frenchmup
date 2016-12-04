using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameSettings : MonoBehaviour {
	private static GameObject _game_settings;

	public static GameObject game_settings {
		get { return _game_settings; }
	}
	
	public static bool tutorial {
		get;
		private set;
	}

	void Awake () {
		_game_settings = gameObject;
		AkSoundEngine.PostEvent ("start_game", gameObject);
		DontDestroyOnLoad (gameObject);
		tutorial = true;
	}

	void Start () {
	}

	public void LoadGame (bool tuto) {
		tutorial = tuto;
		Destroy (gameObject.GetComponent<Menu> ());
		SceneManager.LoadScene (1);
		//SceneManager.LoadScene ("Game");
	}
}
