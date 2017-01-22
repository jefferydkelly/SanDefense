using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour {
	[SerializeField]
	Vector3 min;
	[SerializeField]
	Vector3 max;
	[SerializeField]
	[Range(1, 20)]
	float speed;
	
	// Update is called once per frame
	void Update () {
		Vector3 dir = new Vector3 ((Input.GetKey (KeyCode.W) ? 1 : 0) - (Input.GetKey (KeyCode.S) ? 1 : 0), 0,
			              (Input.GetKey (KeyCode.A) ? 1 : 0) - (Input.GetKey (KeyCode.D) ? 1 : 0));

		dir.Normalize ();
		dir *= speed;
		bool q = Input.GetKey (KeyCode.Q);
		bool e = Input.GetKey (KeyCode.E);
		if (q && !e) {
			Camera.main.transform.RotateAround (Vector3.up, Time.deltaTime);
		} else if (e) {
			Camera.main.transform.RotateAround (Vector3.up, -Time.deltaTime);
		}
		Vector3 newPos = transform.position + dir * Time.deltaTime;
		newPos.x = Mathf.Clamp (newPos.x, min.x, max.x);
		newPos.y =  Mathf.Clamp (newPos.y, min.y, max.y);
		newPos.z = Mathf.Clamp (newPos.z, min.z, max.z);
		transform.position = newPos;
	}
}
