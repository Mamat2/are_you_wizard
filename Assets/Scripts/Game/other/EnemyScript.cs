using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
//using UnityEditor.Experimental.GraphView;
using System.Linq.Expressions;
using System.Linq;
using UnityEngine.PlayerLoop;
using System.Data;
using System.Threading;
using System.ComponentModel.Design;
using System.Runtime.CompilerServices;

public class EnemyScript : MonoBehaviour
{

    public GameObject Enemy;
    GameObject Player;
    public GameObject RoomFloor;

    public float MAX_HELS;
    public float HelsNow;
    public float MAX_MANA;
    public float ManaNow;

    public EnemyStats stats = null;

    private Dictionary<ProgrammSpellElement, int> EnemyEffectsDictionary = new Dictionary<ProgrammSpellElement, int>();
    Dictionary<ProgrammSpellElement, int> alreadyAppliedEffects = new Dictionary<ProgrammSpellElement, int>();

    bool unarmed = false;

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

    float cursePower = 2.5f;


    public TMP_Text HPText;
    public TMP_Text HPText2;


    float Barrier = 0f;
    float FireBarier = 0f;
    float ColdBarier = 0f;
    float DarknesBarier = 0f;
    float LigthBarier = 0f;
    

    float barierCurse = 0f;

    public TMP_Text FireBarierText;
    public TMP_Text DarcknessBarierTxet;
    public TMP_Text ColdBarierText;
    public TMP_Text LigthBarierText;
    public TMP_Text BarierText;
    public TMP_Text StatsText;
    public TMP_Text ResistanseText;


    bool isSpavned = false;


    public void initEnemy(EnemyStats enemy)
    {
        stats = new EnemyStats();
        stats = enemy;
        durabilityNow = stats.durability;
        strengthNow = stats.strength;
        speedNow = stats.speed;
        memoryNow = stats.memory;
        energyNow = stats.energy;

        physicalResistanceNow = stats.physicalResistance;
        darknessResistanceNow = stats.darknessResistance;
        lightResistanceNow = stats.lightResistance;
        heatResistanceNow = stats.heatResistance;
        coldResistanceNow = stats.coldResistance;
        MAX_MANA = stats.energy * 10;
        MAX_HELS = stats.durability * 10;
        //       HelsNow = stats.maxHP;
        HelsNow = MAX_HELS;
        ManaNow = MAX_MANA;
        UpdateStatistics();
    }


    // Start is called before the first frame update
    void Start()
    {
        BatleManager.GetDamageEnemy.AddListener(getDamage);
        BatleManager.CastSpellEvt.AddListener(ResiveSpell);
        GameManager.IsFigtEvt.AddListener(IsFight);
        //  GameManager.NextRoomEvt.AddListener(nextRoom);
        BatleManager.doStepEvt.AddListener(determineTurn);
        FindPlayerObject();

    }

    public void Update()
    {


        FollowHero();

    }
    private void FindPlayerObject()
    {
        Player = GameObject.Find("Player");

    }
    public void determineTurn(string who)
    {
        if (who == "Enemy")
        {
            TimeTostep();
        }

    }




    void IsFight(bool f)
    {
        if (!isSpavned)
        {

        }

    }

    int MooveStep = 0;



    public void enemyResiveCurse()
    {
        float damage = 0;

        float barrier = 0;
        //print("enemyResiveCurse");
        Dictionary<ProgrammSpellElement, int> tmpeffectsDictionary = new Dictionary<ProgrammSpellElement, int>(EnemyEffectsDictionary);

        List<ProgrammSpellElement> BuffModifires = new List<ProgrammSpellElement>();
        List<ProgrammSpellElement> DebuffModifires = new List<ProgrammSpellElement>();
        foreach (var curse in EnemyEffectsDictionary.Keys)
        {
            if (curse.type == "Damage")
            {
                // print("увелисить урон на " + curse.value);
                damage += curse.value;

            }
            if (curse.type == "Barrier")
            {
                barrier += curse.value;
            }
            if (curse.type == "Darkness" || curse.type == "Ligth" || curse.type == "Fire" || curse.type == "Cold")
            {
                //bool added = false;
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
                foreach (var modifire in EnemyEffectsDictionary.Keys)
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
                                getDamage(TMPDMG - TMPDMG / 100 * stats.darknessResistance);


                            }
                            else
                            {
                                //print("CCCCC");
                                DarknesBarier = DarknesBarier - (DarknesBarier - TMPDMG);

                            }

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
                                getDamage(TMPDMG - TMPDMG / 100 * stats.lightResistance);


                            }
                            else
                            {

                                LigthBarier = LigthBarier - (LigthBarier - TMPDMG);

                            }
                 
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
                                getDamage(TMPDMG - TMPDMG / 100 * stats.coldResistance);


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
                                getDamage(TMPDMG - TMPDMG / 100 * stats.heatResistance);


                            }
                            else
                            {
                                //print("CCCCC");
                                FireBarier = FireBarier - (FireBarier - TMPDMG);

                            }
                     
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
            // barrier = spell.GetBarrier().value;

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
                            //print("there");

