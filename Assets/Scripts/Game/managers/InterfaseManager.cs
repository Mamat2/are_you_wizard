using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InterfaseManager : MonoBehaviour
{

    public GameObject pauseMenu;
    public GameObject DeadScreen;
    public GameObject VictoryScreen;
    public GameObject SpelBoardObj;
    public GameObject redRoomCounter;
    public GameObject blueRoomCounter;
    public GameObject questDesc;
    public bool menuFlag = true;
    //public bool FigtStatus = false;

    public GameObject pauseButn;
    //public bool pauseButtonFlag = true;
    // Start is called before the first frame update
    void Start()
    {
        GameManager.HeroeDeadEvt.AddListener(IsDead);
        GameManager.IsFigtEvt.AddListener(SpelBoard);
        GameManager.VinEvt.AddListener(VIN);
    }


    private void VIN()
    {
        VictoryScreen.SetActive(true);
    }


    void SpelBoard(bool figth)
    {
        SpelBoardObj.SetActive(figth);
        redRoomCounter.SetActive(!figth);
        blueRoomCounter.SetActive(!figth);
        questDesc.SetActive(!figth);

    }

    void IsDead()
    { 
        DeadScreen.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Escape))
        {
           
           closeOpenMenu();
            
        }

    }

    void closeOpenMenu()
    {
        GameManager.Pause(menuFlag);
        pauseButn.SetActive(!menuFlag);
        pauseMenu.SetActive(menuFlag);

        if (menuFlag)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1f;
        }



         menuFlag = !menuFlag;



    }

    public void SpellButton( int  number)
    {
        print (number); 
        BatleManager.InvokeSpell(number);

    }


    public void resumeGame()
    {
        closeOpenMenu();

    }
    public void pauseButton()
    {
        closeOpenMenu();
    }
    public void goToMenuButton(int scene)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(scene);
    }
    public void DeadgoToMenuButton(int scene)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(scene);
        GameManager.Refresh();
    }

}
