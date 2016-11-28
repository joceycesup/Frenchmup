using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DebugUI : MonoBehaviour {

    Text text;
    public Player p1;

    void Start()
    {
        text = GetComponent<Text>();
    }

	// Update is called once per frame
	void Update () {
       // text.text = "Health P1 : " + p1.health;

    }
}
