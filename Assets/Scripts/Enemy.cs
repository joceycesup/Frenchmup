using UnityEngine;
using UnityEditor;
using System.Collections;

public class Enemy : Character {

	public enum PatternType {
		StaticOnSection,
		Static,
		Circle, // goes in circles around point : patternArgsO {axis}, patternArgsF {radius, clockwise (positive is clockwise, positive isn't)}
		Path, // follows series of points : patternArgsO {points}, patternArgsF {wait time at each point}
		Bezier
	}

	public float maxSpeed;
	public PatternType pattern;
	public GameObject[] patternArgsO = {};
	public float[] patternArgsF = {};
	private Vector3 targetPosition = new Vector3 (float.MaxValue, 0f);

	protected EnemyGroup m_group;

	protected override void AwakeCharacter () {
		_isEnemy = true;
		speed = maxSpeed;
		if (transform.parent != null)
			m_group = transform.parent.gameObject.GetComponent<EnemyGroup> ();
	}

	void Start () {
		gameObject.GetComponent<Collider2D> ().enabled = true;
		switch (pattern) {
		case PatternType.StaticOnSection:
			SetCanShoot (true);
			break;
		case PatternType.Static:
			SetCanShoot (true);
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
				SetCanShoot (true);
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
				if (patternArgsF.Length < 3)
					System.Array.Resize (ref patternArgsF, 3);
				patternArgsF [1] = 1f;//*/
				patternArgsO [0].transform.parent = gameObject.transform.parent;
				transform.position = patternArgsO[0].GetComponent<BezierSpline>().points[0] + patternArgsO[0].transform.position;
				SetCanShoot (true);
			}
			break;
		case PatternType.Bezier:
			if (patternArgsF.Length < 1) {
				Debug.LogWarningFormat("{0} has too few arguments in patternArgsF to follow a bezier", gameObject);
				pattern = PatternType.Static;
			}
			if (patternArgsO.Length < 1) {
				Debug.LogWarningFormat("{0} has too few arguments in patternArgsO to follow a bezier", gameObject);
				pattern = PatternType.Static;
			}
			if (pattern != PatternType.Static) {//*
				if (patternArgsF.Length < 2)
					System.Array.Resize (ref patternArgsF, 2);
				patternArgsF [1] = 0f;//*/
				patternArgsO [0].transform.parent = gameObject.transform.parent;
				transform.position = patternArgsO[0].GetComponent<BezierSpline>().points[0] + patternArgsO[0].transform.position;
				SetCanShoot (true);
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
		case PatternType.Circle:
			if (transform.position == targetPosition)
				patternArgsF [2] = 1f;
			if (targetPosition.x != float.MaxValue && patternArgsF [2] >= 0) {
				float angle = ((speed * IngameTime.deltaTime) / (patternArgsF [0] * Mathf.PI)) * 180f * patternArgsF [1];
				targetPosition = Vector3.Normalize (Quaternion.Euler (0f, 0f, -angle) * (transform.position - patternArgsO [0].transform.position)) * patternArgsF [0] + patternArgsO [0].transform.position;
			}
			break;
		case PatternType.Path:
			if (IngameTime.time > patternArgsF [2]) {
				if (((int)patternArgsF [1]) < patternArgsO [0].GetComponent<BezierSpline> ().points.Length) {
					Vector3 p2 = patternArgsO [0].GetComponent<BezierSpline> ().points [(int)patternArgsF [1]] + patternArgsO[0].transform.position;
					float factor = (speed * IngameTime.deltaTime) / Vector3.Distance (transform.position, p2);
					transform.position = Vector3.Lerp (transform.position, p2, factor);
					if (factor >= 1f) {
						patternArgsF [2] = patternArgsF [0] + IngameTime.time;
						patternArgsF [1]++;
					}
				}
			}
			break;
		case PatternType.Bezier:
			if (patternArgsF [1] < 1f) {
				patternArgsF [1] = patternArgsO [0].GetComponent<BezierSpline> ().GetT (patternArgsF [1], speed * IngameTime.deltaTime, 10);
				transform.position = patternArgsO [0].GetComponent<BezierSpline> ().GetPoint (patternArgsF [1]);
			}
			break;
		}
		if (targetPosition.x != float.MaxValue && transform.position != targetPosition) {
			transform.position = Vector3.Lerp (transform.position, targetPosition, speed * IngameTime.deltaTime / Vector3.Distance (transform.position, targetPosition));
		}
	}

	void OnDestroy () {
		if (m_group != null) {
			m_group.RemoveEnemy ();
		} else {
			Debug.LogWarningFormat("{0} was destroyed but has no group", gameObject);
		}
		if (pattern == PatternType.Path || pattern == PatternType.Bezier) {
			if (patternArgsO.Length > 0 && patternArgsO [0] != null) {
				Destroy (patternArgsO [0]);
			}
		}
	}

	public override string ToString() {
		return "Enemy";
	}

	void OnDrawGizmosSelected () {
		if (Application.isPlaying)
			return;/*
		else
			this.enabled = false;//*/
		if (this.enabled)
			return;
		if (gameObject.GetComponent<Collider2D> ().enabled) {
			gameObject.GetComponent<Collider2D> ().enabled = false;
		}
		if (pattern == PatternType.Path) {
			if (patternArgsO.Length > 0 && patternArgsO [0] != null) {
				BezierSpline spline = patternArgsO [0].GetComponent<BezierSpline> ();
				if (spline != null) {
					Gizmos.color = Color.cyan;
					for (int i = 0; i < spline.points.Length - 1; ++i) {
						Gizmos.DrawLine (spline.points [i] + patternArgsO [0].transform.position, spline.points [i + 1] + patternArgsO [0].transform.position);
					}
				}
			}
		} else if (pattern == PatternType.Bezier) {
			if (patternArgsO.Length > 0) {
				BezierSpline spline = patternArgsO [0].GetComponent<BezierSpline> ();
				if (spline != null) {
					Vector3 p0 = spline.points [0] + patternArgsO [0].transform.position;
					for (int i = 1; i < spline.ControlPointCount; i += 3) {
						Vector3 p1 = spline.points [i] + patternArgsO [0].transform.position;
						Vector3 p2 = spline.points [i + 1] + patternArgsO [0].transform.position;
						Vector3 p3 = spline.points [i + 2] + patternArgsO [0].transform.position;

						Handles.color = Color.gray;
						Handles.DrawLine (p0, p1);
						Handles.DrawLine (p2, p3);

						Handles.DrawBezier (p0, p3, p1, p2, Color.cyan, null, 2f);
						p0 = p3;
					}
				}
			}
		}
	}
}
