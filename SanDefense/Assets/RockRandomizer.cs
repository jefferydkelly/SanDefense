using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RockRandomizer : MonoBehaviour {

    public Material[] possibleMaterials;
    public Mesh[] meshes;

	// Use this for initialization
	void Start () {
        GetComponent<Renderer>().material = possibleMaterials[Random.Range(0, possibleMaterials.Length)];
        GetComponent<MeshFilter>().mesh = meshes[Random.Range(0, meshes.Length)];
        Destroy(this);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
