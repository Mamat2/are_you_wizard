using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgrammSpellElement
{
    public string type;
    public int level;
    public float value;
   

    public ProgrammSpellElement(string Type, int Level, float Value )
    {
        type = Type;
        level = Level;
        value = Value;
        //duration = Duration;
    }


}
