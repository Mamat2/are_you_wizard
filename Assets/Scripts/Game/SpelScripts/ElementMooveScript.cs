using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElementMooveScript : MonoBehaviour
{

    /* private Vector3 offset;
       private Rigidbody2D rb;

        private void Start()
        {

        }

        private void OnMouseDown()
        { 
            rb = gameObject.GetComponent<Rigidbody2D>();
            offset = transform.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
            rb.velocity = Vector2.zero;
        }

        private void OnMouseDrag()
        {
            Vector3 newPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + offset;
            rb.MovePosition(newPosition);
        }
    */


    private Rigidbody rb;
    private Vector3 screenPoint;
    private Vector3 offset;

    public  bool isSelected = false;


    void OnMouseDown()
    {

/*        if (Input.GetMouseButtonDown(0))
        {
            print("Left mouse button clicked!");
        }
        else if (Input.GetMouseButtonDown(1))
        {
            print("Right mouse button clicked!");
        }
                print("vbnm");*/
         /*       if (Input.GetMouseButton(1)) // Проверяем, что это правая кнопка мыши
                {

                    print("Selected: " + gameObject.name);
                }*/

        rb = GetComponent<Rigidbody>();

        screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
        offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));

        rb.velocity = Vector2.zero; 
       // rb.angularVelocity = Vector3.zero;
    }

    void OnMouseDrag()
    {
        //print("jndjffnsd");
        Vector3 curScreenPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
        Vector3 curPosition = Camera.main.ScreenToWorldPoint(curScreenPoint) + offset;
        transform.position = curPosition;
    }


/*    private void OnMouseUp()
    {
        if (Input.GetMouseButtonUp(0))
        {
            Debug.Log("Left mouse button clicked!");
        }
        else if (Input.GetMouseButtonUp(1))
        {
            Debug.Log("Right mouse button clicked!");
        }
    }
*/





    /*    private void OnMouseUp()
        {
          //  print("mouse");
            isSelected = false;
            print("is selected " + isSelected);
        }

        void OnMouseOver()
        {
            if (Input.GetMouseButtonDown(1)) // Проверяем, что это правая кнопка мыши
            {
                // Ваш код для обработки правого клика мыши здесь

                isSelected = true; 
               print("is selected " + isSelected );
            }
        }
    */

    public void Update()
    { }
/*        if (Input.GetMouseButtonDown(1))
        {
            isSelected = true;
            print("isSelected: " + isSelected + "  " + this.name);
        }

        if (Input.GetMouseButtonUp(1))
        {
            isSelected = false;
            print("isSelected: " + isSelected +"  " + this.name);
        }
    }*/
    /*    private Vector3 offset;
        private Rigidbody2D rb;
    


        void OnMouseDown()
        {
            rb = gameObject.GetComponent<Rigidbody2D>();
            rb.velocity = Vector2.zero;
            offset = gameObject.transform.position -
                  Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f));

        }

        public float distance = 10f;



        void OnMouseDrag()
        {
            Vector3 newPosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, 10.0f);
            transform.position = Camera.main.ScreenToWorldPoint(newPosition) + offset;


        }*/









}
