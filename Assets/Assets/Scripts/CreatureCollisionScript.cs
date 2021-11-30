using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreatureCollisionScript : MonoBehaviour {

    public GameObject food;
    public GameObject toy;

    // Use this for initialization
    void Start () {

	}
	
	// Update is called once per frame
	void Update () {
        CheckFood();
        CheckToy();
	}

    //Check if food exists and set the food variable to be the GameObject with the tag food
    private void CheckFood()
    {
        food = GameObject.FindGameObjectWithTag("Food");
    }
    //check if objects with the toy tag exist then make them the referenced object Toy
    private void CheckToy()
    {
        toy = GameObject.FindGameObjectWithTag("Toy");
    }

    void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Food")
        {
            if (!food.GetComponent<FoodScript>().GetDying())
            {
                transform.parent.GetComponent<GameControl>().hunger += 5;
                //transform.GetComponent<MoveScript>().SetSpeed(1.5f);
                food.GetComponent<FoodScript>().SetDying(true);
            }
            Debug.Log("Collided WITH FOOD");
        }
        if (col.gameObject.tag == "Toy")
        {
            //Force added to make it seem like the object has been thrown istead  of just bumped into
            toy.GetComponent<Rigidbody>().AddForce(transform.up * 2.5f, ForceMode.Impulse);
            toy.GetComponent<Rigidbody>().AddForce(transform.forward * Random.Range(-5.0f, 5.0f), ForceMode.Impulse);
            toy.GetComponent<Rigidbody>().AddForce(transform.right * Random.Range(-5.0f, 5.0f), ForceMode.Impulse);

            if (toy.GetComponent<BallPlayScript>().playable) //Checks if the toy is set to playable, adds to the mood if it is and sets a new timer for when it will next become playable
            {
                toy.GetComponent<BallPlayScript>().playNum--;
                transform.parent.GetComponent<GameControl>().mood += 5;
                toy.GetComponent<ParticleSystem>().Emit(5);
                toy.GetComponent<BallPlayScript>().playTimer = Random.Range(15.0f, 30.0f);
            }
            Debug.Log("COLLIDED WITH TOY");
        }
    }
}