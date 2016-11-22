using UnityEngine;
using System.Collections;

public class Enemy : Character {

	public enum PatternType {
		StaticOnSection,
		Static,
		Circle, // goes in circles around point : patternArgsO {axis}, patternArgsF {radius, clockwise (positive is clockwise, positive isn't)}
		Path // follows series of points : patternArgsO {points}, patternArgsF {wait time at each point}
	}

	public PatternType pattern;
	public GameObject[] patternArgsO = {};
	public float[] patternArgsF = {};
	private Vector3 targetPosition = new Vector3 (float.MaxValue, 0f);

	protected EnemyGroup m_group;

	void Awake () {
		_isEnemy = true;
		speed = maxSpeed;
		if (transform.parent != null)
			m_group = transform.parent.gameObject.GetComponent<EnemyGroup> ();
	}

	void Start () {
		switch (pattern) {
		case PatternType.StaticOnSection:
			break;
		case PatternType.Static:
			break;
		case PatternType.Path:
			if (patternArgsF.Length < 1) {
				Debug.LogWarningFormat("{0} has too few arguments in patternArgsF to follow a path", gameObject);
				pattern = PatternType.Static;
			}
			if (patternArgsO.Length < 2) {
				Debug.LogWarningFormat("{0} has too few arguments in patternArgsO to follow a path", gameObject);
				pattern = PatternType.Static;
			}
			break;
		case PatternType.Circle:
			if (patternArgsF.Length < 2) {
				Debug.LogWarningFormat ("{0} has too few arguments in patternArgsF to follow a circle", gameObject);
				pattern = PatternType.Static;
			} else if (patternArgsF[0] <= 0) {
				Debug.LogWarningFormat ("{0} radius argument must be strictly positive", gameObject);
				pattern = PatternType.Static;
			}
			if (patternArgsO.Length < 1) {
				Debug.LogWarningFormat ("{0} has too few arguments in patternArgsO to follow a circle", gameObject);
				pattern = PatternType.Static;
			}
			if (pattern != PatternType.Static) {
				float l = Vector3.Distance (patternArgsO [0].transform.position, transform.position);
				l = Mathf.Sqrt (l * l - patternArgsF [0] * patternArgsF [0]);
				Vector3 targetPositionTmp = new Vector3 (patternArgsF [1] >= 0 ? patternArgsF [0] : -patternArgsF [0], 0f);
				Vector3 tmpPos = targetPositionTmp + new Vector3 (0f, l);
				//Debug.Log (Vector3.Angle (tmpPos, transform.position - patternArgsO [0].transform.position));
				/*
				GameObject targetPositionTmpObj = (GameObject)Instantiate (patternArgsO [0]);
				targetPositionTmpObj.transform.position = targetPositionTmp + patternArgsO [0].transform.position;
				targetPositionTmpObj.transform.localScale = new Vector3 (0.2f, 0.2f, 0.2f);
				targetPositionTmpObj.GetComponent<SpriteRenderer> ().color = Color.green;
				targetPositionTmpObj.GetComponent<SpriteRenderer> ().sortingOrder = 1;

				targetPositionTmpObj = (GameObject)Instantiate (patternArgsO [0]);
				targetPositionTmpObj.transform.position = tmpPos + patternArgsO [0].transform.position;
				targetPositionTmpObj.transform.localScale = new Vector3 (0.2f, 0.2f, 0.2f);
				targetPositionTmpObj.GetComponent<SpriteRenderer> ().color = Color.green;
				targetPositionTmpObj.GetComponent<SpriteRenderer> ().sortingOrder = 2;//*/

				float angle = Vector3.Angle (tmpPos, transform.position - patternArgsO [0].transform.position);
				if (Vector3.Cross (tmpPos, transform.position - patternArgsO [0].transform.position).z < 0) {
					angle = -angle;
				}
				targetPosition = Quaternion.Euler (0f, 0f, angle) * targetPositionTmp + patternArgsO [0].transform.position;
				/*
				GameObject tangentPoint = (GameObject)Instantiate (patternArgsO [0]);
				tangentPoint.transform.position = targetPositionTmp;
				tangentPoint.transform.localScale = new Vector3 (0.2f, 0.2f, 0.2f);
				tangentPoint.GetComponent<SpriteRenderer> ().color = Color.red;
				tangentPoint.GetComponent<SpriteRenderer> ().sortingOrder = 3;//*/
			}
			break;
		}
	}

	protected override void UpdateCharacter () {
		switch (pattern) {
		case PatternType.StaticOnSection:
			break;
		case PatternType.Static:
			break;
		case PatternType.Path:
			break;
		case PatternType.Circle:
			break;
		}
		if (targetPosition.x != float.MaxValue && transform.position != targetPosition) {
			transform.position = Vector3.Lerp (transform.position, targetPosition, speed * IngameTime.deltaTime / Vector3.Distance (transform.position, targetPosition));
		}

		if (Input.GetButton ("Fire1_P1")) {
			SetCanShoot (true);
		}
		if (Input.GetButtonUp ("Fire1_P1")) {
			SetCanShoot (false);
		}
	}

	void OnDestroy () {
		if (m_group != null) {
			m_group.RemoveEnemy ();
		} else {
			Debug.LogWarningFormat("{0} was destroyed but has no group", gameObject);
		}
	}

	public override string ToString() {
		return "Enemy";
	}

	void OnBecameInvisible () {
		Destroy (gameObject);
	}
}
