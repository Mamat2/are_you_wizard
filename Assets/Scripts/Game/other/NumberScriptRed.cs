using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class NumberScriptRed : MonoBehaviour
{
    public Text roomNumber;

    // Start is called before the first frame update
    void Start()
    {
        GameManager.ChangeRoomNumberEvt.AddListener(ChangeText);
    }


    void ChangeText(int a, int b)
    {
        // print("There");
        roomNumber.GetComponent<Text>().text = b.ToString();

    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
