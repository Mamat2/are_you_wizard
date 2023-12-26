using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class SpellManager : MonoBehaviour
{

    public static UnityEvent<string> CreateSpellElementEvt = new UnityEvent<string>();
    public static UnityEvent<BaseSpelElement> CastSpellEvt = new UnityEvent<BaseSpelElement>();


    // Start is called before the first frame update
    void Start()
    {
        BatleManager.InvokeSpellEvt.AddListener(ProcessKeyInput);
        BatleManager.doStepEvt.AddListener(trackTurnAndClearScreen);
        GameManager.IsFigtEvt.AddListener(IsFight);
        CastSpellEvt.AddListener(ProcessSpell);
    }


    List<List<string>> groups = new List<List<string>>
    {
        new List<string> { "Connector" },
        new List<string> { "Darkness", "Ligth", "Cold", "Fire" },
        new List<string> { "Cut", "Crush", "Pierce" },
        new List<string> { "Damage", "Barrier" },
        new List<string> { "Reverse", "Leech" },
        new  List<string> { "SelfTargeting", "EnemyTargeting" }
    };

    List<int> groupCounters = new List<int>();
    void trackTurnAndClearScreen(string who)
    {
        if (who == "Enemy")
        {
            clearScreen();
        }

    }

    void IsFight(bool state)
    {
        if (state == false)
        {
            clearScreen();
        }
    }

    void clearScreen()
    {
       BaseSpelElement[] spellElements = GameObject.FindObjectsOfType<BaseSpelElement>();
            if (spellElements.Length > 0)
            {
                foreach (BaseSpelElement element in spellElements)
                {
                    Destroy(element.gameObject);
                    }
            }
    }

    public static void CastSpell(BaseSpelElement spelElement)
    {
        CastSpellEvt.Invoke(spelElement);
    }

    void ProcessKeyInput(int key)
    {
        // Проверяем, что группы счетчиков уже инициализированы
        if (groupCounters.Count != groups.Count)
        {
            // Инициализируем счетчики для каждой группы
            for (int i = 0; i < groups.Count; i++)
            {
                groupCounters.Add(0);
            }
        }

        // Получаем текущую группу
        List<string> currentGroup = groups[key - 1];

        // Получаем счетчик для текущей группы
        int currentCounter = groupCounters[key - 1];

        if (currentCounter >= currentGroup.Count)
        {
            // Сбрасываем счетчик к начальному значению
            currentCounter = 0;
        }

        // Вызываем функцию CreateElement и передаем название группы
        CreateSpellElementEvt.Invoke(currentGroup[currentCounter]);

        // Увеличиваем счетчик для текущей группы
        groupCounters[key - 1] = (currentCounter + 1) % currentGroup.Count;

    }


    void ProcessSpell(BaseSpelElement spelElement)
    {
        BaseSpelElement rootElement = FindRootElement(spelElement);
         // print(FindRootElement(spelElement));
        List<List<string>> ElementLists = TraverseBinaryTree(rootElement);
        List<List<string>> filtredList = FilterLists(ElementLists);

/*        print("ElemetnLIst");
        PrintResult(ElementLists); */
      /*  print("FiltredLISt");

       
        PrintResult(filtredList);*/
        // List<ProgrammSpellElement> EffectList = ProcessElements(ElementLists);
        // PrintResult(ElementLists);
        FinalSpel Spell = ProcessElements(filtredList);

/*
        string message = "";

        foreach (var status in Spell.statuses)
        {
            message += (status.type + "   " + status.level + "   " + status.value + " \n ");
        }

        print("manacost " + Spell.manaCost + " \n " +
             "Duration " + Spell.duration + " \n " +
              "target " + Spell.target + " \n " +
              "buff " + Spell.buffDebuff + " \n " +
              "Effects " + "\n" +
              "type    level    value   \n" +
              message + " \n "
              );*/


        Spell.SortEffects();
         BatleManager.CastSpell(Spell.target,Spell);

 /*      message = "";

        foreach (var status in Spell.statuses)
        {
            message += (status.type + "   " + status.level + "   " + status.value + " \n ");
        }

        print("manacost " + Spell.manaCost + " \n " +
             "Duration " + Spell.duration + " \n " +
              "target " + Spell.target + " \n " +
              "buff " + Spell.buffDebuff + " \n " +
              "Effects " + "\n" +
              "type    level    value   \n" +
              message + " \n "
              );*/


        // PrintResult(EffectList);


        //print("-------");



    }





    public BaseSpelElement FindRootElement(BaseSpelElement element)
    {
        if (element == null)
            return null;

        BaseSpelElement tmp = element;

        while (tmp.Parent != null)
        {
            tmp = tmp.Parent;
        }

        return tmp;
    }

    public void PrintResult(List<List<string>> result)
    {
        string Presult ="";
        foreach (List<string> list in result)
        {
            string line = "";
            foreach (string item in list)
            { line += item + " "; }
            Presult += line;
            Presult += " \n";
        }
        print(Presult);
    }

/*    public void PrintResult (List<ProgrammSpellElement> result)
    {
        foreach (var effect in result)
        {
            
            print(effect.type +" "+ effect.level + " " + effect.value + " " + effect.duration);
        }
    }*/

    /*    public List<string[]> TraverseBinaryTree(BaseSpelElement startElement)
        {
            List<string[]> connectorLists = new List<string[]>();
            List<string> connectors = new List<string>();
            TraverseNode(startElement, connectorLists, connectors);
            return connectorLists;
        }*/
    public List<List<string>> TraverseBinaryTree(BaseSpelElement startElement)
    {
        List<List<string>> result = new List<List<string>>();
        List<string> connectorsList = new List<string>();
        List<string> formsList = new List<string>();
        List<string> targetsList = new List<string>();
        List<string> reversList = new List<string>();
        List<string> lechList = new List<string>();
        List<string> elementsList = new List<string>();
        List<string> damageBarierList = new List<string>();
        TraverseNode(startElement, result, 
                    connectorsList, 
                    formsList, 
                    targetsList, 
                    reversList, 
                    lechList, 
                    elementsList, 
                    damageBarierList);
        return result;
    }

    private void TraverseNode(BaseSpelElement element,
                              List<List<string>> resultList,
                              List<string> connectors,
                              List<string> forms,
                              List<string> targets,
                              List<string> revers,
                              List<string> lech,
                              List<string> elements,
                              List<string> damageBarier)
    {
        if (element == null)
            return;

        if (element.value == "Connector")
        {
            connectors.Add(element.value);
        }
        if (element.value == "Form")
        {
            forms.Add(element.secondValue);
        }
        if (element.value == "Target")
        {
            targets.Add(element.secondValue);
        }
        if (element.value == "Reverse")
        {
            revers.Add(element.value);
        }
        if (element.value == "Leech")
        {
            lech.Add(element.value);
        }
        if (element.value == "Particle")
        {
            elements.Add(element.secondValue);
        }
        if (element.value == "Barrier" || element.value == "Damage")
        {
            damageBarier.Add(element.value);
        }

        if (element.child1 == null && element.child2 == null)
        {
            if (element.value == "Connector")
            {
                resultList.Add(new List<string>(connectors));
                //connectors.Clear();
            }
            if (element.value == "Form")
            {
               // print("aded");
                resultList.Add(new List<string>(forms));
                //forms.Clear();
            }
            if (element.value == "Target")
            {
                resultList.Add(new List<string>(targets));
                //targets.Clear();
            }
            if (element.value == "Reverse")
            {
                resultList.Add(new List<string>(revers));
                //revers.Clear();
            }
            if (element.value == "Leech")
            {
                resultList.Add(new List<string>(lech));
               // lech.Clear();
            }
            if (element.value == "Particle")
            {
                resultList.Add(new List<string>(elements));
                elements.Clear();
            }
            if (element.value == "Barrier" || element.value == "Damage")
            {
                resultList.Add(new List<string>(damageBarier));
                damageBarier.Clear();
            }
        }

        if (element.value == "Connector")
        {
            if (element.child1 != null && element.child2 != null)
            {
                if (element.child1.value != "Connector" && element.child2.value != "Connector")
                {
                    resultList.Add(new List<string>(connectors));
                }
                TraverseNode(element.child1, resultList, connectors, forms, targets, revers, lech, elements, damageBarier);
                TraverseNode(element.child2, resultList, connectors, forms, targets, revers, lech, elements, damageBarier);
            }
            else
            if (element.child1 != null && element.child2 == null)
            {
                if (element.child1.value != "Connector")
                {
                    resultList.Add(new List<string>(connectors));

                }
                TraverseNode(element.child1, resultList, connectors, forms, targets, revers, lech, elements, damageBarier);
            }
            else
                if (element.child1 == null && element.child2 != null)
            {
                if (element.child2.value != "Connector")
                {
                    resultList.Add(new List<string>(connectors));

                }
                TraverseNode(element.child2, resultList, connectors, forms, targets, revers, lech, elements, damageBarier);
            }
        }
        else 
        {
            if ((element.value == "Barrier" || element.value == "Damage") &&
                (element.child1!=null )&&
                (element.child1.value != "Barrier" && element.child1.value != "Damage") && damageBarier.Count > 0)
            {
               // print(element.name+"\n " + element.child1 + "\n " + element.child1.value);
              //  print(damageBarier.Count);
                resultList.Add(new List<string>(damageBarier));
                damageBarier.Clear();

            }
           // print("qwe");
            TraverseNode(element.child1, resultList, connectors, forms, targets, revers, lech, elements, damageBarier);
            //TraverseNode(element.child2, resultList, connectors, forms, targets, revers, lech, elements, damageBarier);
        }
        
        
    }


    public List<List<string>> FilterLists(List<List<string>> resultList)
    {
        List<List<string>> filteredLists = new List<List<string>>();

        int CounnectorMaxLength = 0; // Variable to store the maximum length of the filtered lists
        int formMaxLength = 0;
        int targetMaxLength = 0;
        int reverseMaxLength = 0;
        int leechMaxLength = 0;


        foreach (List<string> list in resultList)
        {
            // Check if the list contains the specified elements

            if (list.Contains("Connector"))
            {
                if (list.Count > CounnectorMaxLength)
                {
                    CounnectorMaxLength = list.Count;
                }
            }
           
            if (list.Contains("Cut")||list.Contains("Crush")||list.Contains("Pierce"))
            {
                if (list.Count > formMaxLength)
                {
                    formMaxLength = list.Count;
                }
            }
           
            if (list.Contains("SelfTarget") || list.Contains("EnemyTargeting"))
            {
                
                if (list.Count > targetMaxLength)
                {
                    targetMaxLength = list.Count;
                }
            }

            if (list.Contains("Reverse"))
            {
                if (list.Count > reverseMaxLength)
                {
                    reverseMaxLength = list.Count;
                }
            }

            if (list.Contains("Leech"))
            {
                if (list.Count > leechMaxLength)
                {
                    leechMaxLength = list.Count;
                }
            }

        }

        foreach (List<string> list in resultList)
        {
            if (list.Contains("Darkness") || 
                list.Contains("Ligth") || 
                list.Contains("Cold") || 
                list.Contains("Fire") ||
                list.Contains("Damage") || 
                list.Contains("Barrier") )
            {
                filteredLists.Add(list);
            }
            if (list.Contains("Connector")&& list.Count== CounnectorMaxLength)
            {
                filteredLists.Add(list);
            }
            if ((list.Contains("Cut")|| 
                list.Contains("Crush")|| 
                list.Contains("Pierce")) && list.Count == formMaxLength)
            {
                filteredLists.Add(list);
            }

            // Для переменной "Target"
            if ((list.Contains("SelfTarget") || list.Contains("EnemyTargeting")) && list.Count == targetMaxLength)
            {
                //print("Filtred"); 
                filteredLists.Add(list);
            }

            // Для переменной "Reverse"
            if (list.Contains("Reverse") && list.Count == reverseMaxLength)
            {
                filteredLists.Add(list);
            }

            // Для переменной "Leech"
            if (list.Contains("Leech") && list.Count == leechMaxLength)
            {
                filteredLists.Add(list);
            }

        }




        return filteredLists;
    }


    public FinalSpel ProcessElements( List<List<string>> ListOfElements)
    {
        FinalSpel result = new FinalSpel() ;

        List<ProgrammSpellElement> Effects  = new List<ProgrammSpellElement>();
      //  PrintResult(ListOfElements);

        foreach (List<string> elements in ListOfElements)
        {
/*            string message0 = "";

            foreach (string element in elements)
            {
                message0 += element;
                message0 += ", ";
            }

            print(message0);*/


            if (elements.Contains("Connector"))
            {
                result.manaCost += elements.Count * 2;
            }
            if (elements.Contains("Leech"))
            {
                // print(elements.Count);
                result.duration += (1+ elements.Count);
                result.manaCost += elements.Count * elements.Count * elements.Count;
            }
            if (elements.Contains("SelfTarget") || elements.Contains("EnemyTargeting"))
            {
                if (elements.Count == 1)
                {
                    //  print("Here2");
                    result.target = elements[0];
                }
                else
                {
                    int randomIndex = UnityEngine.Random.Range(0, elements.Count);
                    result.target = elements[randomIndex];
                }
            }

            if (elements.Contains("Damage") && !elements.Contains("Barrier"))
            {

                ProgrammSpellElement Effect = new ProgrammSpellElement("Damage", 0, elements.Count * 5F);

                result.statuses.Add(Effect);
                result.manaCost += elements.Count * 5;
            }
            else
            if (elements.Contains("Barrier") && !elements.Contains("Damage"))
            {

                ProgrammSpellElement Effect = new ProgrammSpellElement("Barrier", 0, elements.Count * 5F);

                result.statuses.Add(Effect);
                result.manaCost += elements.Count * 7;
            }
            else
            if (elements.Contains("Barrier") && elements.Contains("Damage"))
            {
                int barrierCount = 0;
                int damageCount = 0;

                foreach (string element in elements)
                {
                    if (element == "Barrier")
                    {
                        barrierCount++;
                    }
                    else if (element == "Damage")
                    {
                        damageCount++;
                    }
                }

                ProgrammSpellElement Effect = new ProgrammSpellElement("BarrierCurse", 0, (barrierCount + damageCount) / 2F);

                result.statuses.Add(Effect);
                result.manaCost += elements.Count * 5.5f;
            }
            else
            if (elements.Contains("Reverse"))
            {

                int reversCount = elements.Count;
                if (reversCount % 2 != 0)
                {
                    //print("AAAAAAA");
                    result.buffDebuff = true;
                }
                else
                {
                    // print("Noooooooo");
                    result.buffDebuff = false;
                }
            }
            else


            if (elements.Contains("Darkness") ||
                elements.Contains("Ligth") ||
                 elements.Contains("Cold") ||
                 elements.Contains("Fire"))
            {
                if (elements.Count < 2)
                {
                 //   print("aa");
                    result.statuses.Add(new ProgrammSpellElement(elements[0], 0, elements.Count));
                    result.manaCost += elements.Count * 4;
                }
                else
                {
                    string elem1 = elements[0];
                    string elem2 = null;
                    int counter = 1;
                    while (elem2 == null && counter < elements.Count)
                    {
                        if (elements[counter] != elem1)
                        {
                            elem2 = elements[counter];
                        }
                        else { counter++; }

                    }
                    if (elem2 == null)
                    {
                        elem2 = elem1;
                    }


                    if (elem1 == elem2)
                    {
                        // print("3");
                        // print("there");
                        result.statuses.Add(new ProgrammSpellElement(elem1, 0, elements.Count));
                        result.manaCost += elements.Count * 4;
                        switch (elem1)
                        {
                            case "Darkness":
                                result.statuses.Add(new ProgrammSpellElement("Energy", 0, elements.Count));
                                break;
                            case "Ligth":
                                result.statuses.Add(new ProgrammSpellElement("Ligth", 0, elements.Count));
                                break;
                            case "Cold":
                                result.statuses.Add(new ProgrammSpellElement("Durability", 0, elements.Count * 0.5F));
                                break;
                            case "Fire":
                                result.statuses.Add(new ProgrammSpellElement("ManaBurn", 0, elements.Count * 0.5F));
                                break;


                        }
                    }
                    else
                    {
                        switch (elem1)
                        {
                            case ("Darkness"):
                                //  print("there");
                                result.statuses.Add(new ProgrammSpellElement("Darkness", 0, elements.Count));
                                switch (elem2)
                                {

                                    case "Ligth":
                                        result.statuses.Add(new ProgrammSpellElement("Ligth", 0, elements.Count));
                                        result.manaCost += elements.Count * elements.Count * 2;
                                        break;


                                    case "Cold":
                                        result.statuses.Add(new ProgrammSpellElement("Durability", 0, elements.Count * 0.5F));
                                        result.statuses.Add(new ProgrammSpellElement("Cold", 0, elements.Count));
                                        result.manaCost += elements.Count * 5;
                                        break;

                                    case "Fire":
                                        result.statuses.Add(new ProgrammSpellElement("Strength", 0, elements.Count * 0.5F));
                                        result.statuses.Add(new ProgrammSpellElement("Fire", 0, elements.Count));
                                        result.manaCost += elements.Count * 5;
                                        break;

                                }
                                break;


                            case ("Ligth"):
                                result.statuses.Add(new ProgrammSpellElement("Ligth", 0, elements.Count));
                                switch (elem2)
                                {
                                    case "Darkness":
                                        result.statuses.Add(new ProgrammSpellElement("Darkness", 0, elements.Count));
                                        result.manaCost += elements.Count * elements.Count * 2;
                                        break;


                                    case "Cold":
                                        result.statuses.Add(new ProgrammSpellElement("Breakable", 0, elements.Count * 0.5F));
                                        result.statuses.Add(new ProgrammSpellElement("Cold", 0, elements.Count));
                                        result.manaCost += elements.Count * 4;
                                        break;

                                    case "Fire":
                                        result.statuses.Add(new ProgrammSpellElement("Speed", 0, elements.Count * 0.5F));
                                        result.statuses.Add(new ProgrammSpellElement("Fire", 0, elements.Count));
                                        result.manaCost += elements.Count * 4;
                                        break;

                                }
                                break;


                            case ("Cold"):
                                result.statuses.Add(new ProgrammSpellElement("Cold", 0, elements.Count));
                                switch (elem2)
                                {
                                    case "Ligth":
                                        result.statuses.Add(new ProgrammSpellElement("Breakable", 0, elements.Count * 0.5F));
                                        result.statuses.Add(new ProgrammSpellElement("Ligth", 0, elements.Count));
                                        result.manaCost += elements.Count * 4;
                                        break;
                                    case "Darkness":
                                        result.statuses.Add(new ProgrammSpellElement("Darkness", 0, elements.Count));
                                        result.statuses.Add(new ProgrammSpellElement("Durability", 0, elements.Count * 0.5F));
                                        result.manaCost += elements.Count * 4;
                                        break;

                                    case "Fire":
                                        result.statuses.Add(new ProgrammSpellElement("Fire", 0, elements.Count));
                                        result.manaCost += elements.Count * elements.Count * 2;
                                        break;
                                }
                                break;



                            case ("Fire"):
                                result.statuses.Add(new ProgrammSpellElement("Fire", 0, elements.Count));
                                switch (elem2)
                                {
                                    case "Darkness":
                                        result.statuses.Add(new ProgrammSpellElement("Strength", 0, elements.Count * 0.5F));
                                        result.statuses.Add(new ProgrammSpellElement("Darkness", 0, elements.Count));
                                        result.manaCost += elements.Count * 4;
                                        break;
                                    case "Ligth":
                                        result.statuses.Add(new ProgrammSpellElement("Speed", 0, elements.Count * 0.5F));
                                        result.statuses.Add(new ProgrammSpellElement("Ligth", 0, elements.Count));
                                        result.manaCost += elements.Count * 4;
                                        break;
                                    case "Cold":
                                        result.statuses.Add(new ProgrammSpellElement("Cold", 0, elements.Count));

                                        result.manaCost += elements.Count * elements.Count * 2;
                                        break;
                                  
                                }

                                break;

                        }

                    }

                }
            }


            /*      if (elements.Contains("Darkness") ||
                  elements.Contains("Ligth") ||
                  elements.Contains("Cold") ||
                  elements.Contains("Fire"))
              {
                  //print("0");
                  string message = "";

                      foreach (string element in elements) 
                      {
                      message += element;
                      message += ", ";
                      }

                  print(message);

                  if (elements.Count < 2)
                  {
                      print("a");
                  }
                  else
                  {
                      print("b");
                  }

                  if (elements.Count <2)
                  {
                    //  print("1");
                      result.statuses.Add(new ProgrammSpellElement(elements[0], 0, elements.Count));
                      result.manaCost += elements.Count * 4;

                  }
                  else
                  {
                     // print("2");
                      string elem1 = elements[0];
                      string elem2 = null;
                      int counter = 1;
                      while (elem2 == null && counter < elements.Count)
                      {
                          if (elements[counter] != elem1)
                          {
                              elem2 = elements[counter];
                          }
                          else { counter++; }

                      }
                      if (elem2 == null)
                      {
                          elem2 = elem1;
                      }


                      if (elem1 == elem2)
                      {
                         // print("3");
                          // print("there");
                          result.statuses.Add(new ProgrammSpellElement(elem1, 0, elements.Count));
                          result.manaCost += elements.Count * 4;
                          switch (elem1)
                          {
                              case "Darkness":
                                  result.statuses.Add(new ProgrammSpellElement("Energy", 0, elements.Count));
                                  break;
                              case "Ligth":
                                  result.statuses.Add(new ProgrammSpellElement("Ligth", 0, elements.Count));
                                  break;
                              case "Cold":
                                  result.statuses.Add(new ProgrammSpellElement("Durability", 0, elements.Count * 0.5F));
                                  break;
                              case "Fire":
                                  result.statuses.Add(new ProgrammSpellElement("Strength", 0, elements.Count * 0.5F));
                                  break;


                          }
                      }
                      else
                      {
                        //  print("4");
                          switch (elem1)
                          {
                              case ("Darkness"):
                                  //  print("there");
                                  result.statuses.Add(new ProgrammSpellElement("Darkness", 0, elements.Count));
                                  switch (elem2)
                                  {

                                      case "Ligth":
                                          result.statuses.Add(new ProgrammSpellElement("Ligth", 0, elements.Count));
                                          result.manaCost += elements.Count * elements.Count * 2;
                                          break;


                                      case "Cold":
                                          result.statuses.Add(new ProgrammSpellElement("Durability", 0, elements.Count * 0.5F));
                                          result.statuses.Add(new ProgrammSpellElement("Cold", 0, elements.Count));
                                          result.manaCost += elements.Count * 5;
                                          break;

                                      case "Fire":
                                          result.statuses.Add(new ProgrammSpellElement("Strength", 0, elements.Count * 0.5F));
                                          result.statuses.Add(new ProgrammSpellElement("Fire", 0, elements.Count));
                                          result.manaCost += elements.Count * 5;
                                          break;

                                  }
                                  break;


                              case ("Ligth"):
                                  result.statuses.Add(new ProgrammSpellElement("Ligth", 0, elements.Count));
                                  switch (elem2)
                                  {
                                      case "Darkness":
                                          result.statuses.Add(new ProgrammSpellElement("Darkness", 0, elements.Count));
                                          result.manaCost += elements.Count * elements.Count * 2;
                                          break;


                                      case "Cold":
                                          result.statuses.Add(new ProgrammSpellElement("Breakable", 0, elements.Count * 0.5F));
                                          result.statuses.Add(new ProgrammSpellElement("Cold", 0, elements.Count));
                                          result.manaCost += elements.Count * 4;
                                          break;

                                      case "Fire":
                                          result.statuses.Add(new ProgrammSpellElement("Speed", 0, elements.Count * 0.5F));
                                          result.statuses.Add(new ProgrammSpellElement("Fire", 0, elements.Count));
                                          result.manaCost += elements.Count * 4;
                                          break;

                                  }
                                  break;


                              case ("Cold"):
                                  result.statuses.Add(new ProgrammSpellElement("Cold", 0, elements.Count));
                                  switch (elem2)
                                  {
                                      case "Ligth":
                                          result.statuses.Add(new ProgrammSpellElement("Breakable", 0, elements.Count * 0.5F));
                                          result.statuses.Add(new ProgrammSpellElement("Ligth", 0, elements.Count));
                                          result.manaCost += elements.Count * 4;
                                          break;
                                      case "Darkness":
                                          result.statuses.Add(new ProgrammSpellElement("Darkness", 0, elements.Count));
                                          result.statuses.Add(new ProgrammSpellElement("Durability", 0, elements.Count * 0.5F));
                                          result.manaCost += elements.Count * 4;
                                          break;

                                      case "Fire":
                                          result.statuses.Add(new ProgrammSpellElement("Fire", 0, elements.Count));
                                          result.manaCost += elements.Count * elements.Count * 2;
                                          break;
                                  }
                                  break;



                              case ("Fire"):
                                  result.statuses.Add(new ProgrammSpellElement("Fire", 0, elements.Count));
                                  switch (elem2)
                                  {
                                      case "Darkness":
                                          result.statuses.Add(new ProgrammSpellElement("Strength", 0, elements.Count * 0.5F));
                                          result.statuses.Add(new ProgrammSpellElement("Darkness", 0, elements.Count));
                                          result.manaCost += elements.Count * 4;
                                          break;
                                      case "Ligth":
                                          result.statuses.Add(new ProgrammSpellElement("Speed", 0, elements.Count * 0.5F));
                                          result.statuses.Add(new ProgrammSpellElement("Ligth", 0, elements.Count));
                                          result.manaCost += elements.Count * 4;
                                          break;
                                      case "Cold":
                                          result.statuses.Add(new ProgrammSpellElement("Cold", 0, elements.Count));

                                          result.manaCost += elements.Count * elements.Count * 2;
                                          break;
                                      case "Fire":
                                          result.statuses.Add(new ProgrammSpellElement("Speed", 0, elements.Count * 0.5F));
                                          result.manaCost += elements.Count * 4;
                                          break;
                                  }

                                  break;

                          }
                          break;

                      }

                  }

              }*/
        }

        return result;
    }



       
    
    
    


}





