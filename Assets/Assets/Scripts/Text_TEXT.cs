using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Text_TEXT : MonoBehaviour {

    float age;
    public GameControl scriptRef = null;

    private Text ageText;

	// Use this for initialization
	void Start () {

        age = scriptRef.GetAge();
        ageText = GetComponent<UnityEngine.UI.Text>();

    }

    // Update is called once per frame
    void Update () {

        age = scriptRef.GetAge();
        ageText.text = age.ToString();
	}
}
