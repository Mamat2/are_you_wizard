using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System.Linq;

public class heroScript : MonoBehaviour
{

    private Vector3 m_camRot;
    private Transform m_camTransform;
    private Transform m_transform;
    float m_rotateSpeed =10;
    float m_movSpeed = 20;
    private Rigidbody _rb;

    private bool _isGrounded;
    public bool isFigth = false ;
    public bool isDead= false ;
    public bool isPause= false ;
     GameObject Enemy;
    public Transform EnemyTransform;

    bool flag = true;
    bool flag2 = true;

    bool playerTurn = true;

    public Text HeroeHPtext;
    public Text MpText;
    public Text BarrierText;
    public Text DarckBText;
    public Text LightBText;
    public Text ColdBText;
    public Text FireBText;
    public Text statsText;



    Camera cam;


     float MaxDurability = 10f;
    float MaxStrength = 10f;
  float MaxSpeed = 10f;
    float MaxMemory = 100f;
     float MaxEnergy = 10f;

    float MaxPhysicalResistance = 10;  //- физическое сопротивление
     float MaxDarknessResistance = 10;//- сопротивление тьме
  float MaxLightResistance = 10;//- сопротивление свету
     float MaxHeatResistance = 10;//- сопротивление теплу
     float MaxColdResistance = 10;//- сопротивление холоду


    float durabilityNow = 0f;
    float strengthNow = 0f;
    float speedNow = 0f;
    float memoryNow = 0f;
    float energyNow = 0f;
    float physicalResistanceNow = 10.0f;
    float darknessResistanceNow = 0.0f;
    float lightResistanceNow = 0.0f;
    float heatResistanceNow = 0.0f;
    float coldResistanceNow = 0.0f;

    float Barrier = 0f;
    float FireBarier = 0f;
    float ColdBarier = 0f;
    float DarknesBarier = 0f;
    float LigthBarier = 0f;

    float barierCurse = 0f;

     float MAX_HELS;
     float HelsNow;
     float MAX_MANA;
     float ManaNow;
    float HandDamage;

    private  Dictionary<ProgrammSpellElement, int> effectsDictionary = new Dictionary<ProgrammSpellElement, int>();

    //private Dictionary<>
    /*   public GameObject GameManager;
       public GameObject BatelManager;*/
    public float speed = 1.0f;

    bool unarmed = false;


    Transform TouchingTarget;

    public UnityEvent Touch;
    public UnityEvent<float> Hit;

    private bool isFalling;
    private float fallStartTime;


    //float HandDamage = 15 ;

    /*    public  const float MAX_HELS = 100;
        public  static float HelsNow = MAX_HELS;*/


    void Start()
    {
        // BatleManager.HeroeGetDamage.AddListener(ResiveD);
        BatleManager.GetDamageHeroe.AddListener(getDamage);
        GameManager.IsFigtEvt.AddListener(IsFigth);
        GameManager.PauseEvt.AddListener(pause);
        BatleManager.CastSpellEvt.AddListener(ResiveSpell);
        BatleManager.doStepEvt.AddListener(determineTurn);
        GameManager.RefreshEvt.AddListener(Refresh);
        GameManager.VinEvt.AddListener(IsVin);

        _rb = GetComponent<Rigidbody>();
        m_camTransform = Camera.main.transform;
        cam = Camera.main;
        m_transform = GetComponent<Transform>(); // ?????? ????? m_transform ?? ????? ????????? ???????? ?????
       
        InitStats();
        UpdateStatistics();


     }
    void IsVin()
    {
       isPause = true;
    }
    void pause(bool status)
    { 
        isPause = status;
    }
    void Refresh()
    {
        HelsNow = MAX_HELS;
    }


