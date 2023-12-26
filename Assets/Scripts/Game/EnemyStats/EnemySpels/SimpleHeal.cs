using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleHeal : FinalSpel
{

    public SimpleHeal(int heal)
    {

        target = "EnemyTargeting";
        buffDebuff = true;
        manaCost = 3 + heal;
        statuses = new List<ProgrammSpellElement>()
            {
                new ProgrammSpellElement("Damage", 0, heal)
            };


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
