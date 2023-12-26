using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EnemyTrigger : MonoBehaviour
{
    

    public UnityEvent OnPressed;

    public bool thisRoom = false; 



    private void OnTriggerExit(Collider colider)
    {
        // print("asdfsadf");
        if (!thisRoom)
        {
            GameManager.SpavnEnemy(this);
            //GameManager.Figth(true);
            thisRoom = true;
        } 
        //OnPressed.Invoke();


    }



    // Start is called before the first frame update
    void Start()
    {
       // GameManager.NextRoomEvt.AddListener(nextRoom);
    }

    void nextRoom(int number )
    {
        thisRoom = false;
    }
    // Update is called once per frame
    void Update()
    {
       // print("sdfsd");
    }
}
