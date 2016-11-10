using UnityEngine;
using System.Collections;

public class ViewportHandler : MonoBehaviour {
	private static GameObject _viewport;

	public static GameObject viewport {
		get {
			return _viewport;
		}
	}

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<BoxCollider2D> ().size = new Vector2 (Camera.main.orthographicSize * 2.0f * Screen.width / Screen.height, Camera.main.orthographicSize * 2.0f);
		_viewport = gameObject;
	}
	
	// Update is called once per frame
	void Update () {
		gameObject.transform.Translate (new Vector3 (0, 0.01f, 0));
	}

	void OnTriggerExit2D (Collider2D other) {
		if (other.gameObject.tag == "Projectile") {
			Destroy (other.gameObject);
		}
	}
}
