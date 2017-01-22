using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour {

	public float speed = 1;
	public Vector3 point0;

	Vector3 waveSize;
	Vector3 startScale;
	// Use this for initialization
	void Start () {
		point0 = transform.position;
		startScale = transform.localScale;
	}

	IEnumerator MoveForward() {
		while (transform.position.z < waveSize.z) {
			float scale = (transform.position.z - point0.z) / (waveSize.z - point0.z);
			transform.position += new Vector3 (0, 0, Time.deltaTime * speed);
			transform.localScale = new Vector3 (startScale.x, startScale.y * (1 - scale * 0.9f), startScale.z);
			yield return null;
		}
	}

	IEnumerator MoveBackwards() {
		while (transform.position.z > point0.z) {
			float scale = (transform.position.z - waveSize.z) / (point0.z - waveSize.z);
			transform.position -= new Vector3 (0, 0, Time.deltaTime * speed);
			transform.localScale = new Vector3 (startScale.x, startScale.y * (0.1f + scale * 0.9f), startScale.z);
			yield return null;
		}
	}
}

