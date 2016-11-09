using UnityEngine;
using System.Collections;

public class Enemy : Character {

	// Use this for initialization
	void Start () {
		isEnemy = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public string ToString() {
		return "Enemy";
	}
}
