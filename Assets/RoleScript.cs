using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class RoleScript : MonoBehaviour {

	public Player player;

	public Sprite DPS;
	public Sprite SUP;

	void Update(){
		if (player.state == Player.PlayerState.DPS) {
			gameObject.GetComponent<Image> ().sprite = DPS;
		} else {
			gameObject.GetComponent<Image> ().sprite = SUP;
		}
	}

}
