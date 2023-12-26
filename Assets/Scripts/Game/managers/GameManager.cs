using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{

    public static UnityEvent<bool> IsFigtEvt = new UnityEvent<bool>();
    public static UnityEvent HeroeDeadEvt = new UnityEvent();
    public static UnityEvent<bool> PauseEvt = new UnityEvent<bool>();
    public static UnityEvent <NextRoom,int> NextRoomEvt = new UnityEvent<NextRoom,int>();
    public static UnityEvent <int> questEvt = new UnityEvent<int>();
    public static UnityEvent RefreshEvt = new UnityEvent();
    public static UnityEvent <EnemyTrigger> SpavnEvt = new UnityEvent<EnemyTrigger>();
    public static UnityEvent VinEvt = new UnityEvent();
    public static UnityEvent <int,int> ChangeRoomNumberEvt = new UnityEvent<int, int>();
    public static UnityEvent<Dictionary<string, int>> UpadteQuestEvt = new UnityEvent<Dictionary<string, int>>();


    static float distanceFromTrigger = 10f;
    public List<GameObject> QuestItems;

    static Dictionary<int, int> roomNumber = new Dictionary<int, int>()
    {
        { 1,0 },
        { 2,0 },
    };





    // Start is called before the first frame update
    public GameObject pauseMenu;
    //public bool menuFlag = true;

    public GameObject RoomFloor;
    public GameObject Enemy;

   // static public GameObject enemyPrefab;
    //GameObject ProtoEnemyClone;

    public GameObject Player;


    public UnityEvent FigthStarted;
    //public bool isFigth = false;

    Dictionary <string,int> trackedQuests = new Dictionary<string,int>();

    public void addQuestToTrackedQuests(int questNumber)
    {
        if (questNumber == 1 && !trackedQuests.ContainsKey("killingSpree"))
        {
            trackedQuests.Add("killingSpree" , 0 );
            UpadteQuestEvt.Invoke(trackedQuests);

        }
        if (questNumber == 2 && !trackedQuests.ContainsKey("RedRooms"))
        {
            trackedQuests.Add("RedRooms", 0);
            trackedQuests["RedRooms"] = roomNumber[2];
            UpadteQuestEvt.Invoke(trackedQuests);
        }
    
    }

    public static void Figth(bool IsFigt)
    {
        //SpavnEnemy();

        IsFigtEvt.Invoke(IsFigt);

    }
    public static void Pause(bool flag)
    {
        PauseEvt.Invoke(flag);
    }
    public static void IsDead()
    { 
        HeroeDeadEvt.Invoke();
    }
    public static void nextRoom(NextRoom number)
    {
        if (roomNumber[1] >=0 && roomNumber[2] >=0)
        {
            NextRoomEvt.Invoke(number, 0);
        }
       // UpadteQuestEvt.Invoke(trackedQuests);
    }


    public static void AddQuest(int questNumber)
    { 
        questEvt.Invoke(questNumber);

    }
    public static void Refresh()
    {
        RefreshEvt.Invoke();
    }

    public static void SpavnEnemy(EnemyTrigger trigger)
    {
        SpavnEvt.Invoke(trigger);

    }

    void spavn(EnemyTrigger trigger)
    {
        // enemyPrefab = Enemy;
            Vector3 triggerPosition = trigger.transform.position;

        if (roomNumber[1] >= 0 && roomNumber[2] == 1  && !trackedQuests.ContainsKey("RedRooms"))
        {
            Vector3 newPosition = new Vector3(triggerPosition.x, triggerPosition.y, triggerPosition.z) + trigger.transform.forward * distanceFromTrigger;
            GameObject enemy = Instantiate(Enemy, newPosition, trigger.transform.rotation);
            EnemyStats SimpleEnemy = new SecondEnemy();
            enemy.GetComponent<EnemyScript>().initEnemy(SimpleEnemy);

            newPosition = new Vector3(triggerPosition.x + 10, triggerPosition.y + 1.5F, triggerPosition.z + 10) + trigger.transform.forward * distanceFromTrigger;

            GameObject questItem = Instantiate(QuestItems[1], newPosition, trigger.transform.rotation);

        } 
        else if (roomNumber[1] >=0 && roomNumber[2] >= 0)
        {
            Vector3 newPosition = new Vector3(triggerPosition.x, triggerPosition.y, triggerPosition.z) + trigger.transform.forward * distanceFromTrigger;
            GameObject enemy = Instantiate(Enemy, newPosition, trigger.transform.rotation);
            EnemyStats SimpleEnemy = new SimpleEnemy();
            enemy.GetComponent<EnemyScript>().initEnemy(SimpleEnemy);

/*           newPosition = new Vector3(triggerPosition.x +10, triggerPosition.y + 1.5F , triggerPosition.z+10 ) + trigger.transform.forward * distanceFromTrigger;

            GameObject questItem = Instantiate(QuestItems[1],newPosition, trigger.transform.rotation);*/

        }
        IsFigtEvt.Invoke(true);
    }


    void RefreshManager()
    {
        roomNumber = new Dictionary<int, int>(){
        { 1,0 },
        { 2,0 },};

        trackedQuests = new Dictionary<string, int>();

    }


    void Start()
    {
        SpavnEvt.AddListener(spavn);
        questEvt.AddListener(addQuestToTrackedQuests);
        RoomController.RoomNumberEvt.AddListener(resiveRoomNumber);
        IsFigtEvt.AddListener(killingSpreeCounter);
        HeroeDeadEvt.AddListener(RefreshManager);
        AssignIndestructibleTagToChildren();
        
    }


    void AssignIndestructibleTagToChildren()
    {
       // print("Worck");
        GameObject[] allObjects = FindObjectsOfType<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.tag == "indestructible")
            {
                if (obj.transform.childCount > 0)
                {
                       AssignIndestructibleTagToChildrenRecursive(obj.transform);
                }
            }
        }
    }
    void AssignIndestructibleTagToChildrenRecursive(Transform parent)
    {
        foreach (Transform child in parent)
        {
            if (child.gameObject.tag!= "MainCamera")
            {
                child.gameObject.tag = "indestructible";
                AssignIndestructibleTagToChildrenRecursive(child);
            }
            
        }
    }



    void Update()
    {

        if (trackedQuests.ContainsKey("killingSpree"))
        {
            if (trackedQuests["killingSpree"] > 2)
            {
                VinEvt.Invoke();
                RefreshManager();
            }
        }

        if (trackedQuests.ContainsKey("RedRooms"))
        {
            if (trackedQuests["RedRooms"] >=2)
            {
                VinEvt.Invoke();
                RefreshManager() ;
            }
        }
        /*        if (Input.GetKeyUp(KeyCode.Escape))
                {
                    pauseMenu.SetActive(menuFlag);          
                    Pause.Invoke(menuFlag);
                    menuFlag = !menuFlag;
                }*/

    }

    void killingSpreeCounter(bool successfulbattle)
    {
        if (!successfulbattle && trackedQuests.ContainsKey("killingSpree"))
        {
            trackedQuests["killingSpree"] += 1;
            print("Killing Speee" +trackedQuests["killingSpree"]);
            UpadteQuestEvt.Invoke(trackedQuests);
        }

    }

    void resiveRoomNumber(int one , int two)
    {
        roomNumber[1] = one ;    
        
        roomNumber[2] = two ;

        if (trackedQuests.ContainsKey("RedRooms"))
        {
            trackedQuests["RedRooms"] = roomNumber[2];
        }
       
        

       // print(roomNumber[1] + " " + roomNumber[2]);
       ChangeRoomNumberEvt.Invoke(roomNumber[1], roomNumber[2]);
        UpadteQuestEvt.Invoke(trackedQuests);
    }

    public void Touch()
    { 
      
    }

    // Update is called once per frame


}
