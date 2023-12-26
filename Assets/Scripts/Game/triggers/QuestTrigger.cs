using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuestTrigger : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public bool thisQuest= false;

    public int QuestNumber;


    private void OnTriggerEnter(Collider colider)
    {
        // print("asdfsadf");
        if (!thisQuest)
        {
            GameManager.AddQuest(QuestNumber);
            //GameManager.Figth(true);
            thisQuest = true;
            GameObject.Destroy(gameObject);
        }
        //OnPressed.Invoke();


    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
