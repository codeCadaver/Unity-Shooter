using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TipCalculator : MonoBehaviour
{
    [SerializeField] private float bill, tipPercentage = 20.0f;

    private float _total;
    
    // Start is called before the first frame update
    void Start()
    {
        //calculate total
        _total = bill + (bill * (tipPercentage / 100));
        
        Debug.Log($"Your bill: {bill}");
        Debug.Log($"Your tip amount: {tipPercentage}");
        Debug.Log($"Your new total: {_total}");  
    }   

    // Update is called once per frame
    void Update()
    {
        
    }
}
