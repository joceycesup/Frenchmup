using UnityEngine;
using System.Collections;
using Spine.Unity;

public class TestingSpine : MonoBehaviour
{

	SkeletonAnimation skel;

	[SpineAttachment (currentSkinOnly: true, slotField: "Paupiere")]
	public string eyesOpen;

	[SpineAttachment (currentSkinOnly: true, slotField: "Paupiere")]
	public string eyesClosed;

	// Use this for initialization
	void Start ()
	{
		skel = GetComponent<SkeletonAnimation> ();
		StartCoroutine (Blink ());
		//ça ça gère la vitesse d'anim (1 pour 100%)
		skel.AnimationState.TimeScale = 0.75f;
		//On met une animation sur un track, 0 prenant la priorité sur toutes les autres animations, le nom de l'animation puis si ça loop ou pas
		skel.AnimationState.SetAnimation (1, "FlapAiles", true);
	}

	IEnumerator Blink ()
	{
		while (true) {
			//Voilà comment on change l'image attaché à un Slot particulier
			//Il faut rentrer le 
			skel.skeleton.SetAttachment ("Paupiere", eyesClosed);
			yield return new WaitForSeconds (3);
			skel.skeleton.SetAttachment ("Paupiere", eyesOpen);
			yield return new WaitForSeconds (3);
		}
	}

}
