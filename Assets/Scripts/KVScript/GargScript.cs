using UnityEngine;
using System.Collections;

public class GargScript : MonoBehaviour {

	// Update is called once per frame
	void Update () {
		GameObject[] chars = GameObject.FindGameObjectsWithTag("Player");
		float tmpDist = 900;
		int choice = 0;
		for (int i = 0; i < chars.Length; i++) {
			if(Vector3.Distance(chars[i].transform.position, transform.position)<tmpDist){
				choice = i;
				tmpDist = Vector3.Distance(chars[i].transform.position, transform.position);
			}
		}
		Debug.Log (choice);
		Quaternion tmpRot = Quaternion.FromToRotation (Vector3.down, chars [choice].transform.position - transform.position);
		transform.rotation = tmpRot;
		Debug.Log (tmpRot.eulerAngles);
	}
}
