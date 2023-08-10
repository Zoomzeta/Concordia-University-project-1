using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class changescene : MonoBehaviour
{
   //from main menu to go to the first scene
    public void gotomedieval()
    {
        SceneManager.LoadScene("medievalworld");
    }

    //from the medieval world to the 2nd scene
    public void gotodessert()
    {
        SceneManager.LoadScene("desertworld");
    }

    //from the desert world to the 3rd scene
    public void gotoscifi()
    {
        SceneManager.LoadScene("scifiworld");
    }

    //once the player got through the portal
    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Party")
        {
            Scene scene = SceneManager.GetActiveScene();
            if (scene.name == "medievalworld") {

                gotodessert();
            }
            if (scene.name == "desertworld")
            {
                gotoscifi();
            }
        }
    }
}