    // Update is called once per frame
    void Update()
    {
        //print("asdasd");
        if (!isPause && !isDead)
        {

            if (!isFigth)
            {
                Control();
            }
            else if (isFigth && !isDead && !isPause)
            {
                TargetEnemy();
                castSpel();
            }

       

            hitTarget();

            if (IsPlayerFalling())
            {
                if (!isFalling)
                {
                    StartFalling();
                }
                else if (Time.time - fallStartTime > 2f)
                {
                    GameManager.IsDead();
                    isDead = true;
                }
            }
            else
            {
                isFalling = false;
            }

        }

    
    }
    void determineTurn(string who)
    {

          if (who == "Heroe")
          {

            ManaNow = MAX_MANA;

            resiveCurse();
            CurseTracker();
            UpdateStatistics();

            playerTurn = true;
   
        }
        else
        {
            playerTurn = false;
        }
    
    }

    private void FindEnemyObject()
    {
        Enemy = GameObject.FindWithTag("Enemy");
        EnemyTransform = Enemy.transform;
        /*        print(Enemy.transform);
                if (Enemy == null)
                {
                    Debug.LogError("Enemy object not found!");
                }
                else
                {
                    Debug.Log("Enemy object found!");
                }*/
    }

    public void resiveCurse()
    {
        //print("ResivedCurse");
       // print("Herou resive spell");
        float damage = 0;

        float barrier = 0;
         foreach (var key in effectsDictionary.Keys)
        {
            print(key.type + " " + key.level + " " + key.value);
        }



        //  Dictionary<ProgrammSpellElement, int> tmpeffectsDictionary = new Dictionary<ProgrammSpellElement, int>(effectsDictionary);
        List<ProgrammSpellElement> BuffModifires = new List<ProgrammSpellElement>();
        List<ProgrammSpellElement> DebuffModifires = new List<ProgrammSpellElement>();
        foreach (var curse in effectsDictionary.Keys)
        {
            if (curse.type == "Damage")
            {
              
                damage += curse.value;

            }
            if (curse.type == "Barrier")
            {
                barrier += curse.value;
            }
            if (curse.type == "Darkness" || curse.type == "Ligth" || curse.type == "Fire" || curse.type == "Cold")
            {

                if (curse.value > 0)
                {
                    DebuffModifires.Add(curse);

                }
                else
                {
                    BuffModifires.Add(curse);
                }

            }
        }
        if (damage > 0)//наносим урон 
        {
            if (DebuffModifires.Count == 0)// если нет модификаторов
            {
                damage = Barrier - damage; //пробид щит или нет
                if (damage < 0) //да 
                {
                    damage = -damage;
                    getDamage(damage - damage / 100 * physicalResistanceNow);
                }
                else if (damage >= 0)//нет 
                {
                    Barrier = damage;//вычитаем урон из соотвествующего щита 
                }
            }
            else
            {
                foreach (var modifire in effectsDictionary.Keys)
                {
                    float TMPDMG;
                    switch (modifire.type)
                    {
                        case ("Darkness"):
                            //print("there");
                            TMPDMG = damage / DebuffModifires.Count * modifire.value;
                            TMPDMG = (DarknesBarier - TMPDMG) * 1f;
                            // print(damage);
                            if (TMPDMG < 0)
                            {
                                //  print("DDDD");
                                DarknesBarier = 0;
                                TMPDMG = TMPDMG * (-1);
                                getDamage(TMPDMG - TMPDMG / 100 * MaxDarknessResistance);


                            }
                            else
                            {
                                //print("CCCCC");
                                DarknesBarier = DarknesBarier - (DarknesBarier - TMPDMG);

                            }

                            //  getDamage(damage / modifires.Count - (damage * modifire.value / 100 * stats.darknessResistance));
                            break;
                        case "Ligth":
                            TMPDMG = damage / DebuffModifires.Count * modifire.value;
                            TMPDMG = (LigthBarier - TMPDMG) * 1f;
                            // print(damage);
                            if (TMPDMG < 0)
                            {
                                //  print("DDDD");
                                LigthBarier = 0;
                                TMPDMG = TMPDMG * (-1);
                                getDamage(TMPDMG - TMPDMG / 100 * MaxLightResistance);


                            }
                            else
                            {
                                //print("CCCCC");
                                LigthBarier = LigthBarier - (LigthBarier - TMPDMG);

                            }
                            // getDamage(damage / modifires.Count - (damage * modifire.value / 100 * stats.lightResistance));
                            break;
                        case "Cold":
                            TMPDMG = damage / DebuffModifires.Count * modifire.value;
                            TMPDMG = (ColdBarier - TMPDMG) * 1f;
                            // print(damage);
                            if (TMPDMG < 0)
                            {
                                //  print("DDDD");
                                ColdBarier = 0;
                                TMPDMG = TMPDMG * (-1);
                                getDamage(TMPDMG - TMPDMG / 100 * MaxColdResistance);


                            }
                            else
                            {
                                //print("CCCCC");
                                ColdBarier = ColdBarier - (ColdBarier - TMPDMG);

                            }
                            //getDamage(damage / modifires.Count - (damage * modifire.value / 100 * stats.coldResistance));
                            break;
                        case "Fire":
                            TMPDMG = damage / DebuffModifires.Count * modifire.value;
                            TMPDMG = (FireBarier - TMPDMG) * 1f;
                            // print(damage);
                            if (TMPDMG < 0)
                            {
                                //  print("DDDD");
                                FireBarier = 0;
                                TMPDMG = TMPDMG * (-1);
                                getDamage(TMPDMG - TMPDMG / 100 * MaxHeatResistance);


                            }
                            else
                            {
                                //print("CCCCC");
                                FireBarier = FireBarier - (FireBarier - TMPDMG);

                            }
                            //getDamage(damage / modifires.Count - (damage * modifire.value / 100 * stats.heatResistance));
                            break;
                    }
                }
            }

        }
        else if (damage < 0)
        {
            //print("периодическое исцеление ");
            getDamage(damage);
        }

        if (barrier > 0)
        {


            if (BuffModifires.Count == 0)
            {

                Barrier += barrier - (barrier / 100 * barierCurse);

            }
            else
            {

           
                foreach (var modifire in BuffModifires)
                {

                    switch (modifire.type)
                    {
                        case ("Darkness"):

                            DarknesBarier += barrier / BuffModifires.Count - (barrier / 100 * barierCurse);
                            break;
                        case "Ligth":
                            LigthBarier += barrier / BuffModifires.Count - (barrier / 100 * barierCurse);
                            break;
                        case "Cold":
                            ColdBarier += barrier / BuffModifires.Count - (barrier / 100 * barierCurse);
                            break;
                        case "Fire":
                            FireBarier += barrier / BuffModifires.Count - (barrier / 100 * barierCurse);
                            break;
                        default:
                        
                            break;

                    }

                }



            }
        }


        foreach (var key in effectsDictionary.Keys)
        {
         
            if (key.type == "ManaBurn")
            {
                ManaNow -= key.value * 5;
            }
            if (key.level == 0 || key.level == 1)
            {

                switch (key.type)
                {
                    case ("Durability"):
            
                        durabilityNow -= MaxDurability * 2.5F * (key.value / 100);
                        break;
                    case ("Strength"):

                        strengthNow -= MaxStrength * 2.5F * (key.value / 100);
                        break;

                    case ("Speed"):
                        speedNow -= MaxSpeed * 2.5F * (key.value / 100);

                        break;
                    case ("Energy"):

                        energyNow -= MaxEnergy * 2.5F * (key.value / 100);
                        break;

                    case ("Darkness"):
                        darknessResistanceNow -= MaxDarknessResistance * 2.5F * (key.value / 100);
                        break;

                    case ("Light"):
                        lightResistanceNow -= MaxLightResistance * 2.5F * (key.value / 100);

                        break;

                    case ("Cold"):                    
                        coldResistanceNow -= MaxColdResistance * 2.5F * (key.value / 100);
                        break;

                    case ("Fire"):

                        heatResistanceNow -= MaxHeatResistance * 2.5F * (key.value / 100);

                        break;

                    case ("Breakable"):
                   
                        physicalResistanceNow -= MaxPhysicalResistance * 2.5F * (key.value / 100);
                        break;
                    case ("Disarming"):
                        unarmed = true;

                        break;
                    case ("BarrierCurse"):
                        barierCurse += key.value;
                        break;
                    case ("Memory"):
                        memoryNow -= key.value * 4;
                        break;

                }

               // print("Pereopredelenie");
                key.level = 3;
            }
        }
    }

