using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveScript : MonoBehaviour {

    float wanderingTime = 0.0f; //Amount of time that the object has currently been moving towards the destination location for
    public float moveSpeed;
    public Vector3 destinationLoc; // Destination location that the game object will move towards

    const float wanderLimit = 2.5f;

    //declared gameObjects for the boundaries
    public GameObject obj_Left;
    public GameObject obj_Right;
    public GameObject obj_Bot;
    public GameObject obj_Top;
    public GameObject food; //food game object
    public GameObject toy; //toy game object to play with

    // Use this for initialization
    void Start ()
    {
        //Sets the starting location for the creature before the update method is called for the first time to ensure it has a location to move to.
        destinationLoc = GetNewLocation(obj_Left, obj_Right, obj_Bot, obj_Top);
        moveSpeed = 5.0f;
    }
	
	// Update is called once per frame
	void Update () {

        float step = moveSpeed * Time.deltaTime;
        wanderingTime = wanderingTime + Time.deltaTime; //Adding the time since the last frame to the wandering time

        OutOfBoundsCheck();
        if (!GetComponent<CreatureTapScript>().GetFocus())
        {
            if (food != null && food.transform.position.y < 1) //checks if the food exists in the game world
            {
                ChaseObject(food);
            }
            else if (toy != null && toy.GetComponent<BallPlayScript>().playable) //Checks if a toy exists and is set to playable, else ignores the ball until playable is true
            {
                ChaseObject(toy);
            }
            else if (wanderingTime >= wanderLimit) // If the wandering time is greater than 5 seconds
            {
                destinationLoc = GetNewLocation(obj_Left, obj_Right, obj_Bot, obj_Top);
                wanderingTime = 0.0f; //reset the wandering time to 0 to reset the cycle
            }
            transform.position = Vector3.MoveTowards(transform.position, destinationLoc, step);
        }
        CheckFood();
        CheckToy();
    }

    private Vector3 GetNewLocation(GameObject left, GameObject right, GameObject bot, GameObject top) //Method used for defining a new Vector3 location
    {
        Vector3 location = new Vector3(Random.Range(left.transform.position.x, right.transform.position.x), this.transform.position.y, Random.Range(bot.transform.position.z, top.transform.position.z));

        //Sets the current transform, as well as all model children to look at the new location
        transform.LookAt(location);
        transform.GetChild(0).LookAt(location);
        transform.GetChild(1).LookAt(location);
        transform.GetChild(2).LookAt(location);
        transform.GetChild(3).LookAt(location);
        transform.GetChild(4).LookAt(location);

        moveSpeed = Random.Range(2.5f, 7.5f); //Sets a random movespeed each time a new location is set

        //Sets the states of the animation controllers to moving, to ensure that after leaving the focus camera, it plays the running animation
        transform.GetChild(0).GetComponent<Animator>().SetBool("moving", true);
        transform.GetChild(1).GetComponent<Animator>().SetBool("moving", true);
        transform.GetChild(2).GetComponent<Animator>().SetBool("moving", true);
        transform.GetChild(3).GetComponent<Animator>().SetBool("moving", true);
        transform.GetChild(4).GetComponent<Animator>().SetBool("moving", true);

        //Sets the animation controller speed to the new movement speed divided by 10 (to give a smaller number within the bounds of the animation controller
        transform.GetChild(0).GetComponent<Animator>().SetFloat("movement", moveSpeed / 10);
        transform.GetChild(1).GetComponent<Animator>().SetFloat("movement", moveSpeed / 10);
        transform.GetChild(2).GetComponent<Animator>().SetFloat("movement", moveSpeed / 10);
        transform.GetChild(3).GetComponent<Animator>().SetFloat("movement", moveSpeed / 10);
        transform.GetChild(4).GetComponent<Animator>().SetFloat("movement", moveSpeed / 10);

        Debug.Log("!FOODEXISTS");
        return location;
    }

    private void OutOfBoundsCheck() //Checks if the gameObject is out of bounds
    {
        if (transform.position.x < obj_Left.transform.position.x)
        {
            transform.position = new Vector3(obj_Left.transform.position.x, transform.position.y, transform.position.z);
        }// Left check    -
        else if (transform.position.x > obj_Right.transform.position.x)
        {
            transform.position = new Vector3(obj_Right.transform.position.x, transform.position.y, transform.position.z);
        }//Right check   -
        else if (transform.position.z < obj_Bot.transform.position.z)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, obj_Bot.transform.position.z);
        }//bottom check    -
        else if (transform.position.z > obj_Top.transform.position.z)
        {
            transform.position = new Vector3 (transform.position.x, transform.position.y, obj_Top.transform.position.z);
        }//top check      -     Entire if statement checks if the game object is out of bounds
    }
    
    private void ChaseObject (GameObject obj)//Method used to make the creature chase the passed object
    {
        destinationLoc.x = obj.transform.position.x;
        destinationLoc.y = this.transform.position.y;
        destinationLoc.z = obj.transform.position.z;
        transform.LookAt(destinationLoc);
        transform.GetChild(0).LookAt(destinationLoc);
        transform.GetChild(1).LookAt(destinationLoc);
        transform.GetChild(2).LookAt(destinationLoc);
        transform.GetChild(3).LookAt(destinationLoc);
        transform.GetChild(4).LookAt(destinationLoc);
    }

    private void CheckFood()
    {
        food = GameObject.FindGameObjectWithTag("Food");
    }

    private void CheckToy()
    {
        toy = GameObject.FindGameObjectWithTag("Toy");
    }

    public void SetSpeed(float speed)
    {
        moveSpeed = speed;
    }
}