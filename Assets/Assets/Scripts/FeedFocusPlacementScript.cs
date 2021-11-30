using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FeedFocusPlacementScript : MonoBehaviour {

	// Use this for initialization
	void Start () {

        transform.GetComponent<RectTransform>().position = new Vector2(Screen.width - 100, Screen.height / 7);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
