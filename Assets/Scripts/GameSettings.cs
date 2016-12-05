using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameSettings : MonoBehaviour {

	public enum GameState {
		MainMenu,
		Credit,
		GameOver,
		Playing
	}

	private static GameObject _game_settings;

	public static GameObject game_settings {
		get { return _game_settings; }
	}

	public static bool tutorial {
		get;
		private set;
	}

	public static GameState state {
		get;
		private set;
	}

	void Awake () {
		state = GameState.MainMenu;
		if (GameObject.FindObjectsOfType<GameSettings> ().Length > 1) {
			Destroy (gameObject);
			return;
		}
		_game_settings = gameObject;
		AkSoundEngine.PostEvent ("start_game", gameObject);
		DontDestroyOnLoad (gameObject);
		tutorial = true;
	}

	void Start () {
	}

	public static void SetTuto (bool tuto) {
		tutorial = tuto;
	}

	public void LoadGame (bool tuto) {
		SetTuto (tuto);
		Destroy (gameObject.GetComponent<Menu> ());
		SceneManager.LoadScene (1);
		//SceneManager.LoadScene ("Game");
	}

	public void LoadMenu () {
		SceneManager.LoadScene (0);
		//SceneManager.LoadScene ("Game");
	}

	public void Quit () {
		Application.Quit ();
	}

	public void GameOver () {
		Destroy (gameObject.GetComponent<Menu> ());
		SceneManager.LoadScene (1);
		//SceneManager.LoadScene ("Game");
	}

	public static void SetState (GameState newState) {
		state = newState;
		switch (state) {
			case GameState.MainMenu: {
					Menu menu = GameObject.FindObjectOfType<Menu> ();
					if (menu != null) {
						menu.SetState ();
					} else {
						_game_settings.GetComponent<GameSettings> ().LoadMenu ();
					}
				}
				break;
			case GameState.Credit: {
					Menu menu = GameObject.FindObjectOfType<Menu> ();
					if (menu != null) {
						menu.SetState ();
					} else {
						_game_settings.GetComponent<GameSettings> ().LoadMenu ();
					}
				}
				break;
			case GameState.Playing:
				break;
			case GameState.GameOver: {
					Menu menu = GameObject.FindObjectOfType<Menu> ();
					if (menu != null) {
						menu.SetState ();
					} else {
						_game_settings.GetComponent<GameSettings> ().LoadMenu ();
					}
				}
				break;
			default:
				break;
		}
	}
}
