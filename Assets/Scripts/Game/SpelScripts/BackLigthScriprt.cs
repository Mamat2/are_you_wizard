using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackLigthScriprt : MonoBehaviour
{

    BaseSpelElement script;
    public GameObject BacklightRing;

    // Start is called before the first frame update
    void Start()
    {
        GameObject parentObject = transform.parent.gameObject;
        //Transform secondChild = parentObject.transform.Find("BacklightRing");
        script = GetComponentInParent<BaseSpelElement>();
        BacklightRing = parentObject.transform.Find("BacklightRing").gameObject;
        if (BacklightRing == null)
        {
            Debug.LogError("BacklightRing object not found!");
        }





        /*        Transform backlightRingTransform = transform.Find("BacklightRing");
                if (backlightRingTransform != null)
                {
                    BacklightRing = backlightRingTransform.gameObject;
                }
                else
                {
                    Debug.LogError("BacklightRing object not found!");
                }

               

        */
    }



    public void TrackVariablesAndChangeColor()
    {
        string value = script.value;
        BaseSpelElement parent = script.parent;
        BaseSpelElement child1 = script.child1;
        BaseSpelElement child2 = script.child2;
       // print(parent);


        /*        if (BacklightRing == null)
                {
                    Transform backlightRingTransform = transform.Find("BacklightRing");
                    if (backlightRingTransform != null)
                    {
                        BacklightRing = backlightRingTransform.gameObject;
                    }
                    else
                    {
                        Debug.LogError("BacklightRing object not found!");
                    }

                }*/

/*        if (BacklightRing == null)
        {
            BacklightRing = transform.GetChild(1).gameObject;
            if (BacklightRing == null)
            {
                Debug.LogError("BacklightRing object not found!");
            }
        }*/

        if (value == "Reverse" || value == "Target" || value == "Leech" || value == "Form")
        {
            if (parent == null)
            {
                BacklightRing.SetActive(true); // Сделать BackLigthRing видимым
                BacklightRing.GetComponent<Renderer>().material.color = Color.blue; // Изменить на красный синий
            }
            else if (parent != null)
            {
                //BacklightRing.SetActive(true); // Сделать BackLigthRing видимым
                BacklightRing.SetActive(false); // Сделать BackLigthRing невидимым // Изменить на синий цвет
            }
        }
        else if (value == "Connector")
        {
            if (parent == null && (child1 == null || child2 == null))
            {
                BacklightRing.SetActive(true); // Сделать BackLigthRing видимым
                BacklightRing.GetComponent<Renderer>().material.color = Color.white;
            }
            else if (parent != null && (child1 == null || child2 == null))
            {
                BacklightRing.SetActive(true); // Сделать BackLigthRing видимым
                BacklightRing.GetComponent<Renderer>().material.color = Color.red; // Изменить на красный цвет
            }
            else if (parent == null && child1 != null && child2 != null)
            {
                BacklightRing.SetActive(true); // Сделать BackLigthRing видимым
                BacklightRing.GetComponent<Renderer>().material.color = Color.blue; // Изменить на базовый цвет
            }
            else if (parent != null && child1 != null && child2 != null)
            {

                BacklightRing.SetActive(false);
            }
        }
        else
        {

            if (parent == null && child1 == null)
            {
                BacklightRing.GetComponent<Renderer>().material.color = Color.white;
                // BacklightRing.SetActive(false); // Сделать BackLigthRing невидимым
            }
            else if (parent == null && child1 != null)
            {
                BacklightRing.SetActive(true); // Сделать BackLigthRing видимым
                BacklightRing.GetComponent<Renderer>().material.color = Color.blue; // Изменить на красный цвет
            }
            else if (parent != null && child1 == null)
            {
                BacklightRing.SetActive(true); // Сделать BackLigthRing видимым
                BacklightRing.GetComponent<Renderer>().material.color = Color.red; // Изменить на базовый цвет
            }
            else if (parent != null && child1 != null)
            {
                //BacklightRing.SetActive(true); // Сделать BackLigthRing видимым
                BacklightRing.SetActive(false); // Сделать BackLigthRing невидимым // Изменить на синий цвет

            }
        }
    }






    public  void Update()
    {
        TrackVariablesAndChangeColor();
      

    }


}
