using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour {

    //The speed of the enemy
    public float speed;

    //The points for the path the enemy must follow
    //The point number that the enemy is currently on
    private GameObject[] points;
    private int pointNumber = 0;

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {

        //Get all of the points that are in the path
        points = GameObject.FindGameObjectsWithTag("Path");

        //Move the enemy towards the next pathpoint
        gameObject.transform.position = Vector3.MoveTowards(gameObject.transform.position, points[pointNumber].transform.position, Time.deltaTime * speed);

        //Test if the enemy is in the same location as the path point
        if (gameObject.transform.position == points[pointNumber].transform.position)
        {
            //Increase the point the enemy should be at
            pointNumber++;

            //!!!!!TMEPERARY!!!!!//
            //Test if the enemy has reached the last point
            //Go back to the first point the enemy started at
            if (pointNumber >= points.Length)
            {
                pointNumber = 0;
            }
        }
    }
}
