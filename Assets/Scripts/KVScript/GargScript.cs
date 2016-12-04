using UnityEngine;
using System.Collections;

public class GargScript : MonoBehaviour {

	public float RotationSpeed = 1f;
	public bool Aiming;
	public Quaternion tmpRotation;

	// Update is called once per frame
	void Update () {
		if (Aiming) {
			GameObject[] chars = GameObject.FindGameObjectsWithTag ("Player");
			float tmpDist = float.MaxValue;
			int choice = 0;
			for (int i = 0; i < chars.Length; i++) {
				if (Vector3.Distance (chars [i].transform.position, transform.position) < tmpDist) {
					choice = i;
					tmpDist = Vector3.Distance (chars [i].transform.position, transform.position);
				}
			}
			//Debug.Log (choice);
			Quaternion targetRot = Quaternion.FromToRotation (Vector3.down, chars [choice].transform.position - transform.position);
			transform.rotation = Quaternion.RotateTowards (transform.rotation, targetRot,RotationSpeed);
		}
		//Debug.Log (tmpRot.eulerAngles);
	}
}
