using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimerManager : MonoBehaviour {

	static TimerManager instance = null;
	List<Timer> timers = new List<Timer> ();
	List<Timer> toRemove = new List<Timer> ();
	List<Timer> toAdd = new List<Timer> ();
	// Use this for initialization
	void Awake () {
		if (instance == null) {
			instance = this;
		} else {
			Destroy (gameObject);
		}
	}
	
	// Update is called once per frame
	void Update () {

		foreach (Timer t in toAdd) {
			timers.Add (t);
		}
		toAdd = new List<Timer> ();
		if (!GameManager.Instance.IsPaused) {
			float dt = Time.deltaTime;
			foreach (Timer t in timers) {
				if (t.Update (dt)) {
					toRemove.Add (t);
				}
			}
		}

		foreach (Timer t in toRemove) {
			timers.Remove (t);
		}
		toRemove = new List<Timer> ();
	}

	public static TimerManager Instance {
		get {
			return instance;
		}
	}

	public void AddTimer(Timer t) {
		toAdd.Add (t);
	}

	public void RemoveTimer(Timer t) {
		toRemove.Add (t);
	}
}

public class Timer {
	WaitDelegate waitingOn;
	float runTime;
	float curTime;
	public bool repeating = false;
	bool paused = false;
	public Timer(WaitDelegate wd, float time) {
		waitingOn = wd;
		runTime = time;
		curTime = runTime;
	}

	public bool Update(float dt) {
		if (!paused) {
			curTime -= dt;

			if (curTime <= 0) {
				waitingOn ();
				if (repeating) {
					curTime += runTime;
				}

				return !repeating;
			}
		}

		return false;
	}

	public void Pause() {
		paused = true;
	}

	public void Unpause() {
		paused = false;
	}
}
