using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class FinalSpel
{
   public  string target = "all";
    public float manaCost  = 1;
    public bool buffDebuff = false ;
    public int duration = 0;
    public List <ProgrammSpellElement> statuses = new List<ProgrammSpellElement> ();
    //list<string> effects;
    //int gameStep = 0;
    /*    List<string> Effect = new List<string>() { "Darkness", "Ligth", "Cold", "Fire" };
        List<string> SpecialEffect = new List<string>() { "Energy", "Durability", "Strength", "Memory", "Breakable", "Speed", "Disarming" };*/

    //public Dictionary<ProgrammSpellElement, ProgrammSpellElement> EffectsPairs = new Dictionary<ProgrammSpellElement, ProgrammSpellElement> ();

/*       var Effects = new Dictionary<string, float>() {
            {"Energy", 0 },
            {"Durability", 0 },
            {"Strength", 0 },
            {"Memory", 0 },
            {"Breakable", 0 },
            {"Speed", 0 },
            {"Disarming", 0 },
            {"Darkness", 0 },
            {"Ligth", 0 },
            {"Cold", 0 },
            {"Fire", 0 },
            {"Damage", 0 },
            {"Barrier", 0 },
            {"BarrierCurse", 0 },
            { "ManaBurn",0 }
        };

  var tmp = new List<ProgrammSpellElement>(statuses);
        foreach (var element in tmp)
        {
            if (element.value > Effects[element.type])
            {
                Effects[element.type] += element.value;
            }
        }

        statuses.Clear();
        foreach (var kvp in Effects)
        {
            if (kvp.Value > 0)
            {
                statuses.Add(new ProgrammSpellElement(kvp.Key, 0, kvp.Value));
            }
        }
*/
    public void SortEffects()
    {
        var effectsDictionary = new Dictionary<string, float>();

        foreach (var element in statuses)
        {
            if (effectsDictionary.ContainsKey(element.type))
            {
                effectsDictionary[element.type] += element.value;
            }
            else
            {
                effectsDictionary[element.type] = element.value;
            }
        }

        statuses.Clear();
        foreach (var kvp in effectsDictionary)
        {
            statuses.Add(new ProgrammSpellElement(kvp.Key, 0, kvp.Value));
        }

    }

    public List<ProgrammSpellElement> GetEnySpecialEffects()
    {
        List<ProgrammSpellElement> result = new List<ProgrammSpellElement>();
        var SpecialEffects = new Dictionary<string, float>() {
            {"Energy", 0 },
            {"Durability", 0 },
            {"Strength", 0 },
            {"Memory", 0 },
            {"Speed", 0 },
            {"Breakable", 0 },
            {"Disarming", 0 },
            {"Darkness", 0 },
            {"Ligth", 0 },
            {"Cold", 0 },
            {"Fire", 0 },
            {"BarrierCurse", 0 },            
            {"ManaBurn",0 }
        };
        var tmp = new List<ProgrammSpellElement>(statuses);
        foreach (var element in tmp)
        {
            if (SpecialEffects.ContainsKey(element.type))
            {
                result.Add(element);
            }
        }
        return result;
    }



    public List<ProgrammSpellElement> GetSpecialEffects()
    {
        List<ProgrammSpellElement> result = new List<ProgrammSpellElement>();
        var SpecialEffects = new Dictionary<string, float>() {
            {"Energy", 0 },
            {"Durability", 0 },
            {"Strength", 0 },
            {"Memory", 0 },
            {"Speed", 0 },
            {"Breakable", 0 },
            {"Disarming", 0 },
            {"Darkness", 0 },
            {"Ligth", 0 },
            {"Cold", 0 },
            {"Fire", 0 },
            {"BarrierCurse", 0 },
            {"ManaBurn",0 }
        };
        var tmp = new List<ProgrammSpellElement>(statuses);
        foreach (var element in tmp)
        {
            if (SpecialEffects.ContainsKey(element.type))
            {
                result.Add(element);
            }
        }
        return result;  
    }
    public List<ProgrammSpellElement> GetDamageModifire()
    {
        List<ProgrammSpellElement> result = new List<ProgrammSpellElement>();
        var SpecialEffects = new Dictionary<string, float>() {
            {"Darkness", 0 },
            {"Ligth", 0 },
            {"Cold", 0 },
            {"Fire", 0 }
        };
        var tmp = new List<ProgrammSpellElement>(statuses);
        foreach (var element in tmp)
        {
            if (SpecialEffects.ContainsKey(element.type))
            {
                result.Add(element);
            }
        }
        return result;
    }

    public ProgrammSpellElement GetDamage()
    {
        var tmp = new List<ProgrammSpellElement>(statuses);
        foreach (var element in tmp)
        {
            if (element.type == "Damage")
            {
                return element ;
            }
        }
        return null; 
    }
    public ProgrammSpellElement GetBarrier()
    {
        var tmp = new List<ProgrammSpellElement>(statuses);
        foreach (var element in tmp)
        {
            if (element.type == "Barrier")
            {
                return element;
            }
        }
        return null;
    }

    public bool ContainDamage()
    { 
        foreach(var element in statuses)
        {
            if(element.type == "Damage")
            { return true; }
        }
        return false;    
    }
    public bool ContainBarrier()
    {
        foreach (var element in statuses)
        {
            if (element.type == "Barrier")
            { return true; }
        }
        return false;
    }




    /*  
        public bool ContaynSpecialEffect()
        { 


        }

    */

}
