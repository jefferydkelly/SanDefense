using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour {

	public float speed = 1;
	public Vector3 point0;

	Vector3 waveSize;

	// Use this for initialization
	void Start () {
		point0 = transform.position;
	}

	public IEnumerator RollTide(int level) {
		float zDif = Random.Range (2, Mathf.Min(level + 3, 6.5f)) * 2;
		waveSize = transform.position + new Vector3(0, 0, zDif);
		yield return StartCoroutine (MoveForward());
		yield return StartCoroutine (MoveBackwards());

		if (level > 1) {
			Grid.TheGrid.ClearRocks ();
			Grid.TheGrid.ScatterRocks ();
		}
	}

	IEnumerator MoveForward() {
		while (transform.position.z < waveSize.z) {
			transform.position += new Vector3 (0, 0, Time.deltaTime);
			yield return null;
		}
	}

	IEnumerator MoveBackwards() {
		while (transform.position.z > point0.z) {
			transform.position -= new Vector3 (0, 0, Time.deltaTime);
			yield return null;
		}
	}
}

