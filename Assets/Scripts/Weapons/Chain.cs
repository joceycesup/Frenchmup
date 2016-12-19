using UnityEngine;
using System.Collections;

public class Chain : MonoBehaviour {
	public GameObject player1;
	public GameObject player2;
	public GameObject links;

	private BezierSpline spline;
	private float length;
	public float retractSpeed = 0.5f;
	public float turnRate = 90f;
	private GameObject transformObject;

	public float damage = 2f;
	private bool linksActive = true;

	void Awake () {
		transformObject = new GameObject ("TransformObject");
		transformObject.transform.parent = gameObject.transform;
		spline = GetComponent<BezierSpline> ();
		links.SetActive (false);
		spline.points [0] = player1.transform.position - gameObject.transform.position;
		spline.points [3] = player2.transform.position - gameObject.transform.position;
		spline.points [1] = spline.points [2] = (player1.transform.position + player2.transform.position) / 2f;
		length = Vector3.Distance (player1.transform.position, player2.transform.position) / 2f;
	}

	void Update () {
		if (IngameTime.pause)
			return;
		bool tmpActive = linksActive;
		linksActive = player1.GetComponent<Player> ().isActiveAndEnabled && player2.GetComponent<Player> ().isActiveAndEnabled && (player1.GetComponent<Player> ().state != player2.GetComponent<Player> ().state) && player1.GetComponent<Player> ().CheckAbility (Player.Ability.Chain) && player2.GetComponent<Player> ().CheckAbility (Player.Ability.Chain);
		if (!linksActive) {
			links.SetActive (false);
		}
		if (!linksActive)
			return;
		transform.parent.position = Vector3.zero;
		float tmpLength = Vector3.Distance (player1.transform.position, player2.transform.position) / 2f;
		if (tmpLength > length) {
			length = tmpLength;
		} else {
			length = Mathf.Max (tmpLength, length - retractSpeed * IngameTime.deltaTime);
		}
		Vector3 p1pos = spline.points [0] == player1.transform.position ? spline.points [1] : player1.transform.position;
		Vector3 p2pos = spline.points [3] == player2.transform.position ? spline.points [2] : player2.transform.position;
		if (player1.transform.position != spline.points [0] || tmpLength != length) {
			Vector3 tmp1 = spline.GetVelocity (0.005f);
			float angle = Vector3.Angle (spline.points [0] - p1pos, tmp1);
			transformObject.transform.rotation = Quaternion.Euler (Vector3.forward * Mathf.Min (angle, turnRate * IngameTime.deltaTime) * (Vector3.Cross (spline.points [0] - p1pos, tmp1).z > 0f ? -1f : 1f));
			spline.points [1] = player1.transform.position + transformObject.transform.TransformDirection (Vector3.Normalize (spline.points [1] - spline.points [0])) * length;
			spline.points [0] = player1.transform.position;
		}
		if (player2.transform.position != spline.points [3] || tmpLength != length) {
			Vector3 tmp2 = spline.GetVelocity (0.995f);
			float angle = Vector3.Angle (spline.points [3] - p2pos, tmp2);
			transformObject.transform.rotation = Quaternion.Euler (Vector3.forward * Mathf.Min (angle, turnRate * IngameTime.deltaTime) * (Vector3.Cross (spline.points [3] - p2pos, tmp2).z > 0f ? 1f : -1f));
			spline.points [2] = player2.transform.position + transformObject.transform.TransformDirection (Vector3.Normalize (spline.points [2] - spline.points [3])) * length;
			spline.points [3] = player2.transform.position;
		}
		links.SetActive (tmpActive && linksActive);
	}
}
