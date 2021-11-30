using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusCamPositionScript : MonoBehaviour {

    public Transform target; //The rarget that this script will follow

    // Use this for initialization
    void Start () {
        GetComponent<Camera>().enabled = false;
	}
	
	// Update is called once per frame
	void Update () {

        transform.position = new Vector3(target.transform.position.x, 1, target.transform.position.z - 3);
		
	}
}