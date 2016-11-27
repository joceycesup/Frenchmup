using UnityEngine;
using System.Collections;

public class Enemy : Character {

	public enum PatternType {
		StaticOnSection,
		Static,
		Circle, // goes in circles around point : patternArgsO {axis}, patternArgsF {radius, clockwise (positive is clockwise, positive isn't)}
		Path, // follows series of points : patternArgsO {points}, patternArgsF {wait time at each point}
		Bezier
	}

	public PatternType pattern;
	public GameObject[] patternArgsO = {};
	public float[] patternArgsF = {};
	private Vector3 targetPosition = new Vector3 (float.MaxValue, 0f);

	protected EnemyGroup m_group;

	protected override void AwakeCharacter () {
		_isEnemy = true;
		if (transform.parent != null)
			m_group = transform.parent.gameObject.GetComponent<EnemyGroup> ();
	}

	void Start () {
		SetCanShoot (true);
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
			if (patternArgsO.Length < 1) {
				Debug.LogWarningFormat("{0} has too few arguments in patternArgsO to follow a path", gameObject);
				pattern = PatternType.Static;
			}
			if (pattern != PatternType.Static) {//*
				if (patternArgsF.Length < 2)
					System.Array.Resize (ref patternArgsF, 2);
				patternArgsF [1] = 1f;//*/
				transform.position = patternArgsO[0].GetComponent<BezierSpline>().points[0];
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
				patternArgsF [1] = Mathf.Sign (patternArgsF [1]);//*
				if (patternArgsF.Length < 3)
					System.Array.Resize (ref patternArgsF, 3);
				patternArgsF [2] = -1f;//*/
				float l = Vector3.Distance (patternArgsO [0].transform.position, transform.position);
				l = Mathf.Sqrt (l * l - patternArgsF [0] * patternArgsF [0]);
				Vector3 targetPositionTmp = new Vector3 (patternArgsF [1] >= 0 ? patternArgsF [0] : -patternArgsF [0], 0f);
				Vector3 tmpPos = targetPositionTmp + new Vector3 (0f, l);

				float angle = Vector3.Angle (tmpPos, transform.position - patternArgsO [0].transform.position);
				if (Vector3.Cross (tmpPos, transform.position - patternArgsO [0].transform.position).z < 0) {
					angle = -angle;
				}
				targetPosition = Quaternion.Euler (0f, 0f, angle) * targetPositionTmp + patternArgsO [0].transform.position;
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
			if (((int)patternArgsF [1]) < patternArgsO [0].GetComponent<BezierSpline> ().points.Length) {
				Vector3 p2 = patternArgsO [0].GetComponent<BezierSpline> ().points [(int)patternArgsF [1]];
				float factor = (speed * IngameTime.deltaTime) / Vector3.Distance (transform.position, p2);
				transform.position = Vector3.Lerp (transform.position, p2, factor);
				if (factor >= 1f) {
					patternArgsF [1]++;
				}
			}
			break;
		case PatternType.Circle:
			if (transform.position == targetPosition)
				patternArgsF [2] = 1f;
			if (targetPosition.x != float.MaxValue && patternArgsF [2] >= 0) {
				float angle = ((speed * IngameTime.deltaTime) / (patternArgsF [0] * Mathf.PI)) * 180f * patternArgsF [1];
				targetPosition = Vector3.Normalize (Quaternion.Euler (0f, 0f, -angle) * (transform.position - patternArgsO [0].transform.position)) * patternArgsF [0] + patternArgsO [0].transform.position;
			}
			break;
		}
		if (targetPosition.x != float.MaxValue && transform.position != targetPosition) {
			transform.position = Vector3.Lerp (transform.position, targetPosition, speed * IngameTime.deltaTime / Vector3.Distance (transform.position, targetPosition));
		}
		/*
		if (Input.GetButton ("Fire1_P1")) {
			SetCanShoot (true);
		}
		if (Input.GetButtonUp ("Fire1_P1")) {
			SetCanShoot (false);
		}//*/
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
