using UnityEngine;
using System.Collections;

public class ParticuleDestroyer : MonoBehaviour {

	void Start(){
		Debug.Log ("Hello");
	}

	void Update () {
		if (gameObject.GetComponent<ParticleSystem> ().isStopped) {
			Destroy (gameObject);
			Debug.Log ("Goodbye");
		}
	}
}
