using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusInteractionScript : MonoBehaviour {

    private Vector2 startPoint;
    private Vector2 endPoint;
    private float swipeDistance;

	// Use this for initialization
	void Start () {
        swipeDistance = Screen.width * 40 / 100; //swipeDistance is 40% of the screen width
	}
	
	// Update is called once per frame
	void Update ()
    {
        //Checks if the creature is currently being focused, does nothing if false
        if (gameObject.GetComponent<CreatureTapScript>().GetFocus()) 
        {
            if (Input.touchCount > 0)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began) //When the touch has began, set the start and end points to the position of the touch
                {
                    startPoint = Input.GetTouch(0).position;
                    endPoint = Input.GetTouch(0).position;
                }
                else if (Input.GetTouch(0).phase == TouchPhase.Moved) //If the touch has moved, updates the end point to the current touch position
                {
                    endPoint = Input.GetTouch(0).position;
                }
                else if (Input.GetTouch(0).phase == TouchPhase.Ended || Input.GetTouch(0).phase == TouchPhase.Canceled) //When the touch has ended or cancelled, finds out whether the touch is a swipe or a tap.
                {
                    endPoint = Input.GetTouch(0).position;

                    //Check if the distance of the touch is greater than the swipeDistance, if true, then it's a swipe
                    if (Mathf.Abs(endPoint.x - startPoint.x) > swipeDistance || Mathf.Abs(endPoint.y - startPoint.y) > swipeDistance)
                    {
                        Debug.Log("SWIPED");
                        gameObject.transform.GetChild(6).GetComponent<ParticleSystem>().Emit(5);
                        gameObject.GetComponentInParent<GameControl>().mood -= 5;
                    }
                    //Else it was a tap, not a swipe
                    else if (endPoint.x == startPoint.x && endPoint.y == startPoint.y)
                    {
                        Debug.Log("TAPPED");
                        gameObject.transform.GetChild(5).GetComponent<ParticleSystem>().Emit(5);
                        gameObject.GetComponentInParent<GameControl>().mood += 5;
                    }
                }
            }
        }
	}
}
