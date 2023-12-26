using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SecondEnemy : EnemyStats
{

    public SecondEnemy()
    {
        durability = 15;
        strength = 4;
        speed = 8;
        memory = 100;
        energy = 3;
        physicalResistance = 0;
        darknessResistance = 10;
        lightResistance = 5;
        heatResistance = 5;
        coldResistance = 5;


        maxHP = durability * 10F;
        hpNow = maxHP;
        maxMP = energy * 10;
        mpNow = maxMP;

        Spels = new Dictionary<string, FinalSpel>()
        {
            { "DamageSpel", new SimpleDamage(10) },
            { "HealSpell" , new SimpleHeal(10)},
            { "FireBarrier" , new FireBarrier(10,2)},
            { "DarckCurse" , new DarckCurse(10,2)}
        };//
        actionSequence = new List<string>() { "HandDamage", "FireBarrier", "HealSpell", "HealSpell" };

    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
