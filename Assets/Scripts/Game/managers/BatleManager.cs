using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class BatleManager : MonoBehaviour
{

    public static UnityEvent<float> GetDamageEnemy = new UnityEvent<float>();
    public static UnityEvent<float> GetDamageHeroe = new UnityEvent<float>();
    public static UnityEvent<int> InvokeSpellEvt = new UnityEvent<int>();
    public static UnityEvent<string ,FinalSpel> CastSpellEvt = new UnityEvent<string,FinalSpel>();
    public static UnityEvent< string> doStepEvt = new UnityEvent<string>(); 

    public static UnityEvent<GameObject, Collision> DockingRequestEvt = new UnityEvent<GameObject, Collision>();




    public GameObject Player;
    public GameObject Enemy;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }



    public static void HeroeDD(float damage)
    { 
        GetDamageEnemy.Invoke(damage);
    }
    public static void EnemyDD(float damage)
    {
                    
        GetDamageHeroe.Invoke(damage);
    }

    public static void InvokeSpell(int number)
    {
        // print(number);
        InvokeSpellEvt.Invoke(number);
    }

    public static void DockingRequest(GameObject whoConnects, Collision WitWhomConnectedCollision)
    {
        // print("managrer work");
        DockingRequestEvt.Invoke(whoConnects, WitWhomConnectedCollision);
    }

    public static void DoStep(string WhoDo )
    {
        //print("Manager@DoSteps");
        doStepEvt.Invoke(WhoDo);
    }

    public static void CastSpell(string target, FinalSpel spell)
    {
        CastSpellEvt.Invoke(target,spell);

    }
}
