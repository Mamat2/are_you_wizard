using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBarrier : FinalSpel
{
    public FireBarrier(float Barrier , float Fire)
    {

        target = "EnemyTargeting";
        manaCost = 3 + Barrier + Fire;
        statuses = new List<ProgrammSpellElement>()
            {
                new ProgrammSpellElement("Barrier", 0, Barrier),
                new ProgrammSpellElement ("Fire", 0 , Fire)
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
