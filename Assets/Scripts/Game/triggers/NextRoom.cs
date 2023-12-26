using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NextRoom : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] objectsToKeep;  

    public int number;
    void Start()
    {
        
    }
    bool vork = false;

    private void OnTriggerExit(Collider colider)
    {
        // print("next");
        if (!vork)
        {
            
            GameManager.nextRoom(this);
            //OnPressed.Invoke();
            vork = true;
        }
    }

        // Update is called once per frame
        void Update()
    {
        
    }
}
