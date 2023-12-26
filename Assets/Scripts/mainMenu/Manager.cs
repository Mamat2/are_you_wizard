using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Manager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void chageScene(int scene)
    {
       // GameManager.Refresh();
        SceneManager.LoadScene(scene);

    }

    public void Exit() 
    {
        Application.Quit();
    }


    public void onClick()
    {
       // print("sdfad");
    }
}
