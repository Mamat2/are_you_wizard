using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleDamage : FinalSpel
{
    // Start is called before the first frame update

    public SimpleDamage(int damage ) 
    {

        target = "SelfTarget";
        manaCost = 3 + damage;
        statuses = new List<ProgrammSpellElement>()
            {
                new ProgrammSpellElement("Damage", 0, damage)
            };
 

    }

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