    void CurseTracker()
    {
        Dictionary<ProgrammSpellElement, int> tmpeffectsDictionary = new Dictionary<ProgrammSpellElement, int>(effectsDictionary);

        foreach (var curse in effectsDictionary.Keys)
        {
            tmpeffectsDictionary[curse] -= 1;
        }

        foreach (var key in effectsDictionary.Keys)
        {
            if (tmpeffectsDictionary[key] <= 0)
            {
                switch (key.type)
                {
                    case ("Durability"):
                        durabilityNow += MaxDurability * 2.5F * (key.value / 100);
                        break;
                    case ("Strength"):
                        strengthNow += MaxStrength * 2.5F * (key.value / 100);
                        break;
                    case ("Speed"):
                        speedNow += MaxSpeed * 2.5F * (key.value / 100);
                        break;
                    case ("Energy"):
                        energyNow += MaxEnergy * 2.5F * (key.value / 100);
                        break;
                    case ("Darkness"):
                        darknessResistanceNow += MaxDarknessResistance * 2.5F * (key.value / 100);
                        break;
                    case ("Light"):
                        lightResistanceNow += MaxLightResistance * 2.5F * (key.value / 100);
                        break;
                    case ("Cold"):
                        coldResistanceNow += MaxColdResistance * 2.5F * (key.value / 100);
                        break;
                    case ("Fire"):
                        heatResistanceNow += MaxHeatResistance * 2.5F * (key.value / 100);
                        break;
                    case ("Breakable"):
                        physicalResistanceNow += MaxPhysicalResistance * 2.5F * (key.value / 100);
                        break;
                    case ("Disarming"):
                        unarmed = false;
                        break;
                    case ("BarrierCurse"):
                        barierCurse -= key.value;
                        break;
                    case ("Memory"):
                        memoryNow += key.value * 4;
                        break;
                }
                tmpeffectsDictionary.Remove(key);
            }
        }

        effectsDictionary = tmpeffectsDictionary;
    }
    void ResiveSpell(string target, FinalSpel spell)
    {
        if (target == "all" || target == "SelfTarget")
        {
           // print("spell");
            float damage = 0;
            float barrier = 0;
            if (spell.duration == 0)
            {
                if (spell.ContainDamage())
                {
                    damage = spell.GetDamage().value;
                    if (!spell.buffDebuff)
                    {

                        if (spell.GetDamageModifire().Count == 0)
                        {
                            //print("kkkkkk");
                            damage = (Barrier - damage) * 1f;
                            // print(damage);
                            if (damage < 0)
                            {
                                //  print("DDDD");
                                Barrier = 0;
                                damage = damage * (-1);
                                getDamage(damage - damage / 100 * MaxPhysicalResistance);
                            }
                            else
                            {
                                //print("CCCCC");
                                Barrier = Barrier - (Barrier - damage);
                            }

                        }
                        else
                        {
                            float TMPDMG = 0;
                            List<ProgrammSpellElement> modifires = spell.GetDamageModifire();
                            foreach (var modifire in modifires)
                            {
                                switch (modifire.type)
                                {
                                    case ("Darkness"):
                                        //print("there");
                                        TMPDMG = damage / modifires.Count * modifire.value;
                                        TMPDMG = (DarknesBarier - TMPDMG) * 1f;
                                        // print(damage);
                                        if (TMPDMG < 0)
                                        {
                                            //  print("DDDD");
                                            DarknesBarier = 0;
                                            TMPDMG = TMPDMG * (-1);
                                            getDamage(TMPDMG - TMPDMG / 100 *MaxDarknessResistance);


                                        }
                                        else
                                        {
                                            //print("CCCCC");
                                            DarknesBarier = DarknesBarier - (DarknesBarier - TMPDMG);

                                        }

                                        //  getDamage(damage / modifires.Count - (damage * modifire.value / 100 * stats.darknessResistance));
                                        break;
                                    case "Ligth":
                                        TMPDMG = damage / modifires.Count * modifire.value;
                                        TMPDMG = (LigthBarier - TMPDMG) * 1f;
                                        // print(damage);
                                        if (TMPDMG < 0)
                                        {
                                            //  print("DDDD");
                                            LigthBarier = 0;
                                            TMPDMG = TMPDMG * (-1);
                                            getDamage(TMPDMG - TMPDMG / 100 * MaxLightResistance);


                                        }
                                        else
                                        {
                                            //print("CCCCC");
                                            LigthBarier = LigthBarier - (LigthBarier - TMPDMG);

                                        }
                                        // getDamage(damage / modifires.Count - (damage * modifire.value / 100 * stats.lightResistance));
                                        break;
                                    case "Cold":
                                        TMPDMG = damage / modifires.Count * modifire.value;
                                        TMPDMG = (ColdBarier - TMPDMG) * 1f;
                                        // print(damage);
                                        if (TMPDMG < 0)
                                        {
                                            //  print("DDDD");
                                            ColdBarier = 0;
                                            TMPDMG = TMPDMG * (-1);
                                            getDamage(TMPDMG - TMPDMG / 100 * MaxColdResistance);


                                        }
                                        else
                                        {
                                            //print("CCCCC");
                                            ColdBarier = ColdBarier - (ColdBarier - TMPDMG);

                                        }
                                        //getDamage(damage / modifires.Count - (damage * modifire.value / 100 * stats.coldResistance));
                                        break;
                                    case "Fire":
                                        TMPDMG = damage / modifires.Count * modifire.value;
                                        TMPDMG = (FireBarier - TMPDMG) * 1f;
                                        // print(damage);
                                        if (TMPDMG < 0)
                                        {
                                            //  print("DDDD");
                                            FireBarier = 0;
                                            TMPDMG = TMPDMG * (-1);
                                            getDamage(TMPDMG - TMPDMG / 100 * MaxHeatResistance);


                                        }
                                        else
                                        {
                                            //print("CCCCC");
                                            FireBarier = FireBarier - (FireBarier - TMPDMG);

                                        }
                                        //getDamage(damage / modifires.Count - (damage * modifire.value / 100 * stats.heatResistance));
                                        break;
                                    default:
                                        // Вариант по умолчанию, если modifire.type не соответствует ни одному из вариантов
                                        break;

                                }

                            }


                        }
                    }
                    else
                    {

                        getDamage(damage * (-1));
                    }
                }
                if (spell.ContainBarrier())
                {

                    barrier = spell.GetBarrier().value;

                    if (spell.GetDamageModifire().Count == 0)
                    {

                        Barrier += barrier - (barrier / 100 * barierCurse);
      


                    }
                    else
                    {

                        List<ProgrammSpellElement> modifires = spell.GetDamageModifire();
                        foreach (var modifire in modifires)
                        {
                            switch (modifire.type)
                            {
                                case ("Darkness"):
                                    //print("there");

                                    //print("AAA");
                                    DarknesBarier += barrier / modifires.Count - (barrier / 100 * barierCurse);
                                    break;
                                case "Ligth":
                                    LigthBarier += barrier / modifires.Count - (barrier / 100 * barierCurse);
                                    break;
                                case "Cold":
                                    ColdBarier += barrier / modifires.Count - (barrier / 100 * barierCurse);
                                    break;
                                case "Fire":
                                    FireBarier += barrier / modifires.Count - (barrier / 100 * barierCurse);
                                    break;
                                default:
                                    // Вариант по умолчанию, если modifire.type не соответствует ни одному из вариантов
                                    break;

                            }

                        }
                    }

                }
            }
            else
            {
                if (!spell.buffDebuff)
                {
                   //print("a1");

                    var tmp = spell.statuses;
                    foreach (var modifire in tmp)
                    {
                        var tmpModifire = new ProgrammSpellElement(modifire.type, 0, modifire.value);
                        if (ContainElement(modifire))
                        {

                            ProgrammSpellElement tmpKey = FindeTheSame(modifire);



                            // Если новое значение value больше текущего, обновляем значение и длительность
                            if (tmpModifire.value > tmpKey.value && (modifire.level == 0 && tmpKey.level == 0))
                            {
                                //   print("@1");
                                effectsDictionary.Remove(tmpKey);
                                effectsDictionary.Add(tmpModifire, spell.duration);
                            }
                            else

                            if (modifire.value == tmpKey.value && (modifire.level == 0 && tmpKey.level == 0))
                            {
                                //  print("@2");
                                effectsDictionary[tmpKey] += spell.duration;
                            }
                            else
                            {
                                effectsDictionary.Add(tmpModifire, spell.duration);
                            }

                        }
                        else
                        {
                            // Добавляем новый эффект в словарь
                            effectsDictionary.Add(tmpModifire, spell.duration);
                        }

                    }

                }
                else
                {
                   // print("Buff");
                    var tmp = spell.statuses;
                    foreach (var modifire in tmp)
                    {
                       // print("Bred");
                        var tmpModifire = new ProgrammSpellElement(modifire.type, 1, -modifire.value);
                        if (ContainElement(modifire))
                        {

                            ProgrammSpellElement tmpKey = FindeTheSame(modifire);



                            // Если новое значение value больше текущего, обновляем значение и длительность
                            if (tmpModifire.value < tmpKey.value && (modifire.level == 1 && tmpKey.level == 1))
                            {
                                //   print("@1");
                                effectsDictionary.Remove(tmpKey);
                                effectsDictionary.Add(tmpModifire, spell.duration);
                            }
                            else

                            if (modifire.value == tmpKey.value && (modifire.level == 1 && tmpKey.level == 1))
                            {
                                //  print("@2");
                                effectsDictionary[tmpKey] += spell.duration;
                            }
                            else
                            {
                                effectsDictionary.Add(tmpModifire, spell.duration);
                            }

                        }
                        else
                        {
                            // Добавляем новый эффект в словарь
                            effectsDictionary.Add(tmpModifire, spell.duration);
                        }

                    }
                }

            }
            UpdateStatistics();
        }

    }

