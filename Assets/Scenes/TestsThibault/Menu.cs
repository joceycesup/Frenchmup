using UnityEngine;
using System.Collections;

public class Menu : MonoBehaviour {

	public GameObject mainMenu;
	public GameObject gameOver;
	public GameObject credit;

	void Awake () {
		SetState ();
	}

	public void SetState () {
		switch (GameSettings.state) {
			case GameSettings.GameState.MainMenu:
				mainMenu.SetActive (true);
				gameOver.SetActive (false);
				credit.SetActive (false);
				break;
			case GameSettings.GameState.Credit:
				mainMenu.SetActive (false);
				gameOver.SetActive (false);
				credit.SetActive (true);
				break;
			case GameSettings.GameState.Playing:
				mainMenu.SetActive (false);
				gameOver.SetActive (false);
				credit.SetActive (false);
				break;
			case GameSettings.GameState.GameOver:
				mainMenu.SetActive (false);
				gameOver.SetActive (true);
				credit.SetActive (false);
				break;
			default:
				break;
		}
	}
}
