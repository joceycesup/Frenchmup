using UnityEngine;
using System.Collections;

public class ViewportHandler : MonoBehaviour {
	private static GameObject _viewport;

	public float speed = 1.0f;
	public GameObject currentSection {
		get;
		private set;
	}
	private GameObject nextSection;
	private GameObject sections;

	public static GameObject viewport {
		get { return _viewport; }
	}

	void Awake () {//*
		gameObject.GetComponent<BoxCollider2D> ().enabled = true;
		Collider2D c2d = Physics2D.OverlapPoint (transform.position, 1 << LayerMask.NameToLayer ("Background"));
		if (c2d.gameObject.GetComponent<LevelSection> ()) {
			sections = c2d.gameObject.transform.parent.gameObject;
			SetCurrentSection (c2d.gameObject);
		}
		gameObject.GetComponent<BoxCollider2D> ().size = new Vector2 (c2d.bounds.extents.x * 2.0f, Camera.main.orthographicSize * 2.0f);
		float wallThickness = 5.0f;
		for (int i = 0; i < 4; ++i) {
			GameObject wall = new GameObject ();
			wall.layer = LayerMask.NameToLayer ("Wall");
			wall.AddComponent<BoxCollider2D> ();
			wall.GetComponent<BoxCollider2D> ().size = new Vector2 ((i % 2) == 1 ? wallThickness : (gameObject.GetComponent<BoxCollider2D> ().bounds.extents.x + wallThickness) * 2.0f, (i % 2) == 0 ? wallThickness : gameObject.GetComponent<BoxCollider2D> ().bounds.extents.y * 2.0f);
			wall.transform.position = new Vector3 (
				transform.position.x + (((i % 2) == 0 ? 0.0f : ((i < 2 ? 1.0f : -1.0f) * (gameObject.GetComponent<BoxCollider2D> ().bounds.extents.x + wallThickness / 2f)))),
				transform.position.y + (((i % 2) == 1 ? 0.0f : ((i < 2 ? 1.0f : -1.0f) * (gameObject.GetComponent<BoxCollider2D> ().bounds.extents.y + wallThickness / 2f)))));
			wall.transform.parent = transform;
		}/*/
		gameObject.GetComponent<BoxCollider2D> ().size = new Vector2 (Camera.main.orthographicSize * 2.0f * Screen.width / Screen.height, Camera.main.orthographicSize * 2.0f);//*/
		_viewport = gameObject;
	}

	void Update () {
		if (currentSection == null)
			return;
		if (currentSection.GetComponent<LevelSection> ().behaviour != LevelSection.LevelBehaviour.Static || currentSection.GetComponent<LevelSection> ().behaviourLock == null) {
			gameObject.transform.Translate (new Vector3 (0, speed * IngameTime.deltaTime, 0));
		}
	}//*

	void OnTriggerEnter2D (Collider2D other) {
		//Debug.Log ("collision enter " + other.gameObject);
		if (other.gameObject.tag == "Section") {
			if (currentSection == null) {
				sections = other.gameObject.transform.parent.gameObject;
				SetCurrentSection (other.gameObject);
			} else {
				if (other.gameObject.GetComponent<LevelSection> () != null) {
					if (currentSection.GetComponent<LevelSection> ().behaviour == LevelSection.LevelBehaviour.Loop) {
						if (currentSection.GetComponent<LevelSection> ().behaviourLock != null && nextSection != other.gameObject) {
							CopyCurrentSection ();
						} else {
							nextSection = other.gameObject;
						}
					} else {
						nextSection = other.gameObject;
					}
				} else {
					gameObject.transform.position = new Vector3 (0, currentSection.GetComponent<BoxCollider2D> ().bounds.extents.y * 2 + currentSection.transform.position.y - gameObject.GetComponent<BoxCollider2D> ().bounds.extents.y);
					GameObject.Find ("MainCamera").transform.parent = null;
					nextSection = null;
					//reached final section
				}
			}
			//Debug.Log (gameObject + ", Enter : " + other.gameObject);
		} else if (other.gameObject.tag == "EnemyGroup") {
			other.gameObject.GetComponent<EnemyGroup> ().Unleash ();
		}
	}

	void OnTriggerExit2D (Collider2D other) {
		if (other.gameObject.tag == "Section") {
			if (nextSection != null) {
				SetCurrentSection (nextSection);
				if (other.gameObject.transform.position.y < currentSection.transform.position.y) {// && other.gameObject.transform.childCount <= 0) {
					Destroy (other.gameObject);
				}
			} else {
				Destroy (gameObject);
			}
			//Debug.Log (gameObject + ", Exit : " + other.gameObject);
		} else if (other.gameObject.tag == "Projectile") {// || other.gameObject.tag == "Enemy") {
			Projectile pro = other.gameObject.GetComponent<Projectile> ();
			pro.desintegrateTime = 0.5f / pro.speed;
			pro.Disintegrate ();
		} else if (other.gameObject.tag == "Enemy") {
			Destroy (other.gameObject);
		}
	}//*/

	void SetCurrentSection (GameObject section) {
		if (currentSection == null)
			nextSection = section;
		currentSection = section;
		section.transform.parent = null;
		if (currentSection.GetComponent<LevelSection> ().behaviour != LevelSection.LevelBehaviour.Static || currentSection.GetComponent<LevelSection> ().behaviourLock == null) {
			gameObject.transform.Translate (new Vector3 (0, speed * IngameTime.deltaTime, 0));
		}
	}

	void CopyCurrentSection () {
		nextSection = Instantiate (currentSection);
		nextSection.transform.Translate (new Vector3 (0, currentSection.GetComponent<BoxCollider2D> ().bounds.extents.y * 2));
		sections.transform.Translate (new Vector3 (0, currentSection.GetComponent<BoxCollider2D> ().bounds.extents.y * 2));
	}
}
