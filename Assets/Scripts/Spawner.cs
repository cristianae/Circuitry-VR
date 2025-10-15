using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    // Add to component to create a spawner for it
    // To be interactable, interactors must be set to include Spawner and Components layer (6,7)
    // To use, add script to game object
    // Under Interactable Events -> First select entered -> Object is the gameObject, function to run is spawn() in Spawner, run at runtime + editor
    public void spawn()
    {
        if (this.enabled) {
            GameObject newCube = Instantiate(gameObject);
            newCube.layer = 6;
            this.enabled = false;
            gameObject.layer = 7;
        }
        //Instantiate(prefab, transform.position, transform.rotation);
    }
}
