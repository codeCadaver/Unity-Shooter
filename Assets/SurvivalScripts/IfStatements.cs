using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IfStatements : MonoBehaviour
{
    public int clickValue = 10;
    private int _clickTotal = 0;
    private string textString = "";
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            _clickTotal+=clickValue;
        }
        
        // if points is > 50
        textString = _clickTotal >= 50 ?  "You are awesome" : "You need work";
            // print "You are awesome"

        Debug.Log($"{textString}");
    }
}