    void purging()
    {
        Dictionary<ProgrammSpellElement, int> tmpeffectsDictionary = new Dictionary<ProgrammSpellElement, int>(effectsDictionary);

        foreach (var curse in effectsDictionary.Keys)
        {
            tmpeffectsDictionary[curse]=0;
        }
        effectsDictionary = tmpeffectsDictionary;
        CurseTracker();

    }

    public void IsFigth( bool f)
    {
        isFigth = f;
        if (!isFigth)
        {
            purging();
        }
        else
        {
            FindEnemyObject();
        }

    }


    void getDamage(float damage)
    {

        //HelsNow-=damage;
        HelsNow -= Mathf.Round(damage * 100F) / 100F;
        HelsNow = Mathf.Round(HelsNow * 100) / 100;
        UpdateStatistics();
        if (HelsNow <0)
        { 
            GameManager.IsDead();
            isDead = true;
          
        }

        if (HelsNow > MAX_HELS)
        { 
            HelsNow = MAX_HELS;
        }

        


    }

    IEnumerator Timer(float timeInSec)
    {
        yield return new WaitForSeconds(timeInSec);
        //сделать нужное
    }


    void InitStats()
    {
        durabilityNow = MaxDurability;
        strengthNow = MaxStrength;
        speedNow = MaxSpeed;
        memoryNow = MaxMemory;
        energyNow = MaxEnergy;
        physicalResistanceNow = MaxPhysicalResistance;
        darknessResistanceNow = MaxDarknessResistance;
        lightResistanceNow = MaxLightResistance;
        heatResistanceNow = MaxHeatResistance;
        coldResistanceNow = MaxColdResistance;
        MAX_HELS = MaxDurability * 10;
        HelsNow = MAX_HELS;
        MAX_MANA = MaxEnergy * 10;
        ManaNow = MAX_MANA;
        HandDamage = speedNow / 1F * strengthNow / 10f  ;
    }