                            //print("AAA");
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
                            // Вариант по умолчанию, если modifire.type не соответствует ни одному из вариантов
                            break;

                    }

                }



            }
        }

        foreach (var key in EnemyEffectsDictionary.Keys)
        {
            // print("EnemyCurse " + key.type + " " + key.level + " " + key.value);
            if (key.type == "ManaBurn")
            {
                ManaNow -= key.value * 5;
            }
            if (key.level != 2)
            {
                //     print("curse " + key.type);
                switch (key.type)
                {
                    case ("Durability"):
                        durabilityNow -= stats.durability*cursePower * (key.value / 100);
                        break;
                    case ("Strength"):

                        strengthNow -= stats.strength * cursePower * (key.value / 100);
                        break;

                    case ("Speed"):
                        speedNow -= stats.speed * cursePower * (key.value / 100);

                        break;
                    case ("Energy"):

                        energyNow -= stats.energy * cursePower * (key.value / 100);
                        break;

                    case ("Darkness"):
                        darknessResistanceNow -= stats.darknessResistance * cursePower * (key.value / 100);
                        break;

                    case ("Light"):
                        lightResistanceNow -= stats.lightResistance * cursePower * (key.value / 100);

                        break;

                    case ("Cold"):
                        // код для обработки эффекта Cold
                        coldResistanceNow -= stats.coldResistance * cursePower * (key.value / 100);

                        break;

                    case ("Fire"):
                        // код для обработки эффекта Fire
                        heatResistanceNow -= stats.heatResistance * cursePower * (key.value / 100);

                        break;

                    case ("Breakable"):
                        //   print("aaaa");
                        physicalResistanceNow -= stats.physicalResistance * cursePower * (key.value / 100);
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

               
                key.level = 2;
            }
        }

        EnemyEffectsDictionary = tmpeffectsDictionary;
    }
    void CurseTracker()
    {
        Dictionary<ProgrammSpellElement, int> tmpeffectsDictionary = new Dictionary<ProgrammSpellElement, int>(EnemyEffectsDictionary);
        foreach (var curse in EnemyEffectsDictionary.Keys)
        {
            tmpeffectsDictionary[curse] -= 1;

        }

        foreach (var key in EnemyEffectsDictionary.Keys)
        {
            if (tmpeffectsDictionary[key] <= 0)
            {
                //     print("curse " + key.type);
                switch (key.type)
                {
                    case ("Durability"):
                        durabilityNow += stats.durability * cursePower * (key.value / 100);
                        break;
                    case ("Strength"):

                        strengthNow += stats.strength * cursePower * (key.value / 100);
                        break;
                    case ("Speed"):
                        //   print("aaaa");
                        speedNow += stats.speed * cursePower * (key.value / 100);
                        break;
                    case ("Energy"):

                        energyNow += stats.energy * cursePower * (key.value / 100);
                        break;

                    case ("Darkness"):
                        darknessResistanceNow += stats.darknessResistance * cursePower * (key.value / 100);
                        break;
                    case ("Light"):
                        lightResistanceNow += stats.lightResistance * cursePower * (key.value / 100);

                        break;

                    case ("Cold"):
                        // код для обработки эффекта Cold
                        coldResistanceNow += stats.coldResistance * cursePower * (key.value / 100);

                        break;

                    case ("Fire"):
                        // код для обработки эффекта Fire
                        heatResistanceNow += stats.heatResistance * cursePower * (key.value / 100);

                        break;

                    case ("Breakable"):
                        //   print("aaaa");
                        physicalResistanceNow += stats.physicalResistance * cursePower * (key.value / 100);
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
        EnemyEffectsDictionary = tmpeffectsDictionary;
    }


    public void TimeTostep()
    {
       
        UpdateStatistics();
        ManaNow = MAX_MANA;
      
        enemyResiveCurse();
       CurseTracker();
        UpdateStatistics();


        if (MooveStep >= stats.actionSequence.Count)
        {
            MooveStep = 0;
        }
        if (stats.actionSequence.Count != 0)
        {
            if (stats.actionSequence[MooveStep] == "HandDamage")
            {
                if (!unarmed)
                {
                    BatleManager.EnemyDD(speedNow / 1F * strengthNow / 10f);
                }
            }
            else
            {
                float randomValue = UnityEngine.Random.Range(0f, 100f);

                if (randomValue <= memoryNow)
                {
                    FinalSpel Spell = stats.Spels[stats.actionSequence[MooveStep]];
                    if (Spell.manaCost <= ManaNow)
                    {
                        BatleManager.CastSpell(Spell.target, Spell);
                    }
                    else
                    {
                        getDamage((ManaNow - Spell.manaCost) * (-1) / 10f);
                        BatleManager.CastSpell(Spell.target, Spell);

                    }
                 
                    ManaNow -= Spell.manaCost;
                }
                UpdateStatistics();
            }
            MooveStep++;
        }
        BatleManager.DoStep("Heroe");
    }


    void purging()
    {
        Dictionary<ProgrammSpellElement, int> tmpeffectsDictionary = new Dictionary<ProgrammSpellElement, int>(EnemyEffectsDictionary);

        foreach (var curse in EnemyEffectsDictionary.Keys)
        {
            tmpeffectsDictionary[curse] = 0;
        }
        EnemyEffectsDictionary = tmpeffectsDictionary;
        CurseTracker();

    }

    void ResiveSpell(string target, FinalSpel spell)
    {
        if (target == "all" || target == "EnemyTargeting")
        {

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
                                getDamage(damage - damage / 100 * stats.physicalResistance);
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
                                            getDamage(TMPDMG - TMPDMG / 100 * stats.darknessResistance);


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
                                            getDamage(TMPDMG - TMPDMG / 100 * stats.lightResistance);


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
                                            getDamage(TMPDMG - TMPDMG / 100 * stats.coldResistance);


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
                                            getDamage(TMPDMG - TMPDMG / 100 * stats.heatResistance);


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
                        //print("barierCurse" + barierCurse);
                        //print(barrier - barrier / 100 * barierCurse);
                        Barrier += barrier - (barrier / 100 * barierCurse);
                        //print(barrier);


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
               

                    var tmp = spell.statuses;
                    foreach (var modifire in tmp)
                    {
                        var tmpModifire = new ProgrammSpellElement(modifire.type, 0, modifire.value);
                        if (ContainElement(modifire))
                        {
                            
                            ProgrammSpellElement tmpKey = FindeTheSame(modifire);



                          
                            if (tmpModifire.value > tmpKey.value && (modifire.level == 0 && tmpKey.level == 0))
                            {
                          
                                EnemyEffectsDictionary.Remove(tmpKey);
                                EnemyEffectsDictionary.Add(tmpModifire, spell.duration);
                            }
                            else

                            if (modifire.value == tmpKey.value && (modifire.level == 0 && tmpKey.level == 0))
                            {
                             
                                EnemyEffectsDictionary[tmpKey] += spell.duration;
                            }
                            else
                            {
                                EnemyEffectsDictionary.Add(tmpModifire, spell.duration);
                            }

                        }
                        else
                        {
                            // Добавляем новый эффект в словарь
                            EnemyEffectsDictionary.Add(tmpModifire, spell.duration);
                        }
                    }

                }
                else
                {
                    //print("Buff");
                    var tmp = spell.statuses;
                    foreach (var modifire in tmp)
                    {
                       

                    }

                   

                }

            }
            UpdateStatistics();
        }




    }




    void UpdateStatistics()
    {
        // print("Statistick Updated");
        MAX_MANA = energyNow * 10;
        MAX_HELS = durabilityNow * 10;


        if (HelsNow > MAX_HELS)
        {
            HelsNow = MAX_HELS;
        }
        if (ManaNow > MAX_MANA)
        {
            ManaNow = MAX_MANA;
        }
        if (ManaNow < 0)
        {
            ManaNow = MAX_MANA;
        }

        BarierNow();
        ColdBarierNow();
        FireBarierNow();
        DarknesBarierNow();
        LigthBarierNow();
        hpNow();
        mpNow();

        ResistanseText.text = "физ р \n" + Convert.ToString(physicalResistanceNow) +
                             "\nтемное спр \n" + Convert.ToString(darknessResistanceNow) +
                             "\nсветлое спр \n" + Convert.ToString(lightResistanceNow) +
                             "\nспр теплу \n" + Convert.ToString(heatResistanceNow) +
                             "\nспр холоду \n" + Convert.ToString(coldResistanceNow);

        StatsText.text = "прочность " + Convert.ToString(durabilityNow) +
                            "\nскорость " + Convert.ToString(speedNow) +
                            "\nсила" + Convert.ToString(strengthNow) +
                            "\nэнергия " + Convert.ToString(energyNow) +
                            "\nпамять " + Convert.ToString(memoryNow);


    }

    

    bool HaveCurse()
    {
        List<ProgrammSpellElement> Keys = EnemyEffectsDictionary.Keys.ToList();

        List<string> effects = new List<string> {
            "Energy","Durability","Strength",
             "Memory","Speed",
             "Breakable","Disarming",
             "Darkness","Ligth",
             "Cold","Fire",
             "BarrierCurse","ManaBurn" };

        foreach (var key in Keys)
        {
            if (effects.Contains(key.type))
            {
                return true;
            }

        }


        return false;
    }



    void getDamage(float damage)
    {

        HelsNow -= Mathf.Round(damage * 100f) / 100f;
        if (HelsNow > MAX_HELS)
        {
            HelsNow = MAX_HELS;
        }

        UpdateStatistics();
        if (HelsNow <= 0)
        {
            isDead();
         

        }


    }





    public ProgrammSpellElement FindeTheSame(ProgrammSpellElement Modifire)
    {
        List<ProgrammSpellElement> Keys = EnemyEffectsDictionary.Keys.ToList();

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
        List<ProgrammSpellElement> Keys = EnemyEffectsDictionary.Keys.ToList();

        foreach (var element in Keys)
        {
            if (Modifire.type == element.type)
            {
                return true;
            }

        }

        return false;
    }



    void isDead()
    {
        // Enemy.SetActive(false);
        //purging();
        GameManager.Figth(false);
        //isSpavned = false;
        GameObject.Destroy(gameObject);
    }


   
    void nextRoom(GameObject number)
    {
        isSpavned = false;
    }

    void hpNow()
    {
        HPText.text = Convert.ToString(HelsNow) + " / " + Convert.ToString(MAX_HELS);
    }
    void mpNow()
    {
        HPText2.text = Convert.ToString(ManaNow) + " / " + Convert.ToString(MAX_MANA);
    }

    void BarierNow()
    {
        // print(Barrier);
        BarierText.text = Convert.ToString(Barrier);
    }
    void FireBarierNow()
    {

        FireBarierText.text = Convert.ToString(FireBarier);
    }
    void ColdBarierNow()
    {
        ColdBarierText.text = Convert.ToString(ColdBarier);
    }
    void LigthBarierNow()
    {
        LigthBarierText.text = Convert.ToString(LigthBarier);
    }
    void DarknesBarierNow()
    {
        DarcknessBarierTxet.text = Convert.ToString(DarknesBarier);
    }
    public void FollowHero()
    {
        Vector3 targetDirection = Player.transform.position - transform.position;


        float singleStep = 1.0f * Time.deltaTime;


        Vector3 newDirection = Vector3.RotateTowards(transform.forward, targetDirection, singleStep, 0.0f);


        Debug.DrawRay(transform.position, newDirection, Color.red);


        transform.rotation = Quaternion.LookRotation(newDirection);
    }
    // Update is called once per frame

}





