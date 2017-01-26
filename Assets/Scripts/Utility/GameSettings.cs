using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameSettings : MonoBehaviour {

	public enum GameState {
		MainMenu,
		Credit,
		GameOver,
		Playing,
		GameStart
	}

	private static GameObject _game_settings;

	public static GameObject game_settings {
		get { return _game_settings; }
	}

	public static bool tutorial {
		get;
		private set;
	}
	//*
	public static GameState state {
		get;
		private set;
	}

	void Awake () {
		GameSettings[] gs = GameObject.FindObjectsOfType<GameSettings> ();
		if (gs.Length > 1) {
			Destroy (_game_settings);
			_game_settings = gameObject;
		} else {
			state = GameState.GameStart;
			_game_settings = gameObject;
			tutorial = true;
		}
		DontDestroyOnLoad (gameObject);
	}

	void Start () {//*
		if (state == GameState.GameStart) {
			state = GameState.MainMenu;
			AkSoundEngine.PostEvent ("start_game", SoundBank.bank);
			Menu menu = GameObject.FindObjectOfType<Menu> ();
			if (menu != null) {
				menu.SetState ();
			}//*/
		}
	}

	public static void SetTuto (bool tuto) {
		tutorial = tuto;
	}

	public void LoadGame (bool tuto) {
		SetTuto (tuto);
		Cursor.visible = false;
		Destroy (gameObject.GetComponent<Menu> ());
		AkSoundEngine.PostEvent (tuto?"tuto":"not_tuto", SoundBank.bank);
		SceneManager.LoadScene (1);
		//SceneManager.LoadScene ("Game");
	}

	public void LoadMenu () {
		Cursor.visible = true;
		AkSoundEngine.PostEvent ("game_over", SoundBank.bank);
		AkSoundEngine.PostEvent ("start_game", SoundBank.bank);
		SceneManager.LoadScene (0);
		//SceneManager.LoadScene ("Game");
	}

	public void Quit () {
		Application.Quit ();
	}

	public void TogglePause () {
		IngameTime.TogglePause ();
	}

	public static void SetState (GameState newState) {
		state = newState;
		switch (state) {
			case GameState.MainMenu:
				{
					Menu menu = GameObject.FindObjectOfType<Menu> ();
					if (menu != null) {
						menu.SetState ();
					} else {
						_game_settings.GetComponent<GameSettings> ().LoadMenu ();
					}
				}
				break;
			case GameState.Credit:
				{
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
			case GameState.GameOver:
				{
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
