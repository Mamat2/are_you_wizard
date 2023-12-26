using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DarckCurse : FinalSpel
{

    public DarckCurse(float Darck, int Duration)
    {

        target = "all";
        duration = Duration;
        manaCost = 3 + Darck/2f;
        statuses = new List<ProgrammSpellElement>()
            {
                new ProgrammSpellElement("Darkness", 0, Darck)
                //new ProgrammSpellElement ("Fire", 0 , Fire)
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
