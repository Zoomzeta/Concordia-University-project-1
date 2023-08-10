using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// Allows objects to be clicked and selected.

public class SelectionManager : MonoBehaviour
{
    public GameObject selected;
    public new Camera camera;
    public Text stats;

    

    // Update is called once per frame
    void Update()
    {
        
        if(Input.GetMouseButtonDown(0))
        {
            
            Ray ray = camera.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) 
            {
                selected = hit.transform.gameObject;
                Debug.Log("Hit "+ selected.name);

                if (selected.CompareTag("Party") || selected.CompareTag("Enemy"))
                {
                    string name = selected.GetComponent<Character>().PlayerName;
                    int level = selected.GetComponent<Character>().Level;
                    int currentHP = selected.GetComponent<Character>().CurrentHealth;
                    int maxHP = selected.GetComponent<Character>().Health;

                    stats.text = name + "\nLevel: " + level + "\nHP: " + currentHP + "/" + maxHP;
                }

                else
                {
                    stats.text="";
                }

            }
            else
            {
                selected = null;
                Debug.Log("Did not Hit"); 
            }
                
        }
    }
}
