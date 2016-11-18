using UnityEngine;
using System.Collections;

public class IngameTime : MonoBehaviour {
	private static IngameTime _ingameTime;

	public static IngameTime ingameTime {
		get { return _ingameTime; }
	}
	public static float deltaTime {
		get { return _ingameTime!=null?_ingameTime._deltaTime:Time.deltaTime; }
	}
	public static float factor {
		get { return _ingameTime!=null?_ingameTime._timeFactor:Time.deltaTime; }
		set { if (_ingameTime != null) { _ingameTime._timeFactor = value; } }
	}
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
}
