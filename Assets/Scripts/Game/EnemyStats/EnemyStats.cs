using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStats 
{
    public  float maxHP = 0;
    public float hpNow = 0;
    
    public float maxMP = 0;
    public float mpNow = 0;


    public float durability =0;
    public float strength =0;
    public float speed =0;
    public float memory=0;
    public float energy=0;

    public float physicalResistance =0;  //- физическое сопротивление
    public float darknessResistance =0;//- сопротивление тьме
    public float lightResistance =0;//- сопротивление свету
    public float heatResistance =0;//- сопротивление теплу
    public float coldResistance =0;//- сопротивление холоду

//    public List<FinalSpel> Spels = new List<FinalSpel>() ;

/*    Dictionary<string, FinalSpel> Spels = new Dictionary<string, FinalSpel>();


    // public List<ProgrammSpellElement> Effeсts;
    public List<ProgrammSpellElement> Effeсts = new List<ProgrammSpellElement>();*/

    public Dictionary<string, FinalSpel> Spels;

   public  List<string> actionSequence;


    /*    public EnemyStats(float maxHP,  float maxMP,  float durability, float strength, float speed, float memory, float energy, float physicalResistance, float darknessResistance, float lightResistance, float heatResistance, float coldResistance)
        {
            this.maxHP = maxHP *durability;
            this.hpNow = maxHP * durability;
            this.maxMP = maxMP*energy;
            this.mpNow = maxMP * energy;

            this.durability = durability;
            this.strength = strength;
            this.speed = speed;
            this.memory = memory;
            this.energy = energy;



            this.physicalResistance = physicalResistance;
            this.darknessResistance = darknessResistance;
            this.lightResistance = lightResistance;
            this.heatResistance = heatResistance;
            this.coldResistance = coldResistance;
        }*/
/*    public void addSpell()
    {
        FinalSpel Spell = new FinalSpel ();
        ProgrammSpellElement effect = AddEffect("Fire", 1, 10.0f); // Пример добавления нового эффекта
        Spell.statuses.Add(effect);

    }

    public ProgrammSpellElement AddEffect(string Type, int Level, float Value)
    {

        ProgrammSpellElement effect = new ProgrammSpellElement(Type, Level, Value);
        return effect;
    }*/

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
