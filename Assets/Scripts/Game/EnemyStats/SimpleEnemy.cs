using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleEnemy : EnemyStats

{


    public SimpleEnemy()
    {
        durability = 11;
        strength = 9;
        speed = 10;
        memory = 100;
        energy = 2;
        physicalResistance = 0;
        darknessResistance = 10;
        lightResistance = 0;
        heatResistance = 0;
        coldResistance = 0;


        maxHP = durability * 10F;
        hpNow = maxHP;
        maxMP = energy * 10;
        mpNow = maxMP;

        Spels = new Dictionary<string, FinalSpel>()
        {
            { "DamageSpel", new SimpleDamage(10) },
            { "HealSpell" , new SimpleHeal(10)},
            { "FireBArrier" , new FireBarrier(10,2)},
            { "DarckCurse" , new DarckCurse(10,2)}
        };//
        actionSequence = new List<string>(){ "DarckCurse", "DarckCurse", "HealSpell", "DamageSpel" } ;

    }







   

  


    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
