using System.Collections;
using System.Collections.Generic;
//using Unity.VisualScripting;
using UnityEngine;

public class BaseSpelElement : ElementMooveScript
{

    public BaseSpelElement parent;
    public BaseSpelElement child1;
    public BaseSpelElement child2;
    public string value ;
    public string secondValue;
    public GameObject mustWatch;
    public string otherConnection;
   

    public float returnSpeed = 3f;

    public LineRenderer lineRenderer; 



    private bool isVisible = false;



    void Start()
    {
       

        //BacklightRing = transform.Find("BacklightRing").gameObject;
       // BacklightRing = transform.Find("BacklightRing").gameObject;
    }


    public new virtual void Update()
    {
        base.Update();
        MustWath();
 
    }







    public BaseSpelElement Parent
    {
          get  { return parent; }
    }
    public BaseSpelElement Child1
    {
        get {  return child1; }
    }


    public BaseSpelElement(string Value)
    { 
        value = Value;
    }

    public BaseSpelElement(string Value, string SecondValue)
    {
        value = Value;
        secondValue = SecondValue;
    }

     
    public void MustWath()
    {

        if (parent != null)
        {

            if (value == "Particle" && parent.secondValue != secondValue && otherConnection == null)
            {
                if (parent.otherConnection != null)
                {
                    otherConnection = parent.otherConnection;
                }
                else
                {
                    otherConnection = parent.secondValue;
                }

            }
            WathDistance();
        }



        ReturnToParent();
       // OnRightClick();
       // TrackVariablesAndChangeColor();

    }

    private void OnBecameVisible()
    {
        isVisible = true;
    }

    private void OnRightClick()
    {
        if ( isSelected && Input.GetMouseButtonDown(1))
        {
            //print("clik vork" + this.name);
            // Ваш код для обработки правого клика мыши здесь
           
        }
    }

    private bool canReactToRightClick = false;
    private void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(1) && canReactToRightClick)
        {
            SpellManager.CastSpellEvt.Invoke(this);
            BatleManager.DoStep("Enemy");
        }
    }

    private void OnMouseEnter()
    {
        canReactToRightClick = true;
    }

    private void OnMouseExit()
    {
        canReactToRightClick = false;
    }


    /*   private void OnMouseDown()
       {
           print("mouse Down");
          // 
       }
   */
    /*  */


    private void OnBecameInvisible()
    {
        
        if (isVisible)
        {
            mustWatch = null;
            if (parent != null)
            {
                if (parent.child1 == this)
                {
                    parent.child1 = null;
                }
                else if (parent.child2 == this)
                {
                    parent.child2 = null;
                }
                if (child1 != null)
                {
                    child1.otherConnection = null;
                }
                if (child2 != null)
                {
                    child2.otherConnection = null;
                }
                LineRenderer lineRenderer = GetComponent<LineRenderer>();

                lineRenderer.SetPosition(0, transform.position);
                lineRenderer.SetPosition(1, transform.position);

              //  Destroy( GetComponent<LineRenderer>());
                parent = null;
            }
            if (child1 != null)
            {
                Destroy(child1.GetComponent<SpringJoint2D>());
                child1.otherConnection = null;
                child1.parent = null;
                child1.mustWatch = null;
                //Destroy(child1.GetComponent<LineRenderer>());

                LineRenderer lineRenderer = child1.GetComponent<LineRenderer>();

                lineRenderer.SetPosition(0, child1.transform.position);
                lineRenderer.SetPosition(1, child1.transform.position);

            }
            if (child2 != null)
            {
                Destroy(child2.GetComponent<SpringJoint2D>());
                //Destroy(child1.GetComponent<LineRenderer>());
                LineRenderer lineRenderer = child2.GetComponent<LineRenderer>();

                lineRenderer.SetPosition(0, child2.transform.position);
                lineRenderer.SetPosition(1, child2.transform.position);

                child2.otherConnection = null;
                child2.parent = null;
                child2.mustWatch = null;

            }

            Destroy(gameObject);
        }
    }


    public void WathDistance()
    {

        if (mustWatch == null)
        {
            return; // Exit the method if mustWatch is null
        }

        float tmp = Vector3.Distance(gameObject.transform.position, mustWatch.transform.position);


        LineRenderer lineRenderer = GetComponent<LineRenderer>();

       lineRenderer.SetPosition(0, transform.position);
        lineRenderer.SetPosition(1, parent.transform.position);



        if ( 0.8 < tmp )
        {
            mustWatch = null;
            if (parent.child1 == this)
            {
                parent.child1 = null;
            }
            else if (parent.child2 == this)
            {
                parent.child2 = null;
            }
            lineRenderer.SetPosition(1, lineRenderer.GetPosition(0));
            parent = null;
            //ClearInformation();
            otherConnection = null;
            Destroy(gameObject.GetComponent<SpringJoint>());
           // print("Destroed");


        }


    }
    public void ReturnToParent()
    {
        if (transform.parent != null)
        {
            Vector3 parentPos = transform.parent.position;
            Vector3 currentPos = transform.localPosition; // Используйте localPosition вместо position для локальных координат
            float distance = Mathf.Abs(parentPos.z - currentPos.z);

            // Изменение скорости возврата в зависимости от расстояния
            float adjustedSpeed = returnSpeed * Mathf.Clamp01(distance / 5f); // Измените делитель, чтобы изменить максимальное расстояние

            // Минимальная скорость возврата
            float minSpeed = 10f; // Задайте желаемое минимальное значение скорости

            // Проверка минимальной скорости
            if (adjustedSpeed < minSpeed)
            {
                adjustedSpeed = minSpeed;
            }

            // Движение к позиции родителя
            currentPos.z = Mathf.MoveTowards(currentPos.z, parentPos.z, adjustedSpeed * Time.deltaTime);
            transform.localPosition = currentPos;

            // Мгновенная остановка, когда расстояние достигает нуля
            if (distance == 0f)
            {
                returnSpeed = 0f;
            }
        }
    }

    public void ClearInformation()
    {
        mustWatch = null;
        if (parent.child1 == this)
        {
            parent.child1 = null;
        }
        else if (parent.child2 == this)
        {
            parent.child2 = null;
        }

        parent = null;

    }

    public void OnCollisionEnter(Collision collision)
    {


        //string message = gameObject.name + "  " + collision.transform.name;
        //print(message); 
      //  print("sdsdfsdf");

        if (collision.gameObject.tag == "AttachibleObj")
        {

            BatleManager.DockingRequest(gameObject, collision);
        }

    }
}
