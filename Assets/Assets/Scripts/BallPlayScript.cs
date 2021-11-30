using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallPlayScript : MonoBehaviour {

    public float playTimer;
    public bool playable;
    public int playNum; //number of times the ball will be hit before it becomes unplayable

    private float ticker;

	// Use this for initialization
	void Start () {
        playable = true;
    }
	
	// Update is called once per frame
	void Update () {

        if (transform.position.y < 0)
        {
            Destroy(gameObject);
        }
        if (playNum == 0)
        {
            playable = false;
        }

        if (!playable)
        {
            ticker += Time.deltaTime;
            if (ticker >= playTimer)
            {
                playNum = Random.Range(1, 3);
                playable = true;
                ticker = 0.0f;
            }
        }
	}
}