    public void FigthStart()
    {
        UpdateStatistics();
        Enemy = GameObject.Find("Enemy");
        EnemyTransform = Enemy.transform;
        //print(Enemy.transform.position.x);
        
        //TargetEnemy();
      
    }

    void castSpel()
    {


        if ((Input.GetKeyUp(KeyCode.Alpha1) || Input.GetKeyUp(KeyCode.Alpha2) || Input.GetKeyUp(KeyCode.Alpha3)
        || Input.GetKeyUp(KeyCode.Alpha4) || Input.GetKeyUp(KeyCode.Alpha5) || Input.GetKeyUp(KeyCode.Alpha6)
        || Input.GetKeyUp(KeyCode.Alpha7) || Input.GetKeyUp(KeyCode.Alpha8) || Input.GetKeyUp(KeyCode.Alpha9)
        || Input.GetKeyUp(KeyCode.Alpha0)        
        ) && playerTurn)
        {
            int keyPressed = -1;
            if (Input.GetKeyUp(KeyCode.Alpha1))
            {
                keyPressed = 1;
            }
            else if (Input.GetKeyUp(KeyCode.Alpha2))
            {
                keyPressed = 2;
            }
            else if (Input.GetKeyUp(KeyCode.Alpha3))
            {
                keyPressed = 3;
            }
            if (Input.GetKeyUp(KeyCode.Alpha4))
            {
                keyPressed = 4;
            }
            else if (Input.GetKeyUp(KeyCode.Alpha5))
            {
                keyPressed = 5;
            }
            else if (Input.GetKeyUp(KeyCode.Alpha6))
            {
                keyPressed = 6;
            }


            if (keyPressed != -1)
            {
                BatleManager.InvokeSpell(keyPressed);
            
            }
        }


    }



