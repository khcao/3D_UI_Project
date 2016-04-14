using UnityEngine;
using System.Collections;

public class SphereMove : MonoBehaviour {

    public GameObject camera;

	// Use this for initialization
	void Start () {
	    
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = camera.transform.position;
	}
}
