using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodScript : MonoBehaviour {

    public float lifeTime;
    public bool isDying;
    bool enableParticles;

	// Use this for initialization
	void Start () {
        lifeTime = 3;
        isDying = false;
        enableParticles = false;
	}
	
	// Update is called once per frame
	void Update () {

        if(lifeTime <= 0 || transform.position.y < 0) //destroys the game object when the lifetime is less than 0 or the object has fell lower than allowed
        {
            Destroy(gameObject);
        }
        if (isDying)
        {
            if (!enableParticles)
            {
                gameObject.GetComponent<ParticleSystem>().Play();
                enableParticles = true;
            }
            lifeTime -= Time.deltaTime; //lowers the lifetime by the deltatime
            transform.localScale -= new Vector3(0.0025f, 0.0025f, 0.0025f);

            //Checks if the food pellet is too small that it would begin to grow... Sounds weird but that's what happened.
            if( transform.localScale.x < 0 ||
                transform.localScale.y < 0 ||
                transform.localScale.z < 0)
            {
                transform.localScale = Vector3.zero; //Sets the scale transform to 0
            }
        }
        //if
	}

    public void SetDying(bool answer)
    {
        isDying = answer;
    }
    public bool GetDying()
    {
        return isDying;
    }
}
