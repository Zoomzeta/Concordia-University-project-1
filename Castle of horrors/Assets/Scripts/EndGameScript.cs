using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGameScript : MonoBehaviour
{
    public void returnToMainMenu()
    {
        SceneManager.LoadScene("Scenes/MainMenu");
    }
}
