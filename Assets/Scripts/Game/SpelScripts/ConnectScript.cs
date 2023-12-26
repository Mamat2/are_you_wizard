using System.Collections;
using System.Collections.Generic;
using System.Threading;
//using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class ConnectScript : MonoBehaviour
{


    void Start()
    {
        BatleManager.DockingRequestEvt.AddListener(CheckConnect);

    }

    void Update()
    {
    }




    bool haventParrent(BaseSpelElement whoConnecting)
    {
        if (whoConnecting.parent == null) return true;
        else return false;
    }


    bool AreInSameChain(BaseSpelElement obj1, BaseSpelElement obj2)
    {
        // Проверяем, является ли obj2 родителем или ребенком obj1
        if (obj1 == obj2.parent || obj1 == obj2.child1 || obj1 == obj2.child2)
        {
            return true;
        }

        // Проверяем, является ли obj1 родителем или ребенком obj2
        if (obj2 == obj1.parent || obj2 == obj1.child1 || obj2 == obj1.child2)
        {
            return true;
        }
        int counter = 0;
        // Проверяем, находятся ли obj1 и obj2 в одной цепочке родителей
        BaseSpelElement parent1 = obj1.parent;
        while (parent1 != null && counter < 300)
        {
            if (parent1 == obj2.parent || parent1 == obj2.child1 || parent1 == obj2.child2)
            {
                return true;
            }
            parent1 = parent1.parent;
            counter++;
        }

        // Проверяем, находятся ли obj1 и obj2 в одной цепочке детей
        BaseSpelElement child1 = obj1.child1;
        counter = 0;
        while (child1 != null && counter < 300)
        {
            if (child1 == obj2.parent || child1 == obj2.child1 || child1 == obj2.child2)
            {
                return true;
            }
            child1 = child1.child1;
            counter++;
        }

        // Если не нашли общей цепочки, возвращаем false
        return false;
    }


    bool isNotParrent(BaseSpelElement first, BaseSpelElement second)
    {

        int count = 0;
        if (haventParrent(first)) return true;
        // print (false);
        if (first.Parent == second) return false;

        BaseSpelElement tmpParrent = first.Parent;
        while (tmpParrent != null && count < 100)
        {

            if (tmpParrent == second) return false;

            tmpParrent = tmpParrent.Parent;
            count++;


        }

        return true;
    }




    void CheckConnect(GameObject whoConnects, Collision WithWhomConnectedCollision)
    {
        BaseSpelElement child = whoConnects.GetComponent<BaseSpelElement>();
        BaseSpelElement parent = WithWhomConnectedCollision.gameObject.GetComponent<BaseSpelElement>();
//!AreInSameChain(child, parent)
        if ( isNotParrent(parent,child) && isNotParrent(child,parent) && child != parent && RuleOfConnect (child,parent))
        {

            if ((parent.child1 == null || (parent.value == "Connector" && parent.child2 == null)) && child.parent == null)
            {
                if (parent.value == "Connector")
                {
                    if (parent.child1 == null)
                    {
                        parent.child1 = child;
                    }
                    else if (parent.child2 == null)
                    {
                        parent.child2 = child;
                    }
                    child.parent = parent;
                }
                else
                {
                    child.parent = parent;
                    parent.child1 = child;
                }
                DoConnect(child);
            }
        }
    }


    bool RuleOfConnect(BaseSpelElement child , BaseSpelElement parent )
    {
        BaseSpelElement tmp =null;
    //    BaseSpelElement tmp2=null;
        if (parent.parent != null) {
             tmp= parent.parent;

        }
        if (parent.value == "Connector")
        {
            return true;
        }
        else
       if (parent.value == "Reverse" || parent.value == "Target" || parent.value == "Leech" || parent.value == "Form")
        {
            return false;
        }        
        else if (parent.value == "Particle" &&
            (child.value == "Reverse" ||
            child.value == "Target" ||
            child.value == "Leech" ||
            child.value == "Form"))
        {
           
            return false;
        }
        else 
        if (parent.value != "Particle" && child.value != "Particle"&& child.value!="Connector")
        {
            if (tmp == null)
            {
                return true;
            }
            else if (child.value != parent.parent.value)
            {
                return true;

            }
            else { return false; }
        }
        else if (parent.value == "Particle" && child.value == "Particle")
        {
            if (parent.otherConnection != null)
            {
                if (child.child1 == null)
                {
                    if (child.secondValue == parent.otherConnection ||
                        child.secondValue == parent.secondValue)
                    {
                        return true;
                    }
                    else return false;
                }
                else
                {
                    if (parent.secondValue == child.secondValue)
                    {
                        if (parent.secondValue == child.child1.secondValue)
                        {
                            return true;
                        } else
                        if (parent.otherConnection == child.child1.secondValue)
                        {
                            return true;
                        } else 
                        { return false; }

                    } else
                    {
                        if (parent.otherConnection == child.secondValue )
                        {
                            if (parent.secondValue == child.child1.secondValue)
                            {
                                return true;
                            }
                            else if (parent.otherConnection == child.child1.secondValue)
                            {
                                return true;
                            }
                            else return false;
                        }
                        else return false;
                    }
                }
            }
            else 
            {
                if (child.child1 == null)
                {
                    return true;
                }
                else
                {
                    if (parent.secondValue == child.secondValue)
                    {
                        return true;
                    }
                    else if (parent.secondValue == child.child1.secondValue)
                    {
                        return true;
                    } 
                    else return false;
                }
                
          
            
            } 

        }
        else { return false; }

    }


    public List<string> GetUniqueChainElementsFromParent(BaseSpelElement parent)
    {
        List<string> elements = new List<string>();

        while (parent != null && parent.value != null && parent.value != "connector" && elements.Count < 2)
        {
            if (!elements.Contains(parent.secondValue) && parent.secondValue != null)
            {
                elements.Add(parent.secondValue);
            }
            parent = parent.parent;
        }

        // Если список элементов меньше двух, добавьте null в качестве заполнителя
        while (elements.Count < 2)
        {
            elements.Add(null);
        }

        return elements.Count > 2 ? elements.GetRange(0, 2) : elements;
    }

    public List<string> GetUniqueChainElementsFromChild(BaseSpelElement child)
    {
        List<string> elements = new List<string>();

        while (child != null && child.value != null && child.value != "connector" && elements.Count < 2)
        {
            if (!elements.Contains(child.secondValue) && child.secondValue != null)
            {
                elements.Add(child.secondValue);
            }
            child = child.parent;
        }

        // Если список элементов меньше двух, добавьте null в качестве заполнителя
        while (elements.Count < 2)
        {
            elements.Add(null);
        }

        return elements.Count > 2 ? elements.GetRange(0, 2) : elements;
    }



    public bool AreElementsEqual(List<string> element1, List<string> element2)
    {
        // Создание копий массивов для безопасного изменения
        List<string> copy1 = new List<string>(element1);
        List<string> copy2 = new List<string>(element2);

        // Сортировка элементов массивов
        copy1.Sort();
        copy2.Sort();

        // Сравнение элементов массивов
        for (int i = 0; i < Mathf.Min(copy1.Count, copy2.Count); i++)
        {
            if (copy1[i] != copy2[i])
            {
                return false;
            }
        }

        return true;
    }



    void DoConnect(BaseSpelElement child)
    {

        child.mustWatch = child.parent.gameObject;
      //  var joint = child.gameObject.AddComponent<SpringJoint2D>();
        Rigidbody parentRigidbody = child.parent.GetComponent<Rigidbody>();

        // Создаем SpringJoint компонент на дочернем объекте
        SpringJoint joint = child.gameObject.AddComponent<SpringJoint>();
        joint.autoConfigureConnectedAnchor = false;
        joint.connectedBody = parentRigidbody;
        joint.anchor = Vector3.zero;
        joint.connectedAnchor = Vector3.zero;
        joint.spring = 6f;
        joint.damper = 1f;
        joint.minDistance = 0.2f;
        joint.maxDistance = 0.4f;


    }



}


