using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratePath : MonoBehaviour {

    public int amountOfPoints;
    public GameObject point;

	// Use this for initialization
	void Start () {

        for (int i = 0; i < amountOfPoints; i++)
        {
            GameObject newPoint = Instantiate(point, new Vector3(Random.RandomRange(0, 10), Random.RandomRange(0, 10), Random.RandomRange(0, 10)), Quaternion.identity) as GameObject;

            newPoint.name = "point" + i.ToString();
            newPoint.transform.parent = gameObject.transform;
        }
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
