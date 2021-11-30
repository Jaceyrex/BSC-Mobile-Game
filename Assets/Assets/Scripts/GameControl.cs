using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class GameControl : MonoBehaviour
{

    public GameControl control;
    enum Forms { form1_Baby, form2_HappyHungry, form2_HappyFull, form2_SadHungry, form2_SadFull }
    Forms form;

    public int stage = 1; //Current stage of the Metamal
    public int mood;
    public int oldMood = 0; //Used when loading the game
    public int hunger;
    public int oldHunger = 0; //Used when loading the game
    public float age; //total age of the Metamal
    public float statTimer;
    public Material[] materials;
    /* happy - sad
     * 0-1 for Baby form
     * 2-3 for happyHungry form
     * 4-5 for HappyFull form
     * 6-7 for SadHungry form
     * 8-9 for SadFull form
     */

    private const float autoSaveTime = 15.0f;
    private float saveTicker;

    private Transform model;

    //private float reOpenTime; //Time at which the program was reopened.
    //private float missedTime; //Time that the app has been closed for.

    const float minuteInDelta = 60.0f; //1 minute
    const int growTimeInMins = 1;


    // Use this for initialization
    void Awake()
    {
        transform.GetChild(0).gameObject.SetActive(true); //Sets the initial model to that of the baby to ensure that there is always a model on show.
        transform.GetChild(0).GetChild(1).gameObject.SetActive(false);
        transform.GetChild(0).GetChild(2).gameObject.SetActive(false);
        transform.GetChild(0).GetChild(3).gameObject.SetActive(false);
        transform.GetChild(0).GetChild(4).gameObject.SetActive(false);

        if (control == null)
        {
            DontDestroyOnLoad(gameObject);
            control = this;
        }
        else if (control != this)
        {
            Destroy(gameObject);
        }
    }

    //Called when the object is enabled - used to load the file.
    private void OnEnable()
    {
        Debug.Log("enabled");
        LoadData();
    }
    private void OnApplicationFocus(bool focus)
    {
        Debug.Log("focused");
        LoadData();
    }


    //Called when app is suspended.
    private void OnApplicationPause(bool pause)
    {
        Debug.Log("app-paused");
        SaveData();
    }

    //Called when the object is disabled (can include the game being closed) - used to save the file
    private void OnDestroy()
    {
        Debug.Log("destroyed");
        SaveData();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 currentPos = transform.position;
        age = age + Time.deltaTime;
        statTimer = statTimer + Time.deltaTime;
        saveTicker = saveTicker + Time.deltaTime;

        //Ensures that the mood cannot go below it's upper and lower limits
        if (mood < 0)
            mood = 0;
        else if (mood > 100)
            mood = 100;

        //Ensures that the hunger cannot go below it's upper and lower limits
        if (hunger < 0)
            hunger = 0;
        else if (hunger > 100)
            hunger = 100;

        if (statTimer >= (minuteInDelta / 2)) //Lowers mood and hunger every 30 seconds
        {
            mood -= 5;
            hunger -= 5;
            statTimer = 0; //resets the timer
            CheckStage(false);
            MoodCheck();
        }

        if (saveTicker >= autoSaveTime)
        {
            Debug.Log("AUTOSAVED");
            SaveData();
            saveTicker = 0.0f;
        }
    }
    public float GetAge()
    {
        return age;
    }

    private void SaveData()
    {
        BinaryFormatter bf = new BinaryFormatter(); //creates a new binary formatter instance
        FileStream file = File.Open(Application.persistentDataPath + Path.DirectorySeparatorChar + "playerInfo.dat", FileMode.OpenOrCreate); //Opens a file

        PlayerData data = new PlayerData(); //creates a new instance of the PlayerData class called data
        //gives the data object the values of the ingame values for the stats
        data.stage = stage;
        data.mood = mood;
        data.oldMood = oldMood;
        data.hunger = hunger;
        data.oldHunger = oldHunger;
        data.age = age;
        data.closeTime = DateTime.Now;

        //Debug log text for testing
        Debug.Log("saved \n mood " + mood + "\n oldmood " + oldMood + "\n Data mood " + data.mood);

        //serialised and closes the file stream
        bf.Serialize(file, data);
        file.Close();


        PlayerPrefs.SetFloat("quitTime", System.DateTime.Now.Second);
    }
    private void LoadData()
    {
        if (File.Exists(Application.persistentDataPath + "/playerInfo.dat")) //Checks if the file exists, so it doesn't attmept to load a nonexistant file.
        {
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + Path.DirectorySeparatorChar +"playerInfo.dat", FileMode.Open);
            PlayerData data = (PlayerData)bf.Deserialize(file); //Deserialising the file, turning the binary into the actual information
            file.Close();

            //Sets the ingame values to those in the save file.
            stage = data.stage;
            mood = data.mood;
            oldMood = data.oldMood;
            hunger = data.hunger;
            oldHunger = data.oldHunger;

            //Adds the missed amount of time to the age of the creature.
            TimeSpan missedTime = DateTime.Now.Subtract(data.closeTime);
            age = data.age + (float)missedTime.TotalSeconds;

            //decreases the mood and hunger based on how much time has been missed.
            mood -= (int)missedTime.TotalMinutes / 3;
            hunger -= (int)missedTime.TotalMinutes / 3;

            CheckStage(true);

            //debug log messages for testing
            Debug.Log("loaded \n mood " + mood + "\n oldmood " + oldMood + "\n Data mood " + data.mood);
        }
    }

    //Checks the mood of the creature along with the stat ticker, changes material based on mood.
    private void MoodCheck()
    {
        switch (form)
        {
            case Forms.form1_Baby:
                if (mood > 50)
                {
                    transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Renderer>().material = materials[0];
                }
                else
                {
                    transform.GetChild(0).GetChild(0).GetChild(0).GetComponent<Renderer>().material = materials[1];
                }
                break;
            case Forms.form2_HappyHungry:
                if (mood > 50)
                {
                    transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Renderer>().material = materials[2];
                }
                else
                {
                    transform.GetChild(0).GetChild(1).GetChild(0).GetComponent<Renderer>().material = materials[3];
                }
                break;
            case Forms.form2_HappyFull:
                if (mood > 50)
                {
                    transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<Renderer>().material = materials[4];
                }
                else
                {
                    transform.GetChild(0).GetChild(2).GetChild(0).GetComponent<Renderer>().material = materials[5];
                }
                break;
            case Forms.form2_SadHungry:
                if (mood > 50)
                {
                    transform.GetChild(0).GetChild(3).GetChild(0).GetComponent<Renderer>().material = materials[6];
                }
                else
                {
                    transform.GetChild(0).GetChild(3).GetChild(0).GetComponent<Renderer>().material = materials[7];
                }
                break;
            case Forms.form2_SadFull:
                if (mood > 50)
                {
                    transform.GetChild(0).GetChild(4).GetChild(0).GetComponent<Renderer>().material = materials[8];
                }
                else
                {
                    transform.GetChild(0).GetChild(4).GetChild(0).GetComponent<Renderer>().material = materials[9];
                }
                break;
        }
    }
    private void CheckStage(bool onLoad)
    {
        if (onLoad)
        {
            switch (stage)
            {
                case 0: //Ensures that if there is an error and the stage is 0, it will always go up to stage 1
                    stage++;
                    break;
                case 1: //Used on loading to ensure the baby form is loaded if in stage 1.
                    transform.GetChild(0).GetChild(0).gameObject.SetActive(true);
                    model = transform.GetChild(0).GetChild(0);
                    break;
                case 2: //Used on loading for setting the correct model based on the current form
                    transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                    if (oldMood > 50 && oldHunger < 50)
                    {
                        form = Forms.form2_HappyHungry;
                        transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
                        model = transform.GetChild(0).GetChild(1);
                    }
                    else if (oldMood > 50 && oldHunger > 50)
                    {
                        form = Forms.form2_HappyFull;
                        transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
                        model = transform.GetChild(0).GetChild(2);
                    }
                    if (oldMood < 50 && oldHunger < 50)
                    {
                        form = Forms.form2_SadHungry;
                        transform.GetChild(0).GetChild(3).gameObject.SetActive(true);
                        model = transform.GetChild(0).GetChild(3);
                    }
                    else if (oldMood < 50 && oldHunger > 50)
                    {
                        form = Forms.form2_SadFull;
                        transform.GetChild(0).GetChild(4).gameObject.SetActive(true);
                        model = transform.GetChild(0).GetChild(4);
                    }
                    break;
                case 3: // unusued but allows for more stages to be added at a later date
                    break;
            }
            MoodCheck();
        }
        else
        {
            switch (stage)
            {
                case 0:
                    stage++;
                    break;
                case 1:
                    form = Forms.form1_Baby; //Ensures the form is the baby form for rendering.
                    if (age >= (minuteInDelta * growTimeInMins))
                    {
                        oldMood = mood;
                        oldHunger = hunger;
                        stage++;
                        transform.GetChild(0).GetChild(0).gameObject.SetActive(false);
                        if (mood > 50 && hunger < 50)
                        {
                            form = Forms.form2_HappyHungry;
                            transform.GetChild(0).GetChild(1).gameObject.SetActive(true);
                            model = transform.GetChild(0).GetChild(1);
                        }
                        else if (mood > 50 && hunger > 50)
                        {
                            form = Forms.form2_HappyFull;
                            transform.GetChild(0).GetChild(2).gameObject.SetActive(true);
                            model = transform.GetChild(0).GetChild(2);
                        }
                        if (mood < 50 && hunger < 50)
                        {
                            form = Forms.form2_SadHungry;
                            transform.GetChild(0).GetChild(3).gameObject.SetActive(true);
                            model = transform.GetChild(0).GetChild(3);
                        }
                        else if (mood < 50 && hunger > 50)
                        {
                            form = Forms.form2_SadFull;
                            transform.GetChild(0).GetChild(4).gameObject.SetActive(true);
                            model = transform.GetChild(0).GetChild(4);
                        }
                    }
                    break;
                case 2://Does nothing while in stage 2, allows for stages to be added later
                    break;
                case 3: // unusued but allows for more stages to be added at a later date
                    break;
            }
        }
    }
}

[Serializable]
class PlayerData
{
    public int stage;
    public int mood;
    public int oldMood;
    public int hunger;
    public int oldHunger;
    public float age;
    public DateTime closeTime;
}
