using UnityEngine;
using System.Collections;

public class IngameTime : MonoBehaviour {
	public float maxCutoffFrequency = 5000f;

	private static IngameTime _ingameTime;

	public static IngameTime ingameTime {
		get { return _ingameTime; }
	}
	public static float deltaTime {
		get { return _ingameTime!=null?_ingameTime._deltaTime:Time.deltaTime; }
	}/*
	public static float factor {
		get { return _ingameTime!=null?_ingameTime._timeFactor:Time.deltaTime; }
		private set { if (_ingameTime != null) { _ingameTime._timeFactor = value; } }
	}//*/
	public static float time {
		get { return _ingameTime!=null?_ingameTime._time:Time.time; }
	}

	private float _time;
	private float _deltaTime;
	private float _timeFactor = 1.0f;

	void Awake () {
		_ingameTime = this;
	}

	void Update () {
		_deltaTime = Time.deltaTime * _timeFactor;
		_time += _deltaTime;
	}

	public static void MultiplyFactor (float factor) {
		_ingameTime.MultiplyFactorInstance (factor);
	}

	private void MultiplyFactorInstance (float factor) {
		if (factor > 0f) {
			_timeFactor *= factor;
			if (_timeFactor < 1f) {
				gameObject.GetComponent<AudioLowPassFilter> ().cutoffFrequency = _timeFactor * maxCutoffFrequency;
				gameObject.GetComponent<AudioLowPassFilter> ().enabled = true;
			} else {
				gameObject.GetComponent<AudioLowPassFilter> ().enabled = false;
			}
			//AudioListener.volume = _timeFactor;
		}
	}
}
