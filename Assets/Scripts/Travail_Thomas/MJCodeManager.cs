using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MJCodeManager : MonoBehaviour
{

    public GameObject led1;
    public GameObject led2;
    public GameObject led3;

    void Update()
    {
        if (led1.GetComponent<MJCodeLED>().isTrue == true && led2.GetComponent<MJCodeLED>().isTrue == true && led3.GetComponent<MJCodeLED>().isTrue == true)
        {
            //condition de victoire 
        }
    }
}
