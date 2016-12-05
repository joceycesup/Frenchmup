using UnityEngine;
using System.Collections;

public class IngameTime : MonoBehaviour {

	public static bool pause {
		get;
		private set;
	}

	public static IngameTime ingameTime {
		get;
		private set;
	}
	public static float deltaTime {
		get { return ingameTime!=null?ingameTime._deltaTime:Time.deltaTime; }
	}
	public static float time {
		get { return ingameTime!=null?ingameTime._time:Time.time; }
	}
	public static float globalTime {
		get { return ingameTime!=null?ingameTime._globalTime:Time.time; }
	}

	private float _time;
	private float _globalTime;
	private float _deltaTime;
	private float _timeFactor = 1.0f;
	private float _pauseFactor = 1.0f;
	public GameObject pauseObject;

	void Awake () {
		pause = false;
		ingameTime = this;
	}

	void Update () {
		if (Input.GetButtonDown ("Pause"))
			TogglePause ();
		if (!pause) {
			_deltaTime = Time.deltaTime * _timeFactor;
			_time += _deltaTime;
			_globalTime += Time.deltaTime;
		}
	}

	public static void MultiplyFactor (float factor) {
		ingameTime.MultiplyFactorInstance (factor);
	}

	public static void TogglePause () {
		pause = !pause;
		ingameTime.pauseObject.SetActive (pause);
		if (pause) {
			ingameTime._pauseFactor = ingameTime._timeFactor;
			ingameTime._timeFactor = 0f;
		} else {
			ingameTime._timeFactor = ingameTime._pauseFactor;
		}
	}

	private void MultiplyFactorInstance (float factor) {
		if (factor > 0f) {
			_timeFactor *= factor;
			if (_timeFactor < 1f) {
				AkSoundEngine.PostEvent ("bullet_time_on", GameSettings.game_settings);
			} else {
				AkSoundEngine.PostEvent ("bullet_time_off", GameSettings.game_settings);
			}
		}
	}
}
