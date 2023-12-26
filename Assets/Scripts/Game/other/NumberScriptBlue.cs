using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NumberScriptBlue : MonoBehaviour
{
    public Text roomNumber;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.ChangeRoomNumberEvt.AddListener(ChangeText);
    }


    void ChangeText(int a , int b)
    {
       // print(a);
        roomNumber.GetComponent<Text>().text = a.ToString();

    }



    // Update is called once per frame
    void Update()
    {
        
    }
}
