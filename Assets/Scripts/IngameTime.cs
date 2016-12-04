using UnityEngine;
using System.Collections;

public class IngameTime : MonoBehaviour {
	public float maxCutoffFrequency = 5000f;

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
	}/*
	public static float factor {
		get { return _ingameTime!=null?_ingameTime._timeFactor:Time.deltaTime; }
		private set { if (_ingameTime != null) { _ingameTime._timeFactor = value; } }
	}//*/
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

	void Awake () {
		pause = false;
		ingameTime = this;
	}

	void Update () {
		if (!pause) {
			_deltaTime = Time.deltaTime * _timeFactor;
			_time += _deltaTime;
			_globalTime += Time.deltaTime;
		}
	}

	public static void MultiplyFactor (float factor) {
		ingameTime.MultiplyFactorInstance (factor);
	}

	private void MultiplyFactorInstance (float factor) {
		if (factor > 0f) {
			_timeFactor *= factor;
			if (_timeFactor < 1f) {
				AkSoundEngine.PostEvent ("bullet_time_on", GameSettings.game_settings);
				Debug.Log ("bellette time!!");
			} else {
				AkSoundEngine.PostEvent ("bullet_time_off", GameSettings.game_settings);
				Debug.Log ("bellette time au feu!!");
			}
			//AudioListener.volume = _timeFactor;
		}
	}
}
