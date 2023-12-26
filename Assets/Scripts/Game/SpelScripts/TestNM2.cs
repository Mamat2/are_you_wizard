using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestNM2 : MonoBehaviour
{

    TestNM script2;

    // Start is called before the first frame update
    void Start()
    {
       script2 = FindObjectOfType<TestNM>();
        print(script2.AAA);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
