using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureTapScript : MonoBehaviour {

    bool focused;

    public Camera mainCam; //The main camera of the scene
    public Camera focusCam; //the Focus camera
    public GameObject toyButton; //gameobject for the toy button
    public GameObject foodButton; //GameObject for the food button
    public GameObject unfocusButton; //Gameobject for the button to reset the focus

	// Use this for initialization
	void Start () {

        focused = false;
        unfocusButton.SetActive(false); //Ensures the unfocus button is inactive on start
	}
	
	// Update is called once per frame
	void Update () {
        if (!focused)
        {
            if ((Input.touchCount > 0) && (Input.GetTouch(0).phase == TouchPhase.Began))
            {
                Ray raycast = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit raycastHit;
                if (Physics.Raycast(raycast, out raycastHit))
                {
                    Debug.Log("Something Hit");
                    if (raycastHit.collider.name == "Creature")
                    {
                        focused = true;

                        //Sets all the models to face the correct direction to ensure nothing is missed.
                        transform.GetChild(0).eulerAngles = new Vector3(0, 180, 0);
                        transform.GetChild(1).eulerAngles = new Vector3(0, 180, 0);
                        transform.GetChild(2).eulerAngles = new Vector3(0, 180, 0);
                        transform.GetChild(3).eulerAngles = new Vector3(0, 180, 0);
                        transform.GetChild(4).eulerAngles = new Vector3(0, 180, 0);

                        //Sets the animation states for the controllers to be that moving is false, so the idle animation is played
                        transform.GetChild(0).GetComponent<Animator>().SetBool("moving", false);
                        transform.GetChild(1).GetComponent<Animator>().SetBool("moving", false);
                        transform.GetChild(2).GetComponent<Animator>().SetBool("moving", false);
                        transform.GetChild(3).GetComponent<Animator>().SetBool("moving", false);
                        transform.GetChild(4).GetComponent<Animator>().SetBool("moving", false);


                        mainCam.enabled = false; //disabled main camera
                        focusCam.enabled = true; //enables focus camera

                        //Sets the feed and toy buttons to be inactive and the unfocus button to be active
                        toyButton.SetActive(false);
                        foodButton.SetActive(false);
                        unfocusButton.SetActive(true);

                        Debug.Log("Creature clicked");
                    }
                }
            }
        }
    }

    public bool GetFocus()
    {
        return focused;
    }
    public void SetFocus(bool answer)
    {
        focused = answer;
        if (answer == false)
        {
            //Sets the animation states for the controllers to be that moving is true, so the run animation is played
            transform.GetChild(0).GetComponent<Animator>().SetBool("moving", true);
            transform.GetChild(1).GetComponent<Animator>().SetBool("moving", true);
            transform.GetChild(2).GetComponent<Animator>().SetBool("moving", true);
            transform.GetChild(3).GetComponent<Animator>().SetBool("moving", true);
            transform.GetChild(4).GetComponent<Animator>().SetBool("moving", true);

            mainCam.enabled = true; //enables the main camera
            focusCam.enabled = false; //disables the focus camera
            toyButton.SetActive(true);
            foodButton.SetActive(true);
            unfocusButton.SetActive(false);
        }
    }
}