    void TargetEnemy()
    {


        Vector3 targetDirection = EnemyTransform.position - transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(targetDirection);


        float rotationSpeed = 5f; 
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Плавный поворот камеры по вертикали
        float verticalRotationSpeed = 2f; 
        Vector3 targetEulerAngles = targetRotation.eulerAngles;
        Vector3 currentEulerAngles = m_camTransform.rotation.eulerAngles;
        float newVerticalAngle = Mathf.LerpAngle(currentEulerAngles.x, targetEulerAngles.x, verticalRotationSpeed * Time.deltaTime);
        m_camTransform.rotation = Quaternion.Euler(newVerticalAngle, targetEulerAngles.y, targetEulerAngles.z);
    


}

 

    void hitTarget()
    {

        Vector3 mousePos = Input.mousePosition;
        mousePos.z = 10f;
        mousePos = cam.ScreenToWorldPoint(mousePos);
        Debug.DrawRay(transform.position, mousePos - transform.position, Color.red);

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 100))
            {
                TouchingTarget =  hit.transform;
                //print(TouchingTarget); 
            }
        }
        if (Input.GetMouseButtonDown(0))
        {
          //  print("@Hit target" + playerTurn);
            if (TouchingTarget == EnemyTransform && playerTurn && !unarmed)
            {
            //    print("Change to false: " + playerTurn);
                playerTurn = false;
                // print("герой наносит урон");
                BatleManager.HeroeDD(HandDamage);
                BatleManager.DoStep("Enemy");
                
              //  print("After Change to false: " + playerTurn);
                /*  Hit?.Invoke(HandDamage);*/
            }
            else
            {
               // print("просто касание");
                Touch?.Invoke();
            }
        }


    }


   


    void Control()
    {
        
        float rh = Input.GetAxis("Mouse X");
        float rv = Input.GetAxis("Mouse Y");


        m_camRot.x -= rv * m_rotateSpeed;
        m_camRot.x = Mathf.Clamp(m_camRot.x, -90f, 90f); // Ограничиваем угол вращения от -90 до 90 градусов
        m_camRot.y += rh * m_rotateSpeed;

        /*m_camRot.x -= rv * m_rotateSpeed;
        m_camRot.y += rh * m_rotateSpeed;
*/
        m_camTransform.eulerAngles = m_camRot;
        Vector3 camrot = m_camTransform.eulerAngles;
        camrot.x = 0; camrot.z = 0;
        m_transform.eulerAngles = camrot;


        float xm = 0, ym = 0, zm = 0;


       

        if (Input.GetKey(KeyCode.W))
        {
            zm += m_movSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.S)) // ??????? ??????? S, ????? ????????????? ????
        {
            zm -= m_movSpeed * Time.deltaTime / 2;
        }
        if (Input.GetKey(KeyCode.A))
        {
            xm -= m_movSpeed * Time.deltaTime;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            xm += m_movSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.Space) && _isGrounded)//&& is_Ground&& _isGrounded
        {
           
            ym += 15 * m_movSpeed * Time.deltaTime;
        }



        //234234
        
        if (Input.GetKey(KeyCode.LeftShift))
        {
            if (flag)
            {
                m_movSpeed = m_movSpeed * 3;
                flag = false;
                flag2 = true;
            }
        }
        else
        {
            if (flag2)
            {
                m_movSpeed = m_movSpeed / 3;
                flag = true;
                flag2 = false;
            }
        }

        m_transform.Translate(new Vector3(xm, ym, zm), Space.Self);
    }

    public ProgrammSpellElement FindeTheSame(ProgrammSpellElement Modifire)
    {
        List<ProgrammSpellElement> Keys = effectsDictionary.Keys.ToList();

        foreach (var element in Keys)
        {
            if (Modifire.type == element.type)
            {
                return element;
            }

        }

        return null;


    }


    public bool ContainElement(ProgrammSpellElement Modifire)
    {
        List<ProgrammSpellElement> Keys = effectsDictionary.Keys.ToList();

        foreach (var element in Keys)
        {
            if (Modifire.type == element.type)
            {
                return true;
            }

        }

        return false;
    }


    public void UpdateStatistics()
    {
       // print("durabilityNow" + durabilityNow);
        MAX_HELS =durabilityNow * 10;
       // HelsNow = MAX_HELS;
        MAX_MANA = energyNow* 10;
        HandDamage = speedNow / 1F * strengthNow / 10f;
      //  ManaNow = MAX_MANA;
        MpText.GetComponent<Text>().text = ManaNow + " / " + MAX_MANA;
        HeroeHPtext.GetComponent<Text>().text = HelsNow + " / " + MAX_HELS;
        BarrierText.GetComponent<Text>().text = Barrier.ToString(); 
        DarckBText.GetComponent<Text>().text = DarknesBarier.ToString();
        LightBText.GetComponent<Text>().text =LigthBarier.ToString();
        ColdBText.GetComponent<Text>().text = ColdBarier.ToString(); 
        FireBText.GetComponent<Text>().text = FireBarier.ToString();
        statsText.GetComponent<Text>().text = "выносливость " + durabilityNow.ToString() + "\n" +
                                               "скорость " + speedNow.ToString() + "\n" +
                                               "сила " + strengthNow.ToString() + "\n" +
                                               "энергия" + energyNow.ToString() + "\n" +
                                               "память " + memoryNow.ToString() + "\n" +
                                               "сопротивленияя \n"  +
                                               "физ" + physicalResistanceNow.ToString() + "\n" +
                                               "тьме " + darknessResistanceNow.ToString() + "\n" +
                                               "холоду " + coldResistanceNow.ToString() + "\n" +
                                               "свету " + lightResistanceNow.ToString() + "\n" +
                                               "теплу " + heatResistanceNow.ToString()  ;

    }


    void StartFalling()
    {
        isFalling = true;
        fallStartTime = Time.time;
    }
    bool IsPlayerFalling()
    {
        // Получение текущей скорости игрока
        Vector3 velocity = _rb.velocity;

        // Проверка, падает ли игрок по оси Y
        if (velocity.y < -10f)
        {
            return true;
        }

        // Дополнительные условия проверки падения игрока
        // ...

        return false;
    }
    private void IsGroundedUpate(Collision collision, bool value)
    {
        if (collision.gameObject.tag == ("ground"))
        {
            _isGrounded = value;
        }
    }


}
