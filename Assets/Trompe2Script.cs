using UnityEngine;
using System.Collections;
using Spine.Unity;

public class Trompe2Script : MonoBehaviour {

	SkeletonAnimation skel;

	void Start () {
		skel = GetComponent<SkeletonAnimation> ();
		skel.timeScale = 0.75f;
		skel.AnimationState.AddAnimation (0, "Testing", true, 0);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
