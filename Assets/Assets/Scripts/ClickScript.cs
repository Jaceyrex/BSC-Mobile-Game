using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClickScript : MonoBehaviour
{

    public bool tap;
    public bool held;
    public Transform prefab = null;

    private bool foodExists = false;

    Vector2 screenPos = Vector2.zero;
    Vector3 worldPos = Vector3.zero;


    // Use this for initialization
    void Start()
    {

    }

    Transform instance = null;

    public void SpawnFood()
    {
        if (!held)
        {
            if (instance != null)
            {
                Destroy(instance.gameObject);
            }
            instance = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
            instance.GetComponent<ParticleSystem>().Stop();
        }
    }

    public void SpawnToy()
    {
        if (!held)
        {
            if (instance != null)
            {
                Destroy(instance.gameObject);
            }
            instance = Instantiate(prefab, new Vector3(0, 0, 0), Quaternion.identity);
            instance.GetComponent<ParticleSystem>().Stop();
        }
    }

    // Update is called once per frame
    public void Update()
    {
        

        //sets the screenpos variable to be where the click / touch is
        if (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.WindowsPlayer)
        {
            screenPos = Input.mousePosition;
            worldPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 15f));
        }
        else if (Application.platform == RuntimePlatform.Android)
        {
            if (Input.touchCount > 0)
            {
                screenPos = Input.GetTouch(0).position;
                worldPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 15f));
            }
        }

        //if the button is held
        //if (held)
        //{
        //    worldPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 15f));
        //}

        //if the instance isn't empty.
        if (instance != null)
        {
            if (held)
            {
                instance.GetComponent<Rigidbody>().useGravity = false;
                //worldPos = Camera.main.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 15f));
                worldPos.y = 6;
                instance.transform.position = worldPos;
            }
        }
    }

    //public bool Tap
    //{
    //    get
    //    {
    //        return tap;
    //    }
    //}

    public void SetHeld(bool isHeld)
    {
        if (!isHeld)
        {
            instance.GetComponent<Rigidbody>().useGravity = true;
        }
        held = isHeld;
    }
}