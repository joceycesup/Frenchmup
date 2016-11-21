using UnityEngine;
using System.Collections.Generic;

public class SplineDecorator : MonoBehaviour {

	public BezierSpline spline;

	public int precision;
	public float overlap = 1.0f;
	//public float sideDisplacement = 1.0f;

	public bool lookForward;

	public GameObject[] items;
	public Vector3[] extents;
	private List<GameObject> links;

	void Awake () {
		links = new List<GameObject> ();
		extents = new Vector3[items.Length];
		for (int i = 0; i < items.Length; ++i) {
			extents [i] = items [i].GetComponent<SpriteRenderer> ().bounds.extents * overlap;
		}
	}

	void Start () {
		if (precision <= 0 || items == null || items.Length == 0 || overlap <= 0.0f) {
			return;
		}

		float t = 0.0f;
		float d = 0.0f;
		//Vector3 displacement = new Vector3 ();
		int i = 0;
		while (t <= 1.0f) {
			GameObject item = Instantiate (items [i]);
			links.Add (item);
			Vector3 position = spline.GetPoint (t);
			if (lookForward) {
				Quaternion tmpRot = Quaternion.FromToRotation (Vector3.up, spline.GetDirection (t));
				//tmpRot = new Quaternion (tmpRot.z, 0, tmpRot.x, tmpRot.w);
				item.transform.rotation = tmpRot;
				//Debug.Log (t.ToString ("F2") + " : " + spline.GetVelocity (t).magnitude);
				//item.transform.LookAt (position + spline.GetDirection (t));
				//Debug.Log (item.transform.rotation.ToString ());
			}
			item.transform.position = position;// + Quaternion.AngleAxis (item.transform.rotation.eulerAngles.z, Vector3.back) * displacement;
			//Debug.Log (t.ToString ("F2") + " : " + Quaternion.AngleAxis (item.transform.rotation.eulerAngles.z, Vector3.back) * displacement);
			//Debug.Log (item.transform.rotation.eulerAngles);

			d = extents [i].y;
			float tmp = t;
			//Vector3 dCurve = new Vector3 ();
			for (int j = 0; j < precision; ++j) {
				Vector3 velocity = spline.GetVelocity (tmp);
				//dCurve += Vector3.Normalize (velocity);
				tmp += (d / velocity.magnitude) / precision;
			}/*
			displacement.x = (2f * t - 1f);
			displacement.x *= displacement.x;
			displacement.x = 1f - displacement.x;
			displacement.x *= (dCurve.magnitude / ((float)precision)) * sideDisplacement * extents [i].x * Mathf.Sign (Vector3.Dot (Vector3.Normalize (spline.Get2DNormal2 (tmp)), Vector3.Normalize (dCurve)));//*/
			t = tmp;
			//item.transform.localPosition = item.transform.localPosition + item.transform.TransformPoint (displacement);

			i++;
			i %= items.Length;
			item.transform.parent = spline.transform;
		}//*/
	}